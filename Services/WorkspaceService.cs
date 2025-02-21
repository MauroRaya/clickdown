namespace clickdown.Services;

public class WorkspaceService
{
    private readonly AppDbContext _context;
    
    public WorkspaceService(AppDbContext context)
    {
        _context = context;
    }
    
    public List<Workspace> Get()
    {
        return _context.Workspaces.ToList();
    }
    
    public Workspace? GetById(int id)
    {
        Workspace? workspaceDb = _context.Workspaces.Find(id);
        return workspaceDb is null ? null : workspaceDb;
    }
    
    public Workspace? CreateWorkspace(string? userId, WorkspaceViewModel workspaceVm)
    {
        if (userId.IsNullOrEmpty() || !int.TryParse(userId, out int intUserId))
            return null;
        
        Worker? workerDb = _context.Workers.Find(intUserId);

        if (workerDb is not null)
            return null;

        Workspace workspace = workspaceVm.ToWorkspace();
        _context.Workspaces.Add(workspace);
        _context.SaveChanges();
        
        Worker newWorker = new Worker
        {
            Id = intUserId,
            Role = "Admin",
            WorkspaceId = workspace.Id
        };
        
        _context.Workers.Add(newWorker);
        _context.SaveChanges();

        return workspace;
    }

    public Workspace? DeleteWorkspace(string? userId, int workspaceId)
    {
        if (userId.IsNullOrEmpty() || !int.TryParse(userId, out int intUserId))
            return null;
        
        Workspace? workspace = _context.Workspaces.Find(workspaceId);

        if (workspace is null)
            return null;

        _context.Workspaces.Remove(workspace);
        _context.SaveChanges();

        return workspace;
    }
}