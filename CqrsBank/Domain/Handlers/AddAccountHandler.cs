using System.Threading.Tasks;
using CqrsBank.Domain.Commands;
using CqrsBank.Domain.Models;
using CqrsBank.Infrastructure;

namespace CqrsBank.Domain.Handlers
{
  public class AddAccountHandler : IAsyncCommandHandler<AddAccount>
  {
    private readonly AccountContext _context;

    public AddAccountHandler(AccountContext context)
    {
      _context = context;
    }

    public async Task HandleAsync(AddAccount command)
    {
      var account = new BankAccount(command);
      _context.Accounts.Add(account);
      await _context.SaveChangesAsync();
    }
  }
}