using Fund.Api.Common.Api;
using Fund.Core.Handlers;
using Fund.Core.Models;
using Fund.Core.Requests.Categories;
using Fund.Core.Responses;
using Microsoft.Identity.Client;
namespace Fund.Api.Endpoints.Categories
{
    public class CreateCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Caregories: Create")
            .WithSummary("Creates a new category.")
            .WithDescription("Creates a new category.")
            .WithOrder(1)
            .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(
            ICategoryHandler handler,
            CreateCategoryRequest request)
        {
            var response = await handler.CreateAsync(request);

            return response.IsSuccess
              ? TypedResults.Created($"v1/categories/{response.Data?.Id}")
              : TypedResults.BadRequest(response);
        }

    }
}
