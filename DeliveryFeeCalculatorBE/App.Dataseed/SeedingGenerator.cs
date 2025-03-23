using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Dataseed;

public class SeedingGenerator
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IMapper _mapper;

    public SeedingGenerator(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }
    
    string[] _roles = { "client", "admin" };
    private readonly string[] _vehicleTypes = { "Car", "Scooter", "Bike" };
    
    private readonly Dictionary<string, List<(string StationName, int? WmoCode)>> _cityStations =
        new Dictionary<string, List<(string, int?)>>
        {
            { "Tallinn", new List<(string, int?)> { ("Tallinn-Harku", 26038) } },
            { "Tartu",   new List<(string, int?)> { ("Tartu-Tõravere", 26242) } },
            { "Pärnu",   new List<(string, int?)> { ("Pärnu", 41803) } }
        };

    
    private readonly Dictionary<string, Dictionary<string, decimal>> _standardFees = new()
    {
        { "Tallinn", new Dictionary<string, decimal> { { "Car", 4 }, { "Scooter", 3.5m }, { "Bike", 3 } } },
        { "Tartu", new Dictionary<string, decimal> { { "Car", 3.5m }, { "Scooter", 3 }, { "Bike", 2.5m } } },
        { "Pärnu", new Dictionary<string, decimal> { { "Car", 3 }, { "Scooter", 2.5m }, { "Bike", 2 } } }
    };
    
    
    public async Task StartSeeding()
    {
        await AddRolesAsync();
        await CreateAdminUserAsync();
        await CreateCitiesAndStationsAsync();
        await CreateVehicleTypesAsync();
        await CreateStandardFeesAsync();
        await CreateExtraFeesAsync();
    }
    
    public async Task AddRolesAsync()
    {
        

        foreach (var role in _roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var appRole = new AppRole
                {
                    Name = role,
                    RoleName = role,
                    NormalizedName = role.ToUpper()
                };
                await _roleManager.CreateAsync(appRole);
            }
        }
    }
    
    public async Task CreateAdminUserAsync()
    {
        string? adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
        string? adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

        if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
        {
            Console.WriteLine("Admin credentials not set. Skipping admin user creation.");
            return;
        }

        var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin == null)
        {
            var adminUser = new AppUser
            {
                Email = adminEmail,
                UserName = adminEmail,
                FirstName = "System",
                LastName = "Admin",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "admin");
                Console.WriteLine($"Admin user {adminEmail} created successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to create admin user: {result.Errors}");
            }
        }
        else
        {
            Console.WriteLine($"Admin user {adminEmail} already exists.");
        }
    }
    
    public async Task CreateCitiesAndStationsAsync()
    {
        foreach (var kvp in _cityStations)
        {
            var cityName = kvp.Key;
            var stationInfoList = kvp.Value;
            
            var existingCity = await _context.Cities
                .FirstOrDefaultAsync(c => c.Name == cityName);

            if (existingCity == null)
            {
                existingCity = new City { Name = cityName };
                _context.Cities.Add(existingCity);
            }
            
            foreach (var (stationName, wmoCode) in stationInfoList)
            {
                var existingStation = await _context.Stations
                    .FirstOrDefaultAsync(s =>
                        s.StationName == stationName &&
                        s.CityId == existingCity.Id);

                if (existingStation == null)
                {
                    Station station = new Station
                    {
                        StationName = stationName,
                        CityId = existingCity.Id,
                        WmoCode = wmoCode,
                        IsActive = true
                    };
                    
                    _context.Stations.Add(station);
                }
            }
        }
        
        await _context.SaveChangesAsync();
    }
        
    private async Task CreateVehicleTypesAsync()
    {
        foreach (var typeName in _vehicleTypes)
        {
            var existingType = await _context.VehicleTypes.FirstOrDefaultAsync(v => v.Name == typeName);

            if (existingType == null)
            {
                VehicleType vehicleType = new VehicleType { Name = typeName };
                _context.VehicleTypes.Add(vehicleType);
            }
        }

        await _context.SaveChangesAsync();
    }
    
    private async Task CreateStandardFeesAsync()
    {
        foreach (var (cityName, vehicleFees) in _standardFees)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == cityName);
            if (city == null) continue; // Skip if city not found

            foreach (var (vehicleTypeName, feeAmount) in vehicleFees)
            {
                var vehicleType = await _context.VehicleTypes.FirstOrDefaultAsync(
                    v => v.Name == vehicleTypeName);
                if (vehicleType == null) continue; // Skip if vehicle type not found

                var existingFee = await _context.StandardFees
                    .FirstOrDefaultAsync(sf => sf.CityId == city.Id && sf.VehicleTypeId == vehicleType.Id);

                if (existingFee == null)
                {
                    StandardFee standardFee = new StandardFee
                    {
                        CityId = city.Id,
                        VehicleTypeId = vehicleType.Id,
                        FeeAmount = feeAmount
                    };
                    _context.StandardFees.Add(standardFee);
                }
            }
        }

        await _context.SaveChangesAsync();
    }
    
    private async Task CreateExtraFeesAsync()
{
    var scooter = await _context.VehicleTypes.FirstOrDefaultAsync(v => v.Name == "Scooter");
    var bike = await _context.VehicleTypes.FirstOrDefaultAsync(v => v.Name == "Bike");

    if (scooter == null || bike == null) return;

    var defaultExtraFees = new List<ExtraFee>
    {
        // Air Temperature Fees
        new ExtraFee { VehicleTypeId = scooter.Id, ConditionType = "Temperature", MinValue = -100, MaxValue = -10, FeeAmount = 1 },
        new ExtraFee { VehicleTypeId = scooter.Id, ConditionType = "Temperature", MinValue = -10, MaxValue = 0, FeeAmount = 0.5m },
        new ExtraFee { VehicleTypeId = bike.Id, ConditionType = "Temperature", MinValue = -100, MaxValue = -10, FeeAmount = 1 },
        new ExtraFee { VehicleTypeId = bike.Id, ConditionType = "Temperature", MinValue = -10, MaxValue = 0, FeeAmount = 0.5m },

        // Wind Speed Fees
        new ExtraFee { VehicleTypeId = bike.Id, ConditionType = "WindSpeed", MinValue = 10, MaxValue = 20, FeeAmount = 0.5m },

        // Weather Condition Fees
        new ExtraFee { VehicleTypeId = scooter.Id, ConditionType = "snow", MinValue = null, MaxValue = null, FeeAmount = 1 },
        new ExtraFee { VehicleTypeId = scooter.Id, ConditionType = "rain", MinValue = null, MaxValue = null, FeeAmount = 0.5m },
        new ExtraFee { VehicleTypeId = scooter.Id, ConditionType = "sleet", MinValue = null, MaxValue = null, FeeAmount = 1 },
        
        new ExtraFee { VehicleTypeId = bike.Id, ConditionType = "snow", MinValue = null, MaxValue = null, FeeAmount = 1 },
        new ExtraFee { VehicleTypeId = bike.Id, ConditionType = "rain", MinValue = null, MaxValue = null, FeeAmount = 0.5m },
        new ExtraFee { VehicleTypeId = bike.Id, ConditionType = "sleet", MinValue = null, MaxValue = null, FeeAmount = 1 } 
    };

    foreach (var fee in defaultExtraFees)
    {
        var exists = await _context.ExtraFees.FirstOrDefaultAsync(f =>
            f.VehicleTypeId == fee.VehicleTypeId && f.ConditionType == fee.ConditionType &&
            f.MinValue == fee.MinValue && f.MaxValue == fee.MaxValue);

        if (exists == null)
        {
            _context.ExtraFees.Add(fee);
        }
    }

    await _context.SaveChangesAsync();
}
    
}