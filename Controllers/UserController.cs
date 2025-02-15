namespace clickdown.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : Controller
{
    private AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IResult Get()
    {
        var usersDb = _context.Users.ToList();

        var usersDto = new List<UserDto>();

        foreach (var user in usersDb)
        {
            usersDto.Add(new UserDto
            {
                Id = user.Id,
                Username = user.Username
            });
        }

        return Results.Ok(usersDto);
    }

    [HttpGet("{id}")]
    public IResult GetById(
        [FromRoute] int id)
    {
        var userDb = _context.Users.Find(id);

        if (userDb is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(new UserDto
        {
            Id = userDb.Id,
            Username = userDb.Username
        });
    }

    [HttpPost]
    public IResult Post(
        [FromBody] UserViewModel user)
    {
        string hashedPassword = HashingService.GenerateHashedPassword(user.Password);

        var newUser = new User
        {
            Username = user.Username,
            Password = hashedPassword
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();

        return Results.Created($"/api/user/{newUser.Id}", newUser);
    }

    [HttpPut("{id}")]
    public IResult Put(
        [FromRoute] int id,
        [FromBody] UserViewModel user)
    {
        var userDb = _context.Users.Find(id);

        if (userDb is null)
        {
            return Results.NotFound();
        }
        
        string hashedPassword = HashingService.GenerateHashedPassword(user.Password);

        userDb.Username = user.Username;
        userDb.Password = hashedPassword;
        _context.SaveChanges();

        return Results.Ok(userDb);
    }

    [HttpDelete("{id}")]
    public IResult Delete(
        [FromRoute] int id)
    {
        var userDb = _context.Users.Find(id);

        if (userDb is null)
        {
            return Results.NotFound();
        }

        _context.Users.Remove(userDb);
        _context.SaveChanges();

        return Results.Ok($"Usuario com ID {id} excluido com sucesso!");
    }
}