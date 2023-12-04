namespace TubeGram.API.Models;

public class User
{
    public User(string username, string password, string email)
    {
        Username = username;
        Password = password;
        Email = email;
    }

    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;
}