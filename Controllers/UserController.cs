namespace clickdown.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public IResult Get()
    {
        List<UserDto> users = _userService.Get();
        return Results.Ok(users);
    }

    [HttpGet("{id}")]
    public IResult GetById(
        [FromRoute] int id)
    {
        UserDto? user = _userService.GetById(id);
        return user is null ? Results.NotFound() : Results.Ok(user);
    }

    [HttpPost("register")]
    public IResult Register(
        [FromBody] UserViewModel user)
    {
        User newUser = _userService.Register(user);
        return Results.Created($"api/user/{newUser.Id}", newUser);
    }

    [HttpPost("login")]
    public IResult Login(
        [FromBody] UserViewModel userVm)
    {
        string? token = _userService.Login(userVm);
        return token is null ? Results.Unauthorized() : Results.Ok(token);
    }
    
    [Authorize]
    [HttpPut("{targetId}")]
    public IResult Update(
        [FromRoute] int targetId,
        [FromBody] UserViewModel userVm)
    {
        string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        User? user = _userService.Update(userId, targetId, userVm);
        return user is null ? Results.NotFound() : Results.Ok(user);
    }

    [Authorize]
    [HttpDelete("{targetId}")]
    public IResult Delete(
        [FromRoute] int targetId)
    {
        string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        User? user = _userService.Remove(userId, targetId);
        return user is null ? Results.NotFound() : Results.Ok(user);
    }

    [Authorize]
    [HttpPost("workspace/create")]
    public IResult CreateWorkspace(
        [FromBody] WorkspaceViewModel workspaceVm)
    {
        string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Workspace? workspace = _userService.CreateWorkspace(userId, workspaceVm);
        return workspace is null ? Results.BadRequest() : Results.Ok(workspace);
    }
    
    [Authorize]
    [HttpDelete("workspace/delete/{workspaceId}")]
    public IResult DeleteWorkspace(
        [FromRoute] int workspaceId)
    {
        string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Workspace? workspace = _userService.DeleteWorkspace(userId, workspaceId);
        return workspace is null ? Results.BadRequest() : Results.Ok(workspace);
    }
}