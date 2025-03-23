using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class VehicleType : BaseEntityId
{
    [MaxLength(20)]
    public string Name { get; set; } = default!;
}