using Quartz;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using App.BLL.DTO;
using App.Contracts.BLL;
using App.Contracts.DAL;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;

public class WeatherDataJob : IJob
{
    private readonly ILogger<WeatherDataJob> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAppBLL _bll;
    private readonly string _weatherDataUrl;
    private readonly IMapper _mapper;
    
    private static readonly string[] TargetStations = { "Tallinn-Harku", "Tartu-Tõravere", "Pärnu" };

    public WeatherDataJob(
        ILogger<WeatherDataJob> logger,
        IHttpClientFactory httpClientFactory,
        IAppBLL bll,
        IConfiguration configuration,
        IMapper mapper)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _bll = bll;
        _weatherDataUrl = configuration["WeatherData:Url"]
                          ?? "https://www.ilmateenistus.ee/ilma_andmed/xml/observations.php";
        _mapper = mapper;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("WeatherDataJob starting...");

            using var client = _httpClientFactory.CreateClient();
            var xmlData = await client.GetStringAsync(_weatherDataUrl);

            var observations = await ParseWeatherData(xmlData);
            
            if (observations.Any())
            {
                await SaveWeatherDataToDb(observations);
                _logger.LogInformation("Weather data saved successfully.");
            }
            else
            {
                _logger.LogWarning("No matching weather data found for specified stations.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching/parsing/saving weather data.");
        }
    }
    
    private async Task<List<Weather>> ParseWeatherData(string xmlData)
    {
        var doc = XDocument.Parse(xmlData);
        var stationElements = doc.Descendants("station");

        var weatherDataList = new List<Weather>();

        foreach (var station in stationElements)
        {
            var stationName = station.Element("name")?.Value;
            if (!TargetStations.Contains(stationName)) continue;

            var stationEntity = await GetStationByNameAsync(stationName);

            if (stationEntity == null)
            {
                _logger.LogWarning($"No matching station found for weather data: {stationName}");
                continue;
            }

            var weather = new Weather
            {
                Id = Guid.NewGuid(),
                StationId = stationEntity.Id,
                AirTemperature = decimal.TryParse(station.Element("airtemperature")?.Value, 
                    NumberStyles.Any, CultureInfo.InvariantCulture, out decimal temp) ? temp : (decimal?)null,
                WindSpeed = decimal.TryParse(station.Element("windspeed")?.Value, 
                    NumberStyles.Any, CultureInfo.InvariantCulture, out decimal wind) ? wind : (decimal?)null,
                WeatherCondition = station.Element("phenomenon")?.Value,
                Timestamp = DateTime.UtcNow
            };

            weatherDataList.Add(weather);
        }

        return weatherDataList;
    }
    
    private async Task SaveWeatherDataToDb(List<Weather> weatherData)
    {
        var mappedWeatherData = weatherData
            .Select(w => _mapper.Map<Weather>(w)!)
            .ToList();

        foreach (var weather in mappedWeatherData)
        {
            _bll.Weathers.Add(weather);
        }

        await _bll.SaveChangesAsync();
    }
    
    private async Task<Station?> GetStationByNameAsync(string stationName)
    {
        return await _bll.Stations.FirstOrDefaultByNameAsync(stationName);
    }
    
}
