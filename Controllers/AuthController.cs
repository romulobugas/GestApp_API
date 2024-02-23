using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GestApp_API.Services;
using GestApp_API.Views; // Certifique-se de ajustar o namespace conforme o local do seu LoginModel

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthenticationService _authenticationService;

    public AuthController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        try
        {
            var token = await _authenticationService.AuthenticateAsync(loginModel.Username, loginModel.Password);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            // Considerar logar o erro ex para fins de depuração
            return StatusCode(500, ex.Message);
        }
    }
}
