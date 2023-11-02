namespace Domain.DTOs;

public class UserCreationDto
{
    public string Username { get; set; }
    public string Password { get; set; }

    public UserCreationDto(string username, string password)
    {
        Username = username;
        Password = password;
    }
}