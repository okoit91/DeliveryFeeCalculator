﻿using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class VehicleType : IDomainEntityId
{
    public Guid Id { get; set; }
    
    [MaxLength(20)]
    public string Name { get; set; } = default!;
}