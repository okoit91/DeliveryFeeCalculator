using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Station : IDomainEntityId
{
    public Guid Id { get; set; }
    
    public Guid? CityId { get; set; }
    
    public City? City { get; set; }

    [MaxLength(20)]
    public string StationName { get; set; } = default!;
    
    public int? WmoCode { get; set; }
    
    public bool IsActive { get; set; }
}