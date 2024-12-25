using CodePulse.API.Models.Domain;
using System.Net;

namespace CodePulse.API.Reposirories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage image);
        Task<IEnumerable<BlogImage>> GetAll();
    }
}
