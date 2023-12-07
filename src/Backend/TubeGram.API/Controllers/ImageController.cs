using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TubeGram.API.Helpers;
using TubeGram.API.Models;

namespace TubeGram.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController(ApplicationContext context, IConfiguration config) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostImage([FromForm] CreateImageDto newImageDto)
        {
            //TODO: Write allowed file extensions
            //TODO: Write anti-virus service
            //Checking if the user uploaded the image correctly
            if (newImageDto.File.Length == 0)
            {
                return Content("File not selected");
            }

            var path = Path.Combine(config["FileStorage:Images"]!, newImageDto.File.FileName);
            
            //Saving the image in that folder 
            await using var stream = new FileStream(path, FileMode.Create);
            await newImageDto.File.CopyToAsync(stream);
            stream.Close();

            var newImage = new Image
            {
                UserId = newImageDto.UserId,
                Filename = newImageDto.File.FileName,
                CreationDate = DateTime.Now,
                Description = newImageDto.Description
            };

            await context.Images.AddAsync(newImage);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostImage), new {Id = newImage.Id});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(long id)
        {
            var image = await context.Images.FindAsync(id);

            if (image is null)
            {
                return NotFound();
            }
            
            var path = Path.Combine(config["FileStorage:Images"]!, image.Filename);
            var ext = Path.GetExtension(image.Filename).ToLowerInvariant();

            var b = await System.IO.File.ReadAllBytesAsync(path);   // You can use your own method over here.         
            return File(b, "image/" + ext);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(long id)
        {
            var image = await context.Images.FindAsync(id);

            if (image is null)
            {
                return NotFound();
            }
            
            var path = Path.Combine(config["FileStorage:Images"]!, image.Filename);
            System.IO.File.Delete(path);

            context.Images.Remove(image);
            await context.SaveChangesAsync();

            return Ok();
        }
    }

    public record CreateImageDto(int UserId, IFormFile File, string? Description);
}
