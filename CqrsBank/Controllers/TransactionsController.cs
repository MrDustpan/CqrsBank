using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CqrsBank.Domain.Commands;
using CqrsBank.Infrastructure;
using CqrsBank.Query.Queries;

namespace CqrsBank.Controllers
{
  public class TransactionsController : Controller
  {
    private readonly IQueryProcessor _queryProcessor;
    private readonly ICommandProcessor _commandProcessor;

    public TransactionsController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
    {
      _queryProcessor = queryProcessor;
      _commandProcessor = commandProcessor;
    }

    public async Task<ActionResult> Add(GetNewTransaction query)
    {
      var newTransaction = await _queryProcessor.ProcessAsync(query);
      return View(newTransaction);
    }

    [HttpPost]
    public async Task<ActionResult> Add(AddTransaction command)
    {
      await _commandProcessor.ProcessAsync(command);
      return RedirectToAction("Details", "Home", new GetAccountDetails(command.AccountId));
    }
  }
}