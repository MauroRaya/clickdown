namespace clickdown.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : Controller
{
    [HttpGet]
    public IResult Get(
        [FromServices] AppDbContext contexto)
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
    public IResult GetById(
        [FromServices] AppDbContext contexto,
        [FromRoute] int id)
    {
        var userDb = contexto.Users.Find(id);

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
        [FromServices] AppDbContext contexto,
        [FromBody] UserViewModel user)
    {
        var inputBytes = Encoding.UTF8.GetBytes(user.Password);
        var inputHash = SHA256.HashData(inputBytes);
        var hashedPassword = Convert.ToHexString(inputHash);

        var newUser = new User
        {
            Username = user.Username,
            Password = hashedPassword
        };

        contexto.Users.Add(newUser);
        contexto.SaveChanges();

        return Results.Created($"/api/user/{newUser.Id}", newUser);
    }

    [HttpPut("{id}")]
    public IResult Put(
        [FromServices] AppDbContext contexto,
        [FromRoute] int id,
        [FromBody] UserViewModel user)
    {
        var userDb = contexto.Users.Find(id);

        if (userDb is null)
        {
            return Results.NotFound();
        }

        var inputBytes = Encoding.UTF8.GetBytes(user.Password);
        var inputHash = SHA256.HashData(inputBytes);
        var hashedPassword = Convert.ToHexString(inputHash);

        userDb.Username = user.Username;
        userDb.Password = hashedPassword;
        contexto.SaveChanges();

        return Results.Ok(userDb);
    }

    [HttpDelete("{id}")]
    public IResult Delete(
        [FromServices] AppDbContext contexto,
        [FromRoute] int id)
    {
        var userDb = contexto.Users.Find(id);

        if (userDb is null)
        {
            return Results.NotFound();
        }

        contexto.Users.Remove(userDb);
        contexto.SaveChanges();

        return Results.Ok($"Usuario com ID {id} excluido com sucesso!");
    }
}