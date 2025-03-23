using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class UserRequest : IDomainEntityId
{
    public Guid Id { get; set; }
    
    public Guid AppUserId { get; set; }
    
    public AppUser? AppUser { get; set; }
    
    public Guid DeliveryRequestId { get; set; }

    
}