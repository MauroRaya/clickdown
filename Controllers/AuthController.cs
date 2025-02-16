namespace clickdown.Controllers;

[Route("api/")]
[ApiController]
public class AuthController : ControllerBase
{
    private AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }
}