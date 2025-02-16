using clickdown;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TokenService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=Data/app.db"));

builder.Services.AddScoped<UserService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.MapGet("api/ping", () => "pong");

app.Run();
