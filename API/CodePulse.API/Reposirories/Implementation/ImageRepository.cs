using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Reposirories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Reposirories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage image)
        {
            //1. Upload the image to the API/Images folder
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            //2. Update the database
            //https://codepulse.com/images/someImage.jpg
            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.Url = urlPath;

            await dbContext.BlogImages.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
            return await dbContext.BlogImages.ToListAsync();
        }

    }
}
