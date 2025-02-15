var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=Data/app.db"));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.MapGet("api/ping", () => "pong");

app.Run();
