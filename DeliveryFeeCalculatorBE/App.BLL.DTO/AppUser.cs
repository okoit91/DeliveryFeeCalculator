﻿using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;
namespace App.BLL.DTO;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    [MinLength(1)]
    [MaxLength(64)]
    public string FirstName { get; set; } = default!;
    
    [MinLength(1)]
    [MaxLength(64)]
    public string LastName { get; set; } = default!;
    
}