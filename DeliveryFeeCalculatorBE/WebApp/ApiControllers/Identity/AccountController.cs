using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using App.DAL.EF;
using App.Domain.Identity;
using App.DTO.v1_0;
using App.DTO.v1_0.Identity;
using Asp.Versioning;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.Identity;

[ApiVersion("1.0")]
[ApiController]
[Route("/api/v{version:apiVersion}/identity/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public AccountController(UserManager<AppUser> userManager, ILogger<AccountController> logger,
        SignInManager<AppUser> signInManager, IConfiguration configuration, AppDbContext context)
    {
        _userManager = userManager;
        _logger = logger;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }


    /// <summary>
    /// Register new local user into app.
    /// </summary>
    /// <param name="registrationData">Username and password. And personal details.</param>
    /// <param name="expiresInSeconds">Override jwt lifetime for testing.</param>
    /// <returns>JWTResponse - jwt and refresh token</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType<JWTResponse>((int) HttpStatusCode.OK)]
    [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.BadRequest)]
    public async Task<ActionResult<JWTResponse>> Register(
        [FromBody]
        RegisterInfo registrationData,
        [FromQuery]
        int expiresInSeconds)
    {
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;
        expiresInSeconds = expiresInSeconds < _configuration.GetValue<int>("JWT:expiresInSeconds")
            ? expiresInSeconds
            : _configuration.GetValue<int>("JWT:expiresInSeconds");


        // is user already registered
        var appUser = await _userManager.FindByEmailAsync(registrationData.Email);
        if (appUser != null)
        {
            _logger.LogWarning("User with email {} is already registered", registrationData.Email);
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = $"User with email {registrationData.Email} is already registered"
                }
            );
        }

        // register user
        var refreshToken = new AppRefreshToken();
        appUser = new AppUser()
        {
            Email = registrationData.Email,
            UserName = registrationData.Email,
            FirstName = registrationData.Firstname,
            LastName = registrationData.Lastname,
            RefreshTokens = new List<AppRefreshToken>() {refreshToken},
        };

        var result = await _userManager.CreateAsync(appUser, registrationData.Password);
        if (!result.Succeeded)
        {
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = result.Errors.First().Description
                }
            );
        }
        
        result = await _userManager.AddClaimsAsync(appUser, new List<Claim>()
        {
            new(ClaimTypes.GivenName, appUser.FirstName),
            new(ClaimTypes.Surname, appUser.LastName)
        });

        
        await _userManager.AddToRoleAsync(appUser, "client");
        if (!result.Succeeded)
        {
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = result.Errors.First().Description
                }
            );
        }

        // get full user from system with fixed data.
        appUser = await _userManager.FindByEmailAsync(appUser.Email);
        if (appUser == null)
        {
            _logger.LogWarning("User with email {} is not found after registration", registrationData.Email);
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = $"User with email {registrationData.Email} is not found after registration"
                }
            );
        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        var jwt = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration.GetValue<string>("JWT:key"),
            _configuration.GetValue<string>("JWT:issuer"),
            _configuration.GetValue<string>("JWT:audience"),
            expiresInSeconds
        );
        var res = new JWTResponse()
        {
            Jwt = jwt,
            RefreshToken = refreshToken.RefreshToken,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
        };
        return Ok(res);
    }
    
/// <summary>
/// Assigns a specified role to the current user.
/// </summary>
/// <param name="roleAssignment">The role assignment details.</param>
/// <returns>A response indicating whether the operation was successful.</returns>
[HttpPost]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public async Task<IActionResult> SetRole([FromBody] SetRoleModel roleAssignment)
{
    if (roleAssignment == null || string.IsNullOrWhiteSpace(roleAssignment.Role))
    {
        return BadRequest(new RestApiErrorResponse
        {
            Status = HttpStatusCode.BadRequest,
            Error = "Invalid role specified."
        });
    }

    if (roleAssignment.Role == "Admin" || roleAssignment.Role == "Moderator")
    {
        return BadRequest(new RestApiErrorResponse
        {
            Status = HttpStatusCode.BadRequest,
            Error = "Assigning roles 'Admin' or 'Moderator' is not permitted."
        });
    }

    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId))
    {
        return BadRequest(new RestApiErrorResponse
        {
            Status = HttpStatusCode.BadRequest,
            Error = "User ID not found in the token."
        });
    }

    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
        return NotFound(new RestApiErrorResponse
        {
            Status = HttpStatusCode.NotFound,
            Error = "User not found."
        });
    }

    var result = await _userManager.AddToRoleAsync(user, roleAssignment.Role);
    if (result.Succeeded)
    {
        return Ok();
    }

    return BadRequest(new RestApiErrorResponse
    {
        Status = HttpStatusCode.BadRequest,
        Error = string.Join(", ", result.Errors.Select(e => e.Description))
    });
}
    
    /// <summary>
    /// checks if the user is in a role
    /// </summary>
    /// <param name="userId">takes in user id</param>
    /// <param name="role"> takes in the role that will be checked</param>
    /// <returns>returns ok if user is in specific role</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<bool>> IsInRole([FromQuery] string role)
    {
        if (string.IsNullOrEmpty(role))
        {
            return BadRequest("Role must be provided.");
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token.");
        }
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        
        var isInRole = await _userManager.IsInRoleAsync(user, role);
        
        return Ok(isInRole);
    }

    /// <summary>
    /// Logs the user in and returns jwt and refresh token.
    /// </summary>
    /// <param name="loginInfo">Takes in email and password</param>
    /// <param name="expiresInSeconds">takes in Jwt expire time</param>
    /// <returns>Login data</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType<ActionResult<JWTResponse>>((int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<JWTResponse>> Login(
    [FromBody] LoginInfo loginInfo,
    [FromQuery] int expiresInSeconds
)
{
    if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;
    expiresInSeconds = expiresInSeconds < _configuration.GetValue<int>("JWT:expiresInSeconds")
        ? expiresInSeconds
        : _configuration.GetValue<int>("JWT:expiresInSeconds");

    // Verify user
    var appUser = await _userManager.FindByEmailAsync(loginInfo.Email);
    if (appUser == null)
    {
        _logger.LogWarning("WebApi login failed, email {} not found", loginInfo.Email);
        return Unauthorized(new { error = "Invalid credentials" });
    }

    // Verify password
    var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginInfo.Password, false);
    if (!result.Succeeded)
    {
        _logger.LogWarning("WebApi login failed, incorrect password for email {}", loginInfo.Email);
        return Unauthorized(new { error = "Invalid credentials" });
    }

    var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
    if (claimsPrincipal == null)
    {
        _logger.LogWarning("WebApi login failed, claimsPrincipal null");
        return Unauthorized(new { error = "Invalid credentials" });
    }

    if (!_context.Database.ProviderName!.Contains("InMemory"))
    {
        var deletedRows = await _context.AppRefreshTokens
            .Where(t => t.AppUserId == appUser.Id && t.ExpirationDT < DateTime.UtcNow)
            .ExecuteDeleteAsync();
        _logger.LogInformation("Deleted {} refresh tokens", deletedRows);
    }

    var refreshToken = new AppRefreshToken()
    {
        AppUserId = appUser.Id
    };
    _context.AppRefreshTokens.Add(refreshToken);
    await _context.SaveChangesAsync();

    var jwt = IdentityHelpers.GenerateJwt(
        claimsPrincipal.Claims,
        _configuration.GetValue<string>("JWT:key"),
        _configuration.GetValue<string>("JWT:issuer"),
        _configuration.GetValue<string>("JWT:audience"),
        expiresInSeconds
    );

    var responseData = new JWTResponse()
    {
        Jwt = jwt,
        RefreshToken = refreshToken.RefreshToken,
        FirstName = appUser.FirstName,
        LastName = appUser.LastName,
        Email = appUser.Email
    };

    return Ok(responseData);
}
    /// <summary>
    ///  Refreshes the jwt token.
    /// </summary>
    /// <param name="tokenRefreshInfo">takes in refresh token</param>
    /// <param name="expiresInSeconds"> takes in expire time</param>
    /// <returns>Jwt and new refresh token</returns>
    [HttpPost]
    [ProducesResponseType<ActionResult<JWTResponse>>((int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JWTResponse>> RefreshTokenData(
        [FromBody]
        TokenRefreshInfo tokenRefreshInfo,
        [FromQuery]
        int expiresInSeconds
    )
    {
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;
        expiresInSeconds = expiresInSeconds < _configuration.GetValue<int>("JWT:expiresInSeconds")
            ? expiresInSeconds
            : _configuration.GetValue<int>("JWT:expiresInSeconds");

        
        JwtSecurityToken? jwt;
        try
        {
            jwt = new JwtSecurityTokenHandler().ReadJwtToken(tokenRefreshInfo.Jwt);
            if (jwt == null)
            {
                return BadRequest(
                    new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.BadRequest,
                        Error = "No token"
                    }
                );
            }
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "No token"
                }
            );
        }

        // validate jwt, ignore expiration date

        if (!IdentityHelpers.ValidateJwt(
                tokenRefreshInfo.Jwt,
                _configuration.GetValue<string>("JWT:key"),
                _configuration.GetValue<string>("JWT:issuer"),
                _configuration.GetValue<string>("JWT:audience")
            ))
        {
            return BadRequest("JWT validation fail");
        }

        var userEmail = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "No email in jwt"
                }
            );
        }

        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            return NotFound("User with email {userEmail} not found");
        }
        
        await _context.Entry(appUser).Collection(u => u.RefreshTokens!)
            .Query()
            .Where(x =>
                (x.RefreshToken == tokenRefreshInfo.RefreshToken && x.ExpirationDT > DateTime.UtcNow) ||
                (x.PreviousRefreshToken == tokenRefreshInfo.RefreshToken &&
                 x.PreviousExpirationDT > DateTime.UtcNow)
            )
            .ToListAsync();

        if (appUser.RefreshTokens == null || appUser.RefreshTokens.Count == 0)
        {
            return NotFound(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = $"RefreshTokens collection is null or empty - {appUser.RefreshTokens?.Count}"
                }
            );
        }

        if (appUser.RefreshTokens.Count != 1)
        {
            return NotFound("More than one valid refresh token found");
        }

        
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", userEmail);
            return NotFound(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "User/Password problem"
                }
            );
        }
        
        var jwtResponseStr = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration.GetValue<string>("JWT:key"),
            _configuration.GetValue<string>("JWT:issuer"),
            _configuration.GetValue<string>("JWT:audience"),
            expiresInSeconds
        );

        // make new refresh token, keep old one still valid for some time
        var refreshToken = appUser.RefreshTokens.First();
        if (refreshToken.RefreshToken == tokenRefreshInfo.RefreshToken)
        {
            refreshToken.PreviousRefreshToken = refreshToken.RefreshToken;
            refreshToken.PreviousExpirationDT = DateTime.UtcNow.AddMinutes(1);

            refreshToken.RefreshToken = Guid.NewGuid().ToString();
            refreshToken.ExpirationDT = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();
        }

        var res = new JWTResponse()
        {
            Jwt = jwtResponseStr,
            RefreshToken = refreshToken.RefreshToken,
        };

        return Ok(res);
    }
    
    /// <summary>
    /// Logs the user out.
    /// </summary>
    /// <param name="logout">Takes in the logoutinfo</param>
    /// <returns>Removes the refresh token</returns>
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Logout(
        [FromBody]
        LogoutInfo logout)
    {
        // delete the refresh token - so user is kicked out after jwt expiration
        // We do not invalidate the jwt on serverside -
        // that would require pipeline modification and checking against db on every request
        
        // so client can actually continue to use the jwt until it expires (keep the jwt expiration time short ~1 min)

        var userIdStr = _userManager.GetUserId(User);
        if (userIdStr == null)
        {
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Invalid refresh token"
                }
            );
        }

        if (Guid.TryParse(userIdStr, out var userId))
        {
            return BadRequest("Deserialization error");
        }

        var appUser = await _context.Users
            .Where(u => u.Id == userId)
            .SingleOrDefaultAsync();
        if (appUser == null)
        {
            return NotFound(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User/Password problem"
                }
            );
        }

        await _context.Entry(appUser)
            .Collection(u => u.RefreshTokens!)
            .Query()
            .Where(x =>
                (x.RefreshToken == logout.RefreshToken) ||
                (x.PreviousRefreshToken == logout.RefreshToken)
            )
            .ToListAsync();

        foreach (var appRefreshToken in appUser.RefreshTokens!)
        {
            _context.AppRefreshTokens.Remove(appRefreshToken);
        }

        var deleteCount = await _context.SaveChangesAsync();

        return Ok(new {TokenDeleteCount = deleteCount});
    }
}
