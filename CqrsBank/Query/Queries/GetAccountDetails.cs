using CqrsBank.Infrastructure;
using CqrsBank.Query.Results;

namespace CqrsBank.Query.Queries
{
  public class GetAccountDetails : IQuery<AccountDetails>
  {
    public int Id { get; set; }

    public GetAccountDetails()
    {
    }

    public GetAccountDetails(int id)
    {
      Id = id;
    }
  }
}