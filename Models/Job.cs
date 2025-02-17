namespace clickdown.Models;

public class Job
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public float Money { get; set; }
    public int WorkerId { get; set; }
}