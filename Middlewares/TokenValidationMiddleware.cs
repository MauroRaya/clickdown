namespace clickdown.Middlewares;

public class TokenValidationMiddleware
{
    private RequestDelegate _next;

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

        if (publicPaths.Any(p => context.Request.Path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase)))
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

        if (token.IsNullOrEmpty())
        {
            await ReturnErrorResponse(context, "Token is required");
            return;
        }
        
        ClaimsPrincipal? principal = TokenUtil.ValidateToken(token);

        if (principal is null)
        {
            await ReturnErrorResponse(context, "Token is invalid");
            return;
        }

        context.User = principal;
        await _next(context);
    }

    public async Task ReturnErrorResponse(HttpContext context, string errorMessage)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "Application/json";

        var response = Result<string>.NewError(errorMessage);

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}