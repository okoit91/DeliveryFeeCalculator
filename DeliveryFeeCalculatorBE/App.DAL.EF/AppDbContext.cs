using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppRole = App.Domain.Identity.AppRole;
using AppUser = App.Domain.Identity.AppUser;
using City = App.Domain.City;
using ExtraFee = App.Domain.ExtraFee;
using StandardFee = App.Domain.StandardFee;
using VehicleType = App.Domain.VehicleType;
using Weather = App.Domain.Weather;
using Station = App.Domain.Station;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>,
    AppUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public DbSet<City> Cities { get; set; } = default!;
    
    public DbSet<ExtraFee> ExtraFees { get; set; } = default!;
    
    public DbSet<StandardFee> StandardFees { get; set; } = default!;
    
    public DbSet<VehicleType> VehicleTypes { get; set; } = default!;
    
    public DbSet<Weather> Weathers { get; set; } = default!;
    public DbSet<Station> Stations { get; set; } = default!;
    
    public DbSet<AppUser> AppUsers { get; set; } = default!;
    public DbSet<AppRole> AppRoles { get; set; } = default!;

    public DbSet<AppRefreshToken> AppRefreshTokens { get; set; } = default!;
    
    public AppDbContext(DbContextOptions options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        foreach (var relationship in builder.Model
                     .GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                var properties = entry.Properties.Where(p => p.Metadata.ClrType == typeof(DateTime) || p.Metadata.ClrType == typeof(DateTime?));
                foreach (var prop in properties)
                {
                    if (prop.CurrentValue != null) // Check for null if nullable
                    {
                        var dateTime = (DateTime)prop.CurrentValue;
                        prop.CurrentValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    }
                }
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

}