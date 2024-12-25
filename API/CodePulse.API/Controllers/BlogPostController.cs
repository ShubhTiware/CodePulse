using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Reposirories.Implementation;
using CodePulse.API.Reposirories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            //Covert DTO to damin modal
            var blogPost = new BlogPost
            {
                Title = request.Title,
                ShortDiscription = request.ShortDiscription,
                Containt = request.Containt,
                FeatureImageUrl = request.FeatureImageUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                Auther = request.Auther,
                IsVisible = request.IsVisible,
                Categories = new List<Category>()
            };

            //for many to many connection Categories = new List<Category>()
            foreach (var categoryGuid in request.Categories) 
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);

                if (existingCategory is not null) 
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await blogPostRepository.CreateAsync(blogPost);

            //convert domain back to DTO
            var responce = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDiscription = blogPost.ShortDiscription,
                Containt = blogPost.Containt,
                Auther = blogPost.Auther,
                IsVisible = blogPost.IsVisible,
                FeatureImageUrl = blogPost.FeatureImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                //addition many to many
                Categories = blogPost.Categories.Select(x => new CatagoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(responce);
        }

        //GET: {apibaseurl}/api/blogpost
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogposts = await blogPostRepository.GetAllAsync();
            //return blogpost;

            //convert Domain to DTO
            var response = new List<BlogPostDto>();
            foreach (var blogPost in blogposts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    ShortDiscription = blogPost.ShortDiscription,
                    Containt = blogPost.Containt,
                    Auther = blogPost.Auther,
                    IsVisible = blogPost.IsVisible,
                    FeatureImageUrl = blogPost.FeatureImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Categories = blogPost.Categories.Select(x => new CatagoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                });
            }

            return Ok(response);
        }

        //GET: {apibaseurl}/api/blogpost/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            //Get  blogpost from repository
            var blogPost = await blogPostRepository.GetByIdAsync(id);

            if(blogPost is null)
            {
                return NotFound();
            }

            //convert Domain Modal to DTO
            var response = 
                new BlogPostDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    ShortDiscription = blogPost.ShortDiscription,
                    Containt = blogPost.Containt,
                    Auther = blogPost.Auther,
                    IsVisible = blogPost.IsVisible,
                    FeatureImageUrl = blogPost.FeatureImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Categories = blogPost.Categories.Select(x => new CatagoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                };

            return Ok(response);
        }

        //GET: {apibaseurl}/api/blogpost/{urlHandle}
        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            //Get blogpost details from repository
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

            if (blogPost is null)
            {
                return NotFound();
            }

            //convert Domain Modal to DTO
            var response =
                new BlogPostDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    ShortDiscription = blogPost.ShortDiscription,
                    Containt = blogPost.Containt,
                    Auther = blogPost.Auther,
                    IsVisible = blogPost.IsVisible,
                    FeatureImageUrl = blogPost.FeatureImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Categories = blogPost.Categories.Select(x => new CatagoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                };

            return Ok(response);
        }

        //PUT: {apibaseurl}/api/blogpost/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogpostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            //DTO to Domain Modal
            var blogpost = new BlogPost
            {
                Id = id,
                Title = request.Title,
                ShortDiscription = request.ShortDiscription,
                Containt = request.Containt,
                FeatureImageUrl = request.FeatureImageUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                Auther = request.Auther,
                IsVisible = request.IsVisible,
                Categories = new List<Category>()
            };

            //Foreach
            foreach (var categoryGuid in request.Categories) 
            {
                var existingCatagory = await categoryRepository.GetById(categoryGuid);

                if(existingCatagory != null)
                {
                    blogpost.Categories.Add(existingCatagory);
                }
            }

            //Call Repository to update BlogPost Model
            var updatedBlogpost =  await blogPostRepository.UpdateAsync(blogpost);

            if (updatedBlogpost == null) 
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = blogpost.Id,
                Title = blogpost.Title,
                ShortDiscription = blogpost.ShortDiscription,
                Containt = blogpost.Containt,
                FeatureImageUrl = blogpost.FeatureImageUrl,
                UrlHandle = blogpost.UrlHandle,
                PublishedDate = blogpost.PublishedDate,
                Auther = blogpost.Auther,
                IsVisible = blogpost.IsVisible,
                Categories = blogpost.Categories.Select(x => new CatagoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        //DELETE: {apibaseurl}/api/blogpost/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogpostById([FromRoute] Guid id)
        {
            var deletedBlogpoat = await blogPostRepository.DeleteAsync(id);

            if (deletedBlogpoat == null)
            {
                return NotFound();
            }
            var response = new BlogPostDto
            {
                Id = deletedBlogpoat.Id,
                Title = deletedBlogpoat.Title,
                ShortDiscription = deletedBlogpoat.ShortDiscription,
                Containt = deletedBlogpoat.Containt,
                FeatureImageUrl = deletedBlogpoat.FeatureImageUrl,
                UrlHandle = deletedBlogpoat.UrlHandle,
                PublishedDate = deletedBlogpoat.PublishedDate,
                Auther = deletedBlogpoat.Auther,
                IsVisible = deletedBlogpoat.IsVisible
            };
            return Ok(response);
        }
    }
}
