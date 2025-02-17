namespace clickdown.Models;

public class Workspace
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class WorkspaceViewModel
{
    public string Name { get; set; } = string.Empty;

    public Workspace ToWorkspace()
    {
        return new Workspace
        {
            Name = this.Name,
        };
    }
}