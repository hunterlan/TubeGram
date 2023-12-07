namespace TubeGram.API.Models;

public class FileItem
{
    public long Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }

    public string Filename { get; set; } = null!;
    
    public DateTime CreationDate { get; set; }
}