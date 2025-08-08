namespace AuthService.Dtos;

public class UserAuthDto
{
    public string UserName { get; set; }
    public string UserPassword { get; set; }
    public string PasswordHash { get; set; }
}