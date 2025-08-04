namespace TaskService.Models;

public enum TaskPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
}
public class TaskItem
{
    public int TaskId { get; set; }
    public string TaskTitle { get; set; }
    public string TaskDescription { get; set; }
    public bool TaskStatus { get; set; }
    public DateTime TaskDate { get; set; }
    public TaskPriority? TaskPriority { get; set; }
    public int UserId { get; set; }
}