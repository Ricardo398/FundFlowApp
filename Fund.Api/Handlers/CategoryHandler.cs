using Fund.Api.Data;
using Fund.Core.Handlers;
using Fund.Core.Models;
using Fund.Core.Requests.Categories;
using Fund.Core.Responses;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography.Xml;

namespace Fund.Api.Handlers
{
    public class CategoryHandler(AppDbContext context) : ICategoryHandler
    {
        public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
        {
            await Task.Delay(5000);
            var category = new Category
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description
            };

            await context.Categories.AddAsync(category);

            try
            {
                await context.SaveChangesAsync();

                return new Response<Category?>(category, code: 201, message: "Categoria criada com sucesso!");
            }
            catch (Exception)
            {
                return new Response<Category?>(data: null, code: 500, message: "Não foi possível criar uma categoria.");
            }
        }

        public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)
            {
                return new Response<Category?>(data: null, code: 404, message: "Categoria não encontrada.");
            }

            context.Categories.Remove(category);

            try
            {
                await context.SaveChangesAsync();
                return new Response<Category?>(data: category, message: "Categoria excluída com sucesso!");

            }
            catch (Exception)
            {
                return new Response<Category?>(data: null, code: 500, message: "Não foi possível excluir a categoria.");
            }

        }

        public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
        {
            var query = context.Categories.AsNoTracking().Where(x => x.UserId == request.UserId).OrderBy(x => x.Title);

            var categories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            try
            {
                var count = await query.CountAsync();

                return new PagedResponse<List<Category>?>(
                    categories,
                    count,
                    request.PageNumber,
                    request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<Category>?>(data: null, code: 500, message: "Não foi possível recuperar todas as categorias.");
            }

        }

        public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
        {
            try
            {
                var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                return category is null
                ? new Response<Category?>(data: null, code: 404, message: "Categoria não encontrada.")
                : new Response<Category?>(category);
            }
            catch (Exception)
            {
                return new Response<Category?>(data: null, code: 500, message: "Não foi possível recuperar a categoria.");
            }

        }

        public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)
            {
                return new Response<Category?>(data: null, code: 404, message: "Categoria não encontrada.");
            }

            category.Title = request.Title;
            category.Description = request.Description;
            context.Categories.Update(category);

            try
            {
                await context.SaveChangesAsync();

                return new Response<Category?>(category, message: "Categoria atualizada com sucesso!");
            }
            catch (Exception)
            {
                return new Response<Category?>(data: null, code: 500, message: "Não foi possível atualizar a categoria.");
            }
        }
    }
}
