using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class AppRole : IDomainEntityId
{
    public Guid Id { get; set; }
    
    [MaxLength(30)]
    public string? RoleName { get; set; }
    
    public ICollection<AppUser>? Users { get; set; }
    
}