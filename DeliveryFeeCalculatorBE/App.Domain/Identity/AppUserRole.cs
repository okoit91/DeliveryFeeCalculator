using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppUserRole : IdentityUserRole<Guid>
{
    public virtual string? UserName { get; set; }
    public virtual string? RoleName { get; set; }
}