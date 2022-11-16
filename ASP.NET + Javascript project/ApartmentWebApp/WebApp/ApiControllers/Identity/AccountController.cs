using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using App.DAL.EF;
using App.Domain.Identity;
using Base.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using AppUser = App.Domain.Identity.AppUser;

// using WebApp.DTO;
// using WebApp.DTO.Identity;

namespace WebApp.ApiControllers.Identity;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly Random _rnd = new();
    private readonly AppDbContext _context;


    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        ILogger<AccountController> logger, IConfiguration configuration, AppDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
        _configuration = configuration;
        _context = context;
    }
    
    /// <summary>
    /// Login into the rest backend - generates JWT to be included in
    /// Authorize: Bearer xyz
    /// </summary>
    /// <param name="loginData">Supply email and password</param>
    /// <returns>JWT and refresh token</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Identity.JwtResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Identity.Message), StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<JwtResponse>> LogIn([FromBody] Login loginData)
    {
        var appUser = await _userManager.FindByEmailAsync(loginData.Email);
        if (appUser == null)
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(new App.Public.DTO.v1.Identity.Message("User/Password problem!"));
        }

        // verify username and password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginData.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed, password problem for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(new App.Public.DTO.v1.Identity.Message("User/Password problem"));
        }

        var refreshToken = new RefreshToken();

        appUser.RefreshTokens = new List<RefreshToken>() {refreshToken};
        var updateTokenResult = await _userManager.UpdateAsync(appUser);

        if (updateTokenResult == null)
        {
            _logger.LogWarning("Could not update refreshToken for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(new App.Public.DTO.v1.Identity.Message("User/Password problem"));
        }

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(new App.Public.DTO.v1.Identity.Message("User/Password problem"));
        }


        // generate jwt
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes"))
        );

        var res = new JwtResponse()
        {
            Token = jwt,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            RefreshToken = refreshToken.Token
        };

        return Ok(res);
    }
    
    /// <summary>
    /// Register an account in the rest backend - generates JWT to be included in
    /// Authorize: Bearer xyz
    /// </summary>
    /// <param name="registrationData">Supply email, password, first name and last name</param>
    /// <returns>JWT, refresh token, first name, lastname</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Identity.JwtResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Identity.Message), StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<JwtResponse>> Register(Register registrationData)
    {
        // verify user


        var appUser = await _userManager.FindByEmailAsync(registrationData.Email);
        if (appUser != null)
        {
            _logger.LogWarning("User with email {} is already registered", registrationData.Email);

            return BadRequest(ErrorFormatBadRequest("email", "Email already registered"));
        }

        var refreshToken = new RefreshToken();
        appUser = new AppUser()
        {
            FirstName = registrationData.FirstName,
            LastName = registrationData.LastName,
            Email = registrationData.Email,
            UserName = registrationData.Email,
            RefreshTokens = new List<RefreshToken>()
            {
                refreshToken
            }
        };
        // create user (system will dop it)
        var result = await _userManager.CreateAsync(appUser, registrationData.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        result = await _userManager.AddClaimAsync(appUser, new Claim("aspnet.firstname", appUser.FirstName));
        result = await _userManager.AddClaimAsync(appUser, new Claim("aspnet.lastname", appUser.LastName));
        result = await _userManager.AddToRoleAsync(appUser, "admin");
        
        // get full user from system with fixed data
        appUser = await _userManager.FindByEmailAsync(appUser.Email);

        if (appUser == null)
        {
            _logger.LogWarning("User with email {} is not found after registration", registrationData.Email);

            return BadRequest(ErrorFormatBadRequest("email",
                $"User with email {registrationData.Email} is not found after registration"));
        }

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", registrationData.Email);
            return NotFound("User/Password problem");
        }


        // generate jwt from claims
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes"))
        );

        var res = new JwtResponse()
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName
        };

        return Ok(res);
    }
    
    
    /// <summary>
    /// Refreshes token and jwt
    /// </summary>
    /// <param name="refreshTokenModel">Supply a refresh token model</param>
    /// <returns>JWT, refresh token, first name, lastname</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Identity.JwtResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Identity.Message), StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel)
    {
       
        // get info from jwt
        JwtSecurityToken jwtToken;
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Jwt);

            if (jwtToken == null)
            {
               

                return BadRequest(ErrorFormatBadRequest("jwtToken", "No token"));
            }
        }
        catch (Exception e)
        {
            return BadRequest(ErrorFormatBadRequest("jwtToken",   $"Can't parse the token, {e.Message}"));
        }

        var tokenHandler = new JwtSecurityTokenHandler();

        // TODO: validate token signature
        // jwtToken.

        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        if (userEmail == null)
        {
            return BadRequest(ErrorFormatBadRequest("userEmail","No email in jwt"));
        }

        // get user and tokens
        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            return BadRequest(ErrorFormatBadRequest("userEmail", $"User with email {userEmail} not found"));
        }

        // load and compare refresh tokens
        await _context.Entry(appUser).Collection(u => u.RefreshTokens!)
            .Query()
            .Where(x => (x.Token == refreshTokenModel.RefreshToken && x.ExpirationDateTime > DateTime.UtcNow) ||
                        (x.PreviousToken == refreshTokenModel.RefreshToken &&
                         x.PreviousExpirationDateTime > DateTime.UtcNow))
            .ToListAsync();

        if (appUser.RefreshTokens == null)
        {
            return BadRequest(ErrorFormatBadRequest("RefreshTokens", "RefreshTokens collection is null"));
        }

        if (appUser.RefreshTokens.Count == 0)
        {
            return Problem("RefreshTokens collection is empty, no valid refresh tokens found");
        }

        if (appUser.RefreshTokens.Count != 1)
        {
            return Problem("More than one valid refresh token found.");
        }

        // generate new jwt

        // generate jwt from claims
        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", userEmail);
            return NotFound("User/Password problem");
        }

        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes"))
        );

        // create new refresh token, obsolete old ones
        var refreshToken = appUser.RefreshTokens.First();
        if (refreshToken.Token == refreshTokenModel.RefreshToken)
        {
            refreshToken.PreviousToken = refreshToken.Token;
            refreshToken.PreviousExpirationDateTime = DateTime.UtcNow.AddMinutes(1);

            refreshToken.Token = Guid.NewGuid().ToString();
            refreshToken.ExpirationDateTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();
        }

        var res = new JwtResponse()
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName
        };
        return Ok(res);
    }

    private RestApiErrorResponse ErrorFormatBadRequest(string errorName, string error)
    {
        var errorRespones = new RestApiErrorResponse()
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Title = "App error",
            Status = HttpStatusCode.BadRequest,
            TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
        };
        errorRespones.Errors[errorName] = new List<string>()
        {
            error
        };

        return errorRespones;
    }
}