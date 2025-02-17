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
        string? token = context
            .Request
            .Headers["Authorizarion"]
            .FirstOrDefault()
            ?.Split(" ")
            .Last();

        if (!token.IsNullOrEmpty())
        {
            ClaimsPrincipal principal = TokenUtil.ValidateToken(token);

            if (principal is not null)
            {
                context.User = principal;
            }
        }

        await _next(context);
    }
}