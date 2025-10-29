using Fund.Core.Models;
using Fund.Core.Requests.Categories;
using Fund.Core.Responses;

namespace Fund.Core.Handlers
{
    public interface ICategoryHandler
    {
        Task<Response<Transaction?>> CreateAsync(CreateTransacrionRequest request);
        Task<Response<Transaction?>> UpdateAsync(UpdateCategoryRequest request);
        Task<Response<Transaction?>> DeleteAsync(DeleteCategoryRequest request);
        Task<Response<Transaction?>> GetByIdAsync(GetCategoryByIdRequest request);
        Task<PagedResponse<List<Transaction>?>> GetAllAsync(GetAllCategoriesRequest request);
    }
}
