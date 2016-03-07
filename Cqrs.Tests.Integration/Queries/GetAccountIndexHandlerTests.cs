using System.Configuration;
using System.Threading.Tasks;
using CqrsBank.Domain.Commands;
using CqrsBank.Domain.Models;
using CqrsBank.Infrastructure;
using CqrsBank.Query.Handlers;
using CqrsBank.Query.Queries;
using NUnit.Framework;

namespace Cqrs.Tests.Integration.Queries
{
  [TestFixture]
  public class GetAccountIndexHandlerTests : DatabaseTest
  {
    [Test]
    public async Task GetsAllAccounts()
    {
      var account1 = new BankAccount(new AddAccount {Name = "Checking"});
      var account2 = new BankAccount(new AddAccount {Name = "Savings"});

      var context = new AccountContext(CONNECTION_STRING_NAME);

      context.Accounts.Add(account1);
      context.Accounts.Add(account2);
      await context.SaveChangesAsync();

      var cs = ConfigurationManager.ConnectionStrings[CONNECTION_STRING_NAME].ConnectionString;
      var query = new GetAccountIndex();
      var db = new SqlConnectionFactory(cs);
      var handler = new GetAccountIndexHandler(db);
      var result = await handler.HandleAsync(query);

      Assert.AreEqual(2, result.Accounts.Count);
      Assert.AreEqual("Checking", result.Accounts[0].Name);
      Assert.AreEqual("Savings", result.Accounts[1].Name);
    }
  }
}