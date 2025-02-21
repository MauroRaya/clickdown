namespace clickdown.Controllers;

[Route("api/workspace")]
[ApiController]
public class WorkspaceController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly WorkspaceService _workspaceService;
    
    public WorkspaceController(
        IHttpContextAccessor httpContextAccessor,
        WorkspaceService workspaceService)
    {
        _workspaceService = workspaceService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HttpGet]
    public IResult Get()
    {
        List<Workspace> workspaces = _workspaceService.Get();
        return Results.Ok(workspaces);
    }

    [HttpGet("{id}")]
    public IResult GetById(
        [FromRoute] int id)
    {
        Workspace? workspace = _workspaceService.GetById(id);
        return workspace is null ? Results.NotFound() : Results.Ok(workspace);
    }
    
    [Authorize]
    [HttpPost("create")]
    public IResult CreateWorkspace(
        [FromBody] WorkspaceViewModel workspaceVm)
    {
        string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Workspace? workspace = _workspaceService.CreateWorkspace(userId, workspaceVm);
        return workspace is null ? Results.BadRequest() : Results.Ok(workspace);
    }
    
    [Authorize]
    [HttpDelete("delete/{workspaceId}")]
    public IResult DeleteWorkspace(
        [FromRoute] int workspaceId)
    {
        string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Workspace? workspace = _workspaceService.DeleteWorkspace(userId, workspaceId);
        return workspace is null ? Results.BadRequest() : Results.Ok(workspace);
    }
}