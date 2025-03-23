using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class StandardFee : BaseEntityId
{
    public Guid CityId { get; set; }
    
    public City? City { get; set; }
    public Guid VehicleTypeId { get; set; }
    
    public VehicleType? VehicleType { get; set; }
    
    
    [Range(0.01, 999.99)]
    public decimal FeeAmount { get; set; } = default!;
}