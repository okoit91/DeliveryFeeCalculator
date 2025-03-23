using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class ExtraFee : BaseEntityId
{
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