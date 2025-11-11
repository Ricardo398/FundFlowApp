using Fund.Api.Common.Api;
using Fund.Core.Handlers;
using Fund.Core.Models;
using Fund.Core.Requests.Transactions;
using Fund.Core.Responses;

namespace Fund.Api.Endpoints.Transactions
{
    public class DeleteTransactionEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapDelete("/{id}", HandleAsync)
            .WithName("Transactions: Delete")
            .WithSummary("Delete a transaction")
            .WithDescription("Delete a transaction")
            .WithOrder(3)
            .Produces<Response<Transaction?>>();
        private static async Task<IResult> HandleAsync(
            ITransactionHandler handler,
            long id)
        {
            var request = new DeleteTransactionRequest
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
