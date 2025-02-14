namespace clickdown.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : Controller
{
    [HttpGet]
    public IResult Get(AppDbContext contexto)
    {
        var usersDb = contexto.Users.ToList();

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
    public IResult GetById(AppDbContext context, int id)
    {
        var userDb = context.Users.Find(id);

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
    public IResult Post(AppDbContext context, UserViewModel user)
    {
        var inputBytes = Encoding.UTF8.GetBytes(user.Password);
        var inputHash = SHA256.HashData(inputBytes);
        var hashedPassword = Convert.ToHexString(inputHash);

        var newUser = new User
        {
            Username = user.Username,
            Password = hashedPassword
        };

        context.Users.Add(newUser);
        context.SaveChanges();

        return Results.Created($"/api/user/{newUser.Id}", newUser);
    }

    [HttpPut("{id}")]
    public IResult Put(AppDbContext context, int id, UserViewModel user)
    {
        var userDb = context.Users.Find(id);

        if (userDb is null)
        {
            return Results.NotFound();
        }

        var inputBytes = Encoding.UTF8.GetBytes(user.Password);
        var inputHash = SHA256.HashData(inputBytes);
        var hashedPassword = Convert.ToHexString(inputHash);

        userDb.Username = user.Username;
        userDb.Password = hashedPassword;
        context.SaveChanges();

        return Results.Ok(userDb);
    }

    [HttpDelete("{id}")]
    public IResult Delete(AppDbContext context, int id)
    {
        var userDb = context.Users.Find(id);

        if (userDb is null)
        {
            return Results.NotFound();
        }

        context.Users.Remove(userDb);
        context.SaveChanges();

        return Results.Ok($"Usuario com ID {id} excluido com sucesso!");
    }
}