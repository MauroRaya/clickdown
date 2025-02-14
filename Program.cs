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

//add hashing
app.MapPost("api/user", (AppDbContext context, UserViewModel user) =>
{
    var newUser = new User
    {
        Username = user.Username,
        Password = user.Password,
    };

    context.Users.Add(newUser);
    context.SaveChanges();

    return Results.Created($"/api/user/{newUser.Id}", user);
});

app.MapPut("/api/user/{id}", (AppDbContext context, int id, UserViewModel user) =>
{
    var userDb = context.Users.Find(id);

    if (userDb is null)
    {
        return Results.NotFound();
    }

    userDb.Username = user.Username;
    userDb.Password = user.Password;
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