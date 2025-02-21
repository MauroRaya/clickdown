namespace clickdown.Middlewares;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var publicPaths = new[] 
        {
            "/api/ping",
            "/api/user/register",
            "/api/user/login",
        };

        if (publicPaths.Any(p =>
                context.Request.Path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase))
            ||  context.Request.Path.Value.Equals("/"))
        {
            await _next(context);
            return;
        }

        string? token = context
            .Request
            .Headers["Authorization"]
            .FirstOrDefault()?
            .Split(' ')
            .Last();
        
        Console.WriteLine($"Token: {token}");

        if (token.IsNullOrEmpty())
        {
            await ResponseUtil.ReturnErrorResponse(context, "Token is required");
            return;
        }
        
        ClaimsPrincipal? principal = TokenUtil.ValidateToken(token);

        if (principal is null)
        {
            await ResponseUtil.ReturnErrorResponse(context, "Token is invalid");
            return;
        }

        context.User = principal;
        await _next(context);
    }
}