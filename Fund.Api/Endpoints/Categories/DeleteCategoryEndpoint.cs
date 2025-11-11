using Fund.Api.Common.Api;
using Fund.Core.Handlers;
using Fund.Core.Models;
using Fund.Core.Requests.Categories;
using Fund.Core.Responses;
using System.Security.Claims;

namespace Fund.Api.Endpoints.Categories
{
    public class DeleteCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
            .WithName("Categories: Delete")
            .WithSummary("Deletes a category.")
            .WithDescription("Deletes a category.")
            .WithOrder(3)
            .Produces<Response<Category?>>();
        private static async Task<IResult> HandleAsync(
            ICategoryHandler handler,
            long id)
        {
            var request = new DeleteCategoryRequest
            {
                UserId = ApiConfiguration.UserId,
                Id = id,
            };
            var result = await handler.DeleteAsync(request);
            return result.IsSuccess
              ? TypedResults.Ok(result)
              : TypedResults.BadRequest(result);
        }

    }
}
