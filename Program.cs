var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=./Data/app.db"));

var app = builder.Build();

app.MapGet("/api/ping", () => "pong");

app.MapGet("/api/user", (AppDbContext context) =>
{
    var usersDb = context.Users.ToList();

    var usersDto = new List<UserDto>();

    foreach (var user in usersDb)
    {
        usersDto.Add(new UserDto {
            Id = user.Id,
            Username = user.Username
        });
    }

    return Results.Ok(usersDto);
});

app.MapGet("/api/user/{id}", (AppDbContext context, int id) =>
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
});

app.MapPost("api/user", (AppDbContext context, UserViewModel user) =>
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
});

app.MapPut("/api/user/{id}", (AppDbContext context, int id, UserViewModel user) =>
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
});

app.MapDelete("/api/user/{id}", (AppDbContext context, int id) =>
{
    var userDb = context.Users.Find(id);

    if (userDb is null)
    {
        return Results.NotFound();
    }

    context.Users.Remove(userDb);
    context.SaveChanges();

    return Results.Ok($"Usuario com ID {id} excluido com sucesso!");
});

app.Run();

//separate into controller
//create hash service
//maybe a service vm -> user and user -> dto?
