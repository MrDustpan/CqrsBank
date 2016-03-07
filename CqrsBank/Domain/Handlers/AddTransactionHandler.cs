using System.Data.Entity;
using System.Threading.Tasks;
using CqrsBank.Domain.Commands;
using CqrsBank.Infrastructure;

namespace CqrsBank.Domain.Handlers
{
  public class AddTransactionHandler : IAsyncCommandHandler<AddTransaction>
  {
    private readonly AccountContext _context;

    public AddTransactionHandler(AccountContext context)
    {
      _context = context;
    }

    public async Task HandleAsync(AddTransaction command)
    {
      var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == command.AccountId);
      account.AddTransaction(command);
      await _context.SaveChangesAsync();
    }
  }
}