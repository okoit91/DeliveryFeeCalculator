using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppRole : IdentityRole<Guid>, IDomainEntityId
{
    public AppRole()
    {
        
    }

    public AppRole(string roleName, string name)
    {
        RoleName = roleName;
        Name = name;
    }
    
    [MaxLength(30)]
    public string? RoleName { get; set; }
    public override string? Name { get; set; }
    
    public ICollection<AppUser>? Users { get; set; }
    
}