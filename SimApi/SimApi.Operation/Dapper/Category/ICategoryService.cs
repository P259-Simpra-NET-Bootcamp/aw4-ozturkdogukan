using SimApi.Base;
using SimApi.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimApi.Operation.Dapper.Category
{
    public interface ICategoryService
    {
        ApiResponse<List<CategoryResponse>> GetAll();
        ApiResponse<CategoryResponse> GetById(int id);
        void Insert(CategoryRequest category);
        void Update(CategoryRequest category);
        void Delete(int id);
    }
}
