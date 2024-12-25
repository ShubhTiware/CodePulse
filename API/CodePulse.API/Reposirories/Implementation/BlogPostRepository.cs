using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Reposirories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Reposirories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext DbContext;
        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }


        public async Task<BlogPost> CreateAsync(BlogPost post)
        {
            await DbContext.BlogPosts.AddAsync(post);
            await DbContext.SaveChangesAsync();
            return post;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await DbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
          return await DbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogpost = await DbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlogpost == null) 
            {
                return null;
            }

            //Update BlogPost
            DbContext.Entry(existingBlogpost).CurrentValues.SetValues(blogPost);

            //Update Categories
            existingBlogpost.Categories = blogPost.Categories;

            await DbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogpost = await DbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

            if (existingBlogpost != null) 
            {
                DbContext.BlogPosts.Remove(existingBlogpost);
                await DbContext.SaveChangesAsync();
                return existingBlogpost;
            }

            return null;
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await DbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }
    }
}
