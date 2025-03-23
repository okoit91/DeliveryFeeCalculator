using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.BLL.DTO;

public class AppRole : IdentityRole<Guid>, IDomainEntityId
{
    
}