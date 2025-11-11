using Fund.Api.Common.Api;
using Fund.Core.Handlers;
using Fund.Core.Models;
using Fund.Core.Requests.Categories;
using Fund.Core.Responses;

namespace Fund.Api.Endpoints.Categories
{
    public class GetCategoryByIdEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Categories: Get By Id")
            .WithSummary("Get a category by id.")
            .WithDescription("Get a category by id.")
            .WithOrder(4)
            .Produces<Response<Category?>>();
        private static async Task<IResult> HandleAsync(
            ICategoryHandler handler,
            long id)
        {
            var request = new GetCategoryByIdRequest
            {
                UserId = ApiConfiguration.UserId,
                Id = id,
            };
            var result = await handler.GetByIdAsync(request);
            return result.IsSuccess
              ? TypedResults.Ok(result)
              : TypedResults.BadRequest(result);
        }
    }
}
