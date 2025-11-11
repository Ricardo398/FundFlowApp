using Fund.Api.Common.Api;
using Fund.Core.Handlers;
using Fund.Core.Models;
using Fund.Core.Requests.Transactions;
using Fund.Core.Responses;

namespace Fund.Api.Endpoints.Transactions
{
    public class UpdateTransactionEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPut("/{id}", HandleAsync)
            .WithName("Transactions: Update")
            .WithSummary("Update a transaction")
            .WithDescription("Update a transaction")
            .WithOrder(2)
            .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(
            ITransactionHandler handler,
            UpdateTransactionRequest request,
            long id)
        {
            request.UserId = ApiConfiguration.UserId;
            request.Id = id;

            var result = await handler.UpdateAsync(request);
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
