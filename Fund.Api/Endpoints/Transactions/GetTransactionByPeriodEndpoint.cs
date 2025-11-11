using Fund.Api.Common.Api;
using Fund.Core;
using Fund.Core.Handlers;
using Fund.Core.Models;
using Fund.Core.Requests.Transactions;
using Fund.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fund.Api.Endpoints.Transactions
{
    public class GetTransactionByPeriodEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/", HandleAsync)
            .WithName("Transactions: Get By Period")
            .WithSummary("Get transactions by period")
            .WithDescription("Get transactions by period")
            .WithOrder(5)
            .Produces<Response<List<Transaction?>>>();
        private static async Task<IResult> HandleAsync(
            ITransactionHandler handler,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
            [FromQuery] int pageSize = Configuration.DefaultPageSize)
        {
            var request = new GetTransactionsByPeriodRequest
            {
                UserId = ApiConfiguration.UserId,
                PageNumber = pageNumber,
                PageSize = pageSize,
                StartDate = startDate,
                EndDate = endDate
            };
            var result = await handler.GetByPeriodAsync(request);
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
