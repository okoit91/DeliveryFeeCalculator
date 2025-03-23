using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class DeliveryRequest : IDomainEntityId

{
    
    public Guid Id { get; set; }
    
    public Guid CityId { get; set; }
    
    public Guid VehicleTypeId { get; set; }
    
    public DateTime Timestamp { get; set; }
    
    [Range(0.01, 999.99)]
    public decimal? SumFee { get; set; } = default!;
    
}