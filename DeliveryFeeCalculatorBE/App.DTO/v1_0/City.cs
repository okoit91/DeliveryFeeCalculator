using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class City
{
    public Guid Id { get; set; }
    
    [MaxLength(20)]
    public string Name { get; set; } = default!;
}