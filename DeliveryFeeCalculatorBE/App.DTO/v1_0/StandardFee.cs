using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class StandardFee
{
    public Guid Id { get; set; }
    
    public Guid CityId { get; set; }
    
    public City? City { get; set; }
    
    public Guid VehicleTypeId { get; set; }
    
    public VehicleType? VehicleType { get; set; }
    
    [Range(0.01, 999.99)]
    public decimal FeeAmount { get; set; } = default!;
}