using System.Data.Entity;
using System.Threading.Tasks;
using CqrsBank.Domain.Commands;
using CqrsBank.Infrastructure;

namespace CqrsBank.Domain.Handlers
{
  public class DeleteAccountHandler : IAsyncCommandHandler<DeleteAccount>
  {
    private readonly AccountContext _context;

    public DeleteAccountHandler(AccountContext context)
    {
      _context = context;
    }

    public async Task HandleAsync(DeleteAccount command)
    {
      var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == command.Id);
      _context.Accounts.Remove(account);
      await _context.SaveChangesAsync();
    }
  }
}