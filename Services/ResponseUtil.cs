namespace clickdown.Services;

public static class ResponseUtil
{
    public static async Task ReturnErrorResponse(HttpContext context, string errorMessage)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "Application/json";

        var response = Result<string>.NewError(errorMessage);

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}