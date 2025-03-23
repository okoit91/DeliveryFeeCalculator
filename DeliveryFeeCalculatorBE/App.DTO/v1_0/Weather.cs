using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class Weather
{
    public Guid Id { get; set; }
    
    public Guid StationId { get; set; }
    
    public Station? Station { get; set; }

    [Range(0.01, 999.99)]
    public decimal? AirTemperature { get; set; }
    
    [Range(0.01, 999.99)]
    public decimal? WindSpeed { get; set; }
    
    [MaxLength(30)]
    public string? WeatherCondition { get; set; }
    
    public DateTime Timestamp { get; set; }
    
} 