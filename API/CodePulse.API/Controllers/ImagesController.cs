using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Reposirories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        //POST: {apibase}/api/images
        [HttpPost]
        public async Task<IActionResult> UploadImge([FromForm] IFormFile file, [FromForm] string filename, [FromForm] string title)
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid) 
            {
                var blogImage = new BlogImage
                {
                    FileExtension =Path.GetExtension(file.FileName).ToLower(),
                    FileName = filename,
                    Title = title,
                    DateCreated = DateTime.Now
                };

                blogImage = await imageRepository.Upload(file, blogImage);

                //convert Domain to DTO
                var response = new BlogIageDto
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    Url = blogImage.Url,
                    FileName = blogImage.FileName
                };

                return Ok(response);
            }

            return BadRequest(ModelState);

        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowExentions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowExentions.Contains(Path.GetExtension(file.FileName).ToLower())) 
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            if (file.Length > 10485760) 
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }

        //GET: {apibase}/api/images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            //call image repository to get all images
            var images = await imageRepository.GetAll();

            //covert Domain to DTO
            var response = new List<BlogIageDto>();
            foreach (var image in images)
            {
                response.Add(new BlogIageDto
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    Url = image.Url,
                    FileName = image.FileName
                });
            }
            return Ok(response);
        }
    }
}
