using AutoMapper;
using SimApi.Base;
using SimApi.Data;
using SimApi.Data.Uow;
using SimApi.Schema;
using System.Security.Cryptography.X509Certificates;

namespace SimApi.Operation.Dapper.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public void Delete(int id)
        {
            unitOfWork.DapperRepository<SimApi.Data.Category>().DeleteById(id);
            unitOfWork.Complete();
        }

        public ApiResponse<List<CategoryResponse>> GetAll()
        {
            var categoryList = unitOfWork.DapperRepository<SimApi.Data.Category>().GetAll().ToList();
            var mapped = mapper.Map<List<SimApi.Data.Category>, List<CategoryResponse>>(categoryList);
            return new ApiResponse<List<CategoryResponse>>(mapped);

        }

        public ApiResponse<CategoryResponse> GetById(int id)
        {
            var category = unitOfWork.DapperRepository<SimApi.Data.Category>().GetById(id);
            var mapped = mapper.Map<SimApi.Data.Category, CategoryResponse>(category);
            return new ApiResponse<CategoryResponse>(mapped);
        }

        public void Insert(CategoryRequest cat)
        {
            var category = mapper.Map<CategoryRequest, SimApi.Data.Category>(cat);
            unitOfWork.DapperRepository<SimApi.Data.Category>().Insert(category);
            unitOfWork.Complete();
        }

        public void Update(CategoryRequest cat)
        {
            var category = unitOfWork.DapperRepository<SimApi.Data.Category>().GetById(cat.Id);
            var categoryObject = mapper.Map<CategoryRequest, SimApi.Data.Category>(cat, category);
            unitOfWork.DapperRepository<SimApi.Data.Category>().Update(categoryObject);
            unitOfWork.Complete();
        }
    }
}


