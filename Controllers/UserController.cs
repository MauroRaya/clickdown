namespace clickdown.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : Controller
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
        if (user is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(user);
    }

    [HttpPost]
    public IResult Post(
        [FromBody] UserViewModel user)
    {
        User newUser = _userService.Add(user);
        return Results.Created($"/api/user/{newUser.Id}", newUser);
    }

    [HttpPut("{id}")]
    public IResult Put(
        [FromRoute] int id,
        [FromBody] UserViewModel user)
    {
        _userService.Update(id, user);
        return Results.NoContent();
    }

    [HttpDelete("{id}")]
    public IResult Delete(
        [FromRoute] int id)
    {
        _userService.Remove(id);
        return Results.NoContent();
    }
}