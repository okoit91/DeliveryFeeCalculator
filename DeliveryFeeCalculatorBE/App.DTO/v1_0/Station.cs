using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class Station
{
    public Guid Id { get; set; }
    
    public Guid? CityId { get; set; }
    
    public City? City { get; set; }

    [MaxLength(20)]
    public string StationName { get; set; } = default!;
    
    public int? WmoCode { get; set; }
    
    public bool IsActive { get; set; }
}