namespace clickdown.Middlewares;

public class DevEnvironmentMiddleware
{
    private readonly RequestDelegate _next;

    public DevEnvironmentMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var privatePaths = new[]
        {
            "/api/user"
        };

        if (!context.Request.Method.Equals(HttpMethods.Get))
        {
            await ResponseUtil.ReturnErrorResponse(context, "Request Method is not Get");
            return;
        }
        
        string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        Console.WriteLine($"Environment: {env}");

        if (!env.Equals("Development"))
        {
            await ResponseUtil.ReturnErrorResponse(context, "Not in Development environment");
            return;
        }

        if (privatePaths.Any(p => p.Equals(context.Request.Path.Value)))
        {
            await _next(context);
        }
    }
}