using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TubeGram.API.Helpers;
using TubeGram.API.Models;

namespace TubeGram.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController(ApplicationContext context, IConfiguration config) : ControllerBase
    {
        private readonly string[] _permittedExtensions = { ".mp4", ".webm" };

        // GET api/<VideoController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var video = await context.Videos.FindAsync(id);

            if (video is null)
            {
                return NotFound();
            }

            var path = Path.Combine(config["FileStorage:Videos"]!, video.Filename);
            var ext = Path.GetExtension(video.Filename).ToLowerInvariant();
            
            var b = await System.IO.File.ReadAllBytesAsync(path);   // You can use your own method over here.         
            return File(b, "video/" + ext);
        }

        [HttpGet]
        public Task<IActionResult> GetVideos([FromQuery] int count)
        {
            var videos = context.Videos.OrderBy(i => i.CreationDate).Take(count);
            return Task.FromResult<IActionResult>(Ok(videos));
        }

        // POST api/<VideoController>
        [HttpPost]
        [RequestSizeLimit(1024*1024*1024)]
        public async Task<IActionResult> PostVideo([FromForm] PostVideoDto newVideoDto)
        {
            var ext = Path.GetExtension(newVideoDto.Video.FileName);
            if (!_permittedExtensions.Contains(ext))
            {
                return StatusCode(StatusCodes.Status415UnsupportedMediaType, "Only .mp4 and .webm videos supported.");
            }
            //TODO: Write anti-virus service
            if (newVideoDto.Video.Length == 0)
            {
                return Content("File not selected");
            }
            
            var path = Path.Combine(config["FileStorage:Videos"]!, newVideoDto.Video.FileName);
            
            //Saving the image in that folder 
            await using var stream = new FileStream(path, FileMode.Create);
            await newVideoDto.Video.CopyToAsync(stream);
            stream.Close();

            var newVideo = new Video
            {
                UserId = newVideoDto.UserId,
                Filename = newVideoDto.Video.FileName,
                CreationDate = DateTime.Now,
                Description = newVideoDto.Description
            };

            await context.Videos.AddAsync(newVideo);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostVideo), new { Id = newVideo.Id });
        }

        // DELETE api/<VideoController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var video = await context.Videos.FindAsync(id);

            if (video is null)
            {
                return NotFound();
            }

            var path = Path.Combine(config["FileStorage:Videos"]!, video.Filename);
            System.IO.File.Delete(path);

            context.Videos.Remove(video);

            await context.SaveChangesAsync();

            return Ok();
        }
    }

    public record PostVideoDto(int UserId, IFormFile Video, string? Description);
}
