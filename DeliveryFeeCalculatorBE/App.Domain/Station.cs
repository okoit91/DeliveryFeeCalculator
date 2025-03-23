using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Station : BaseEntityId
{
    public Guid? CityId { get; set; }
    
    public City? City { get; set; }

    [MaxLength(20)] public string StationName { get; set; } = default!;
    
    public int? WmoCode { get; set; }
    
    public bool IsActive { get; set; }
}