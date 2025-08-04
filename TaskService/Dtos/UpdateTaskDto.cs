namespace TaskService.Dtos;

public class UpdateTaskDto
{
    public string TaskTitle { get; set; }
    public string TaskDescription { get; set; }
    public bool TaskStatus { get; set; }
}