using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class ExtraFee : IDomainEntityId
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