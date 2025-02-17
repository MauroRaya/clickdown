namespace clickdown.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<Job> Jobs { get; set; }
}