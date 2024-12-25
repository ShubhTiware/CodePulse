using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Reposirories.Implementation;
using CodePulse.API.Reposirories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatagorysController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CatagorysController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        //post: https://localhost:7016/api/Catagorys
        [HttpPost] 
        public async Task<IActionResult> CreateCatagory(CreateCatagoryRequestDto request)
        {        
            //map DTO to Domain Modal
            var catagory = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            await categoryRepository.CreateAsync(catagory);

            //Domain modal to DTO
            var responce = new CatagoryDto
            {
                Id = catagory.Id,
                Name = catagory.Name,
                UrlHandle = catagory.UrlHandle
            };

            return Ok(responce);
        }

        //Get: https://localhost:7016/api/Catagorys
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();

            //map domain modal ton DTO
            var responce = new List<CatagoryDto>();
            foreach (var category in categories)
            {
                responce.Add(new CatagoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }

            return Ok(responce);
        }

        //Get https://localhost:7016/api/Catagorys/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCatagoryById([FromRoute] Guid id)
        {
            var existingCategory = await categoryRepository.GetById(id);

            if(existingCategory is null)
            {
                return NotFound();
            }

            var response = new CatagoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };

            return Ok(response);
        }

        //put https://localhost:7016/api/Catagorys/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpadateCategoryRequestDto request)
        {
            //Convert DTO to Domain Model
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            category = await categoryRepository.UpdateAsync(category);

            if(category == null)
            {
                return NotFound();
            }
            var response = new CatagoryDto
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            return Ok(response);
        }

        //Delete https://localhost:7016/api/Catagorys/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);
            if(category is null)
            {
                return NotFound();
            }

            //Convert Domain Modal to DTO
            var response = new CatagoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }

    }
}
