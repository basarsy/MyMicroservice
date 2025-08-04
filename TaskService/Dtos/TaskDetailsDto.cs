namespace TaskService.Dtos;

public class TaskDetailsDto
{
    public int TaskId { get; set; }
    public string TaskTitle { get; set; }
    public string TaskDescription { get; set; }
    public bool TaskStatus { get; set; }
    public DateTime TaskDate { get; set; }
}