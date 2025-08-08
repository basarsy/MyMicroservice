using ManageService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ManageService.Properties.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManageController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ManageController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPatch]
    [Route("{id}/assign")]
    public async Task<IActionResult> AssignUser(int userId, int taskId, AssignDto assignDto)
    {
        var userResponse = await _httpClient.GetAsync($"api/user/{userId}");
        if (!userResponse.IsSuccessStatusCode)
        {
            return BadRequest($"Failed to get user with id {userId}");
        }
        
        var taskResponse = await _httpClient.GetAsync($"api/task/{taskId}");
        if (!taskResponse.IsSuccessStatusCode)
        {
            return BadRequest($"Failed to get task with id {taskId}");
        }

        var payload = new
        {
            TaskId = taskId,
            UserId = userId
        };
        
        var assignResponse = await _httpClient.PatchAsJsonAsync(
            "http://taskservice/api/task/assign", payload);
        if (!assignResponse.IsSuccessStatusCode)
            return BadRequest("Failed to assign user to task.");

        return Ok("User assigned successfully.");
    }
}