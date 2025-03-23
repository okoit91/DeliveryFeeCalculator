using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace App.DTO.v1_0;

public class ExtraFee
{
    public Guid Id { get; set; }
    
    public Guid VehicleTypeId { get; set; }
    
    public VehicleType? VehicleType { get; set; } 
    
    [MaxLength(20)]
    public string ConditionType { get; set; } = default!;
    
    [Range(0.01, 999.99)]
    public decimal? MinValue { get; set; }
    
    [Range(0.01, 999.99)]
    public decimal? MaxValue { get; set; }
    
    [Range(0.01, 999.99)]
    public decimal FeeAmount { get; set; }
}