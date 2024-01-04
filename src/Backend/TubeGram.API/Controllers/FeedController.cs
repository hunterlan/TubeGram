using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TubeGram.API.Helpers;
using TubeGram.API.Models;

namespace TubeGram.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FeedController(ApplicationContext context) : ControllerBase
    {
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var photos = context.Images.Take(50).ToList();
            var videos = context.Videos.Take(50).ToList();

            var posts = new List<Feed>();
            foreach (var photo in photos)
            {
                var userData = await context.Users.FindAsync(photo.UserId);
                posts.Add(new Feed {Timestamp = new DateTimeOffset(photo.CreationDate).ToUnixTimeSeconds(), 
                    Id = photo.Id, Description = photo.Description ?? "", Type = "Photo", Username = userData!.Username});
            }

            foreach (var video in videos)
            {
                var userData = await context.Users.FindAsync(video.UserId);
                posts.Add(new Feed {Timestamp = new DateTimeOffset(video.CreationDate).ToUnixTimeSeconds(), 
                    Id = video.Id, Description = video.Description ?? "", Type = "Photo", Username = userData!.Username});
            }

            var feed = posts.OrderBy(p => p.Timestamp).TakeLast(20);
            return Ok(feed);
        }
    }
}
