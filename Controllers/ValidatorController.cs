using GestApp_API.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ValidatorController : ControllerBase
{
    private readonly TokenValidationService _tokenValidationService;

    public ValidatorController(TokenValidationService tokenValidationService)
    {
        _tokenValidationService = tokenValidationService;
    }

    [HttpGet("validate-token")]
    public IActionResult ValidateToken(string token)
    {
        var result = _tokenValidationService.ValidateToken(token);

        if (result.IsValid)
        {
            return Ok(new
            {
                message = "Token is valid.",
                username = result.Username,
                timeRemaining = $"{result.TimeRemaining.TotalMinutes} minutes"
            });
        }

        return BadRequest(new { message = "Token is invalid.", error = result.ValidationError });
    }
}
