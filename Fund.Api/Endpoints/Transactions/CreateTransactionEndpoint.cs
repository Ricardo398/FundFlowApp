

using Fund.Api.Common.Api;
using Fund.Core.Handlers;
using Fund.Core.Models;
using Fund.Core.Requests.Transactions;
using Fund.Core.Responses;

namespace Fund.Api.Endpoints.Transactions
{
    public class CreateTransactionEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/", HandleAsync)
            .WithName("Transactions: Create")
            .WithSummary("Create a new transaction")
            .WithDescription("Create a new transation")
            .WithOrder(1)
            .Produces<Response<Transaction?>>();
        private static async Task<IResult> HandleAsync(
            ITransactionHandler handler,
            CreateTransactionRequest request)
        {
            request.UserId = ApiConfiguration.UserId;

            var result = await handler.CreateAsync(request);
            return result.IsSuccess
                ? TypedResults.Created($"v1/transactions/{result.Data?.Id}", result)
                : TypedResults.BadRequest(result);
        }
    }
}
