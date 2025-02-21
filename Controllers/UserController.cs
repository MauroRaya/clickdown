namespace clickdown.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var result = await _userService.GetAsync();
        
        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id)
    {
        var result = await _userService.GetByIdAsync(id);
        
        return result.Success
            ? Ok(result)
            : NotFound(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] UserViewModel user)
    {
        var result = await _userService.RegisterAsync(user);
        
        return result.Success
            ? Created($"api/user/{result.Data?.Id}", result.Data)
            : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(
        [FromBody] UserViewModel userVm)
    {
        var result = await _userService.LoginAsync(userVm);
        
        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }

    [Authorize]
    [HttpPut("{targetId}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] int targetId,
        [FromBody] UserViewModel userVm)
    {
        string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _userService.UpdateAsync(userId, targetId, userVm);

        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }

    [Authorize]
    [HttpDelete("{targetId}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int targetId)
    {
        string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _userService.RemoveAsync(userId, targetId);

        return result.Success
            ? Ok(result)
            : NotFound(result);
    }
}