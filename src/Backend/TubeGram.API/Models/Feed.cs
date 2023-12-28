namespace TubeGram.API.Models;

public class Feed
{
    public long Id { get; set; }
    
    public string Type { get; set; }
    
    public string Username { get; set; }
    
    public string Description { get; set; }
    
    public long Timestamp { get; set; }
}