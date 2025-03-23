using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class AppUser : IDomainEntityId
{
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [MaxLength(64)]
    public string FirstName { get; set; } = default!;
    [MinLength(1)]
    [MaxLength(64)]
    public string LastName { get; set; } = default!;

}