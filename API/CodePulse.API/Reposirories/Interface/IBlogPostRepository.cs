using CodePulse.API.Models.Domain;

namespace CodePulse.API.Reposirories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost post);

        Task<IEnumerable<BlogPost>> GetAllAsync();

        Task<BlogPost?> GetByIdAsync(Guid id);

        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);

        Task<BlogPost?> UpdateAsync(BlogPost blogPost);

        Task<BlogPost?> DeleteAsync(Guid id);
    }
}
