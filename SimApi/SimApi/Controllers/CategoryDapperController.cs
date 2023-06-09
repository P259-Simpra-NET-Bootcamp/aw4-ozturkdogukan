using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimApi.Base;
using SimApi.Data;
using SimApi.Operation;
using SimApi.Operation.Dapper.Category;
using SimApi.Schema;

namespace SimApi.Service.Controllers
{

    [ResponseGuid]
    [Route("simapi/v1/[controller]")]
    [ApiController]
    public class CategoryDapperController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private IMapper mapper;

        public CategoryDapperController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        [HttpGet]
        public ApiResponse<List<CategoryResponse>> GetAll()
        {
            var categoryList = categoryService.GetAll();
            return categoryList;
        }

        [HttpGet("{id}")]
        public ApiResponse<CategoryResponse> GetById(int id)
        {
            var category = categoryService.GetById(id);
            return category;
        }

        [HttpPost("Insert")]
        public void Insert([FromBody] CategoryRequest cat)
        {
            //var entity = mapper.Map<Category>(cat);
            categoryService.Insert(cat);
        }

        [HttpPut("Update")]
        public void Update([FromBody] CategoryRequest cat)
        {
            categoryService.Update(cat);
        }

        [HttpDelete("Delete")]
        public void Delete(int id)
        {
            categoryService.Delete(id);
        }
    }
}
