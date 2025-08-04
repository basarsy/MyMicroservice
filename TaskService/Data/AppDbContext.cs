using Microsoft.EntityFrameworkCore;
using TaskService.Models;

namespace TaskService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<TaskItem> Tasks { get; set; }
}