using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Weather : BaseEntityId
{
    
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