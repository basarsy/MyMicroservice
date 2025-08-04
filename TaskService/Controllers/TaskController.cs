using Microsoft.AspNetCore.Mvc;
using TaskService.Data;
using TaskService.Dtos;
using TaskService.Models;

namespace TaskService.Controllers;

[Route("api/{controller}")]
[ApiController]

public class TaskController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public TaskController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetTasks()
    {
        var tasks = _context.Tasks
            .Select(t => new TaskDetailsDto()
            {
                TaskId = t.TaskId,
                TaskTitle = t.TaskTitle,
                TaskDescription = t.TaskDescription,
                TaskStatus = t.TaskStatus,
                TaskDate = t.TaskDate
            })
            .ToList();
        return Ok(tasks);   
    }
    
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetTask(int id)
    {
        var task = _context.Tasks
            .Where(t=> t.TaskId == id)
            .Select(t => new TaskDetailsDto()
            {
                TaskId = t.TaskId,
                TaskTitle = t.TaskTitle,
                TaskDescription = t.TaskDescription,
                TaskStatus = t.TaskStatus,
                TaskDate = t.TaskDate
            })
            .FirstOrDefault();
        if (task == null)
        {
            return NotFound($"Task with id {id} not found");
        }
        return Ok(task);   
    }

    [HttpPost]
    public IActionResult CreateTask(CreateTaskDto createDto)
    {
        var task = new TaskItem()
        {
            TaskTitle = createDto.TaskTitle,
            TaskDescription = createDto.TaskDescription,
            TaskStatus = false,
            TaskDate = DateTime.UtcNow
        };
        
        _context.Tasks.Add(task);
        _context.SaveChanges();
        
        return CreatedAtAction(nameof(GetTask),
            new
            {
                id = task.TaskId,
                name = task.TaskTitle
            }, task);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult UpdateTask(int id, UpdateTaskDto updateDto)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound($"Task with id {id} not found");
        }

        task.TaskTitle = updateDto.TaskTitle;
        task.TaskDescription = updateDto.TaskDescription;
        task.TaskStatus = updateDto.TaskStatus;

        _context.SaveChanges();
        return Ok($"Task with id {id} has been updated");
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteTask(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound($"Task with id {id} not found");
        }
        
        _context.Tasks.Remove(task);
        _context.SaveChanges();
        return Ok($"Task with id {id} has been deleted");
    }
    
    [HttpPatch]
    [Route("toggle/{id}")]
    public IActionResult ToggleTaskStatus(int id, bool taskStatus)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound($"Task with id {id} not found");
        }
        task.TaskStatus = !task.TaskStatus;
        _context.SaveChanges();
        return Ok($"Task with id {id} has been updated");   
    }

    [HttpPut]
    [Route("prio/{id}")]
    public IActionResult UpdateTaskPriority(int id, TaskPriorityDto priorityDto)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound($"Task with id {id} not found");
        }

        if (task.TaskPriority == priorityDto.TaskPriority)
        {
            return BadRequest($"Task with id {id} already has priority {priorityDto.TaskPriority}");       
        }
        task.TaskPriority = priorityDto.TaskPriority;
        _context.SaveChanges();
        return Ok($"Task with id {id} has been priority assigned as {priorityDto.TaskPriority}.");  
    }

    [HttpGet]
    [Route("priofilter")]
    public IActionResult FilterTasksByPriority(TaskPriorityDto priorityDto)
    {
        var task = _context.Tasks
            .Where(p => p.TaskPriority == priorityDto.TaskPriority)
            .Select(pt => new TaskFilterDto()
            {
                TaskTitle = pt.TaskTitle,
                TaskDescription = pt.TaskDescription
            })
            .ToList();
        if (task == null)
        {
            return BadRequest($"There are no tasks with priority {priorityDto.TaskPriority}");
        }
        return Ok(task);   
    }
    
    [HttpGet]
    [Route("statusfilter")]
    public IActionResult FilterTasksByStatus(TaskStatusDto statusDto)
    {
        var task = _context.Tasks
            .Where(s => s.TaskStatus == statusDto.TaskStatus)
            .Select(st => new TaskFilterDto()
            {
                TaskTitle = st.TaskTitle,
                TaskDescription = st.TaskDescription
            })
            .ToList();
        if (task == null)
        {
            return BadRequest($"There are no tasks with status {statusDto.TaskStatus}");
        }
        return Ok(task);
    }
}