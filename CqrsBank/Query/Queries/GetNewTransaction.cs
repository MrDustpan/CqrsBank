using CqrsBank.Infrastructure;
using CqrsBank.Query.Results;

namespace CqrsBank.Query.Queries
{
  public class GetNewTransaction : IQuery<NewTransaction>
  {
    public int AccountId { get; set; }

    public GetNewTransaction()
    {
    }

    public GetNewTransaction(int accountId)
    {
      AccountId = accountId;
    }
  }
}