using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class City : BaseEntityId
{
    [MaxLength(20)]
    public string Name { get; set; } = default!;
}