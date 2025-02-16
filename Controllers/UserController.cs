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
    public IResult Post(
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
    
    [HttpPut("{id}")]
    public IResult Put(
        [FromRoute] int id,
        [FromBody] UserViewModel userVm)
    {
        User? user = _userService.Update(id, userVm);
        return user is null ? Results.NotFound() : Results.Ok(user);
    }

    [HttpDelete("{id}")]
    public IResult Delete(
        [FromRoute] int id)
    {
        User? user = _userService.Remove(id);
        return user is null ? Results.NotFound() : Results.Ok(user);
    }
}