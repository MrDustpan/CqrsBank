using System.Collections.Generic;

namespace CqrsBank.Query.Results
{
  public class AccountIndex
  {
    public List<AccountIndexItem> Accounts { get; set; }

    public AccountIndex()
    {
      Accounts = new List<AccountIndexItem>();
    } 
  }

  public class AccountIndexItem
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }
}