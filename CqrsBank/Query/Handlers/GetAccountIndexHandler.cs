using System.Linq;
using System.Threading.Tasks;
using CqrsBank.Infrastructure;
using CqrsBank.Query.Queries;
using CqrsBank.Query.Results;
using Dapper;

namespace CqrsBank.Query.Handlers
{
  public class GetAccountIndexHandler : IAsyncQueryHandler<GetAccountIndex, AccountIndex>
  {
    private readonly IDbConnectionFactory _db;

    public GetAccountIndexHandler(IDbConnectionFactory db)
    {
      _db = db;
    }

    public async Task<AccountIndex> HandleAsync(GetAccountIndex query)
    {
      const string sql = "select [Id], [Name] from [BankAccount] order by name";

      using (var cn = await _db.GetOpenConnectionAsync())
      {
        return new AccountIndex
        {
          Accounts = (await cn.QueryAsync<AccountIndexItem>(sql, query)).ToList()
        };
      }
    }
  }
}