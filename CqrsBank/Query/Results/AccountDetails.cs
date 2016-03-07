using System.Collections.Generic;

namespace CqrsBank.Query.Results
{
  public class AccountDetails
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public List<TransactionItem> Transactions { get; set; }

    public AccountDetails()
    {
      Transactions = new List<TransactionItem>();
    } 
  }

  public class TransactionItem
  {
    public string Description { get; set; }
    public decimal Amount { get; set; }
  }
}