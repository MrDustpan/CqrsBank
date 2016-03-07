using System.Linq;
using System.Threading.Tasks;
using CqrsBank.Infrastructure;
using CqrsBank.Query.Queries;
using CqrsBank.Query.Results;
using Dapper;

namespace CqrsBank.Query.Handlers
{
  public class GetAccountDetailsHandler : IAsyncQueryHandler<GetAccountDetails, AccountDetails>
  {
    private readonly IDbConnectionFactory _db;

    public GetAccountDetailsHandler(IDbConnectionFactory db)
    {
      _db = db;
    }

    public async Task<AccountDetails> HandleAsync(GetAccountDetails query)
    {
      const string sql = 
        @"select [Id], [Name], [Balance] from [BankAccount] where [Id] = @id;
        select [Description], [Amount] from [Transaction] where [AccountId] = @id";

      using (var cn = await _db.GetOpenConnectionAsync())
      using (var multi = await cn.QueryMultipleAsync(sql, query))
      {
        var details = (await multi.ReadAsync<AccountDetails>()).FirstOrDefault();
        if (details != null)
        {
          details.Transactions = (await multi.ReadAsync<TransactionItem>()).ToList();
        }
        return details;
      }
    }
  }
}