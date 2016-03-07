using System.Linq;
using System.Threading.Tasks;
using CqrsBank.Infrastructure;
using CqrsBank.Query.Queries;
using CqrsBank.Query.Results;
using Dapper;

namespace CqrsBank.Query.Handlers
{
  public class GetNewTransactionHandler : IAsyncQueryHandler<GetNewTransaction, NewTransaction>
  {
    private readonly IDbConnectionFactory _db;

    public GetNewTransactionHandler(IDbConnectionFactory db)
    {
      _db = db;
    }

    public async Task<NewTransaction> HandleAsync(GetNewTransaction query)
    {
      const string sql = "select [Id] [AccountId], [Name] [AccountName] from [BankAccount] where [Id] = @accountId";

      using (var cn = await _db.GetOpenConnectionAsync())
      {
        return (await cn.QueryAsync<NewTransaction>(sql, query)).FirstOrDefault();
      }
    }
  }
}