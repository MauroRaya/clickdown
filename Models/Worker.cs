namespace clickdown.Models;

public class Worker
{
    public int Id { get; set; }
    public string Role { get; set; } = string.Empty;
    public int WorkspaceId { get; set; }
}