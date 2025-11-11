using Fund.Api.Data;
using Fund.Core.Handlers;
using Fund.Core.Models;
using Fund.Core.Requests.Transactions;
using Fund.Core.Responses;
using Fund.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Fund.Core.Common;

namespace Fund.Api.Handlers
{
    public class TransactionHandler(AppDbContext context) : ITransactionHandler
    {
        public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
        {
            if (request is { Type: ETransactionType.Withdraw, Amount: > 0 })
                request.Amount *= -1;

            var transaction = new Transaction
            {
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.Now,
                Amount = request.Amount,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Title = request.Title,
                Type = request.Type
            };

            try
            {
                await context.Transactions.AddAsync(transaction);
                await context.SaveChangesAsync();
                return new Response<Transaction?>(transaction, code: 201, message: "Transação criada com sucesso.");
            }
            catch (Exception)
            {
                return new Response<Transaction?>(data: null, code: 500, message: "Não foi possível criar sua transação.");
            }
        }

        public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
        {
            try
            {
                var transaction = await context.Transactions
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (transaction is null)
                {
                    return new Response<Transaction?>(data: null, code: 404, message: "Não foi possível recuperar sua transação.");
                }

                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();
                return new Response<Transaction?>(data: transaction, message: "Sua transação foi deletada com sucesso!");

            }
            catch (Exception)
            {
                return new Response<Transaction?>(data: null, code: 500, message: "Não foi possível deletar sua transação.");
            }
        }

        public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
        {
            try
            {
                var transaction = await context.Transactions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                return transaction is null
                ? new Response<Transaction?>(data: null, code: 404, message: "Não foi possível encontrar sua transação.")
                : new Response<Transaction?>(data: transaction, code: 200, message: "Transação recuperada com sucesso!");

            }
            catch (Exception)
            {
                return new Response<Transaction?>(data: null, code: 500, message: "Não foi possível recuperar sua transação.");
            }
        }

        public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
        {
            request.StartDate ??= DateTime.Now.GetFirstDay();
            request.EndDate ??= DateTime.Now.GetLastDay();

            var query = context.Transactions.AsNoTracking().Where(x =>
            x.PaidOrReceivedAt >= request.StartDate &&
            x.PaidOrReceivedAt <= request.EndDate &&
            x.UserId == request.UserId)
            .OrderBy(x => x.PaidOrReceivedAt);

            var transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            try
            {
                var count = await query.CountAsync();

                return new PagedResponse<List<Transaction>?>(
                    transactions,
                    count,
                    request.PageNumber,
                    request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<Transaction>?>(data: null, code: 500, message: "Não foi possível recuperar todas as transações.");
            }
        }

        public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
        {
            if (request is { Type: ETransactionType.Withdraw, Amount: > 0 })
            {
                request.Amount *= -1;
            }
            ;

            try
            {
                var transaction = await context.Transactions
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (transaction is null)
                {
                    return new Response<Transaction?>(data: null, code: 404, message: "Transação não encontrada!");
                }

                transaction.CategoryId = request.CategoryId;
                transaction.Amount = request.Amount;
                transaction.Title = request.Title;
                transaction.Type = request.Type;
                transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

                context.Transactions.Update(transaction);
                await context.SaveChangesAsync();
                return new Response<Transaction?>(data: transaction, code: 201, message: "Transação atualizada com sucesso.");
            }
            catch (Exception)
            {
                return new Response<Transaction?>(data: null, code: 500, message: "Não foi possível atualizar sua transação.");
            }
        }
    }
}
