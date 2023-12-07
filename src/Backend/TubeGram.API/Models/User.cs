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
    
    public ICollection<Image> Images { get; set; }
    public ICollection<Video> Videos { get; set; }
    public PictureProfile PictureProfile { get; set; }
}