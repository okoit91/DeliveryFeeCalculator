using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class StandardFee : IDomainEntityId
{
    public Guid Id { get; set; }
    
    public Guid CityId { get; set; }
    
    public City? City { get; set; }
    
    public Guid VehicleTypeId { get; set; }
    
    public VehicleType? VehicleType { get; set; }
    
    [Range(0.01, 999.99)]
    public decimal FeeAmount { get; set; } = default!;

    
}