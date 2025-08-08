using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Controllers;


[Microsoft.AspNetCore.Components.Route("api/{controller}")]
[ApiController]

public class UserController : ControllerBase
{
    private readonly UserDbContext _context;

    public UserController(UserDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetUser(){
        var user = _context.Users
            .Select(u => new UserDetailsDto()
            {
                UserId = u.UserId,
                UserName = u.UserName,
            })
            .ToList();
        
        return Ok(user);    
    }
    
    [HttpGet("{id}")]
    public IActionResult GetUser(int id){
        var user = _context.Users
            .Where(u => u.UserId == id)
            .Select(u => new UserDetailsDto()
            {
                UserId = u.UserId,
                UserName = u.UserName,
            })
            .FirstOrDefault();

        if (user == null)
        {
            return NotFound($"User with id {id} not found");
        }
        return Ok(user);   
    }
    
    [HttpPost]
    public IActionResult CreateUser(UserCreateDto createDto)
    {
        var user = new User()
        {
            UserName = createDto.UserName,
        };
        
        var hasher = new PasswordHasher<User>();
        user.UserPassword = hasher.HashPassword(user,createDto.UserPassword);
        
        _context.Users.Add(user);
        _context.SaveChanges();
        
        return CreatedAtAction(nameof(GetUser),
            new
            {
                id = user.UserId,
                name = user.UserName
            }, user);   
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound($"User with id {id} not found");
        }
        
        _context.Users.Remove(user);
        _context.SaveChanges();

        return Ok($"User with id {id} has been deleted");
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, UpdateUserDto updateDto)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound($"User with id {id} not found");
        }

        user.UserName = updateDto.UserName;
        user.UserPassword = updateDto.UserPassword;
        
        _context.SaveChanges();
        return Ok($"User with id {id} has been updated");   
    }
    
    [HttpGet]
    [Route("auth/{username}")]
    public async Task<IActionResult> GetUserByUserName(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => 
            u.UserName == username);
        if (user == null)
        {
            return NotFound($"User with username {username} not found");
        }

        return Ok(new UserDetailsDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash
            }
        );
    }
}