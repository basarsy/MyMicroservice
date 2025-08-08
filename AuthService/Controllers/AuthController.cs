using AuthService.Dtos;
using AuthService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService;

[Route("api/{controller}")]
[ApiController]

public class AuthController : ControllerBase
{
    private readonly JwtTokenService _tokenService;
    private readonly HttpClient _httpClient;
    
    public AuthController(JwtTokenService tokenService, IHttpClientFactory httpClientFactory)
    {
        _tokenService = tokenService;
        _httpClient=httpClientFactory.CreateClient("UserService");
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserAuthDto userAuthDto)
    {
        var response = await _httpClient.GetAsync($"api/user/auth/{userAuthDto.UserName}");
        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("User not found.");
        }

        var user = await response.Content.ReadFromJsonAsync<UserAuthDto>();
        
        var hasher = new PasswordHasher<UserAuthDto>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, userAuthDto.UserPassword);
        if (result != PasswordVerificationResult.Success)
        {
            return Unauthorized("User password is wrong.");
        }

        var token = _tokenService.GenerateToken();
        return Ok(new { token } );
    }
}