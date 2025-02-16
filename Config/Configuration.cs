namespace clickdown.Services.Config;

public class Configuration
{
    public static readonly string? PrivateKey = Environment.GetEnvironmentVariable("PrivateKey");
}