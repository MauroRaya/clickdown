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

    [HttpPost]
    public IResult Post(
        [FromBody] UserViewModel user)
    {
        User newUser = _userService.Add(user);
        return Results.Created($"api/user/{newUser.Id}", newUser);
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