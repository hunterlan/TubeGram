using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TubeGram.API.Helpers;
using TubeGram.API.Models;

namespace TubeGram.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _config;

        public ImageController(ApplicationContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateImage([FromForm] CreateImageDto newImageDto)
        {
            //Checking if the user uploaded the image correctly
            if (newImageDto.file.Length == 0)
            {
                return Content("File not selected");
            }

            var path = Path.Combine(_config["FileStorage:Images"]!, newImageDto.file.FileName);
            
            //Saving the image in that folder 
            await using var stream = new FileStream(path, FileMode.Create);
            await newImageDto.file.CopyToAsync(stream);
            stream.Close();

            var newImage = new Image
            {
                UserId = newImageDto.UserId,
                Filename = newImageDto.file.FileName,
                CreationDate = DateTime.Now,
                Description = newImageDto.description
            };

            await _context.Images.AddAsync(newImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateImage), new {Id = newImage.Id});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(long id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image is null)
            {
                return NotFound();
            }
            
            var path = Path.Combine(_config["FileStorage:Images"]!, image.Filename);
            var ext = Path.GetExtension(image.Filename).ToLowerInvariant();

            var b = await System.IO.File.ReadAllBytesAsync(path);   // You can use your own method over here.         
            return File(b, "image/" + ext);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(long id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image is null)
            {
                return NotFound();
            }
            
            var path = Path.Combine(_config["FileStorage:Images"]!, image.Filename);
            System.IO.File.Delete(path);

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }

    public record CreateImageDto(int UserId, IFormFile file, string? description);
}
