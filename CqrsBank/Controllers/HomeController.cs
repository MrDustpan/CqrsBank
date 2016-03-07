using System.Threading.Tasks;
using System.Web.Mvc;
using CqrsBank.Domain.Commands;
using CqrsBank.Infrastructure;
using CqrsBank.Query.Queries;

namespace CqrsBank.Controllers
{
  public class HomeController : Controller
  {
    private readonly IQueryProcessor _queryProcessor;
    private readonly ICommandProcessor _commandProcessor;

    public HomeController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
    {
      _queryProcessor = queryProcessor;
      _commandProcessor = commandProcessor;
    }

    public async Task<ActionResult> Index(GetAccountIndex query)
    {
      var accountIndex = await _queryProcessor.ProcessAsync(query);
      return View(accountIndex);
    }

    public ActionResult Add()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Add(AddAccount command)
    {
      await _commandProcessor.ProcessAsync(command);

      return RedirectToAction("Index");
    }

    public async Task<ActionResult> Details(GetAccountDetails query)
    {
      var accountDetails = await _queryProcessor.ProcessAsync(query);
      return View(accountDetails);
    }

    [HttpPost]
    public async Task<ActionResult> Delete(DeleteAccount command)
    {
      await _commandProcessor.ProcessAsync(command);
      return RedirectToAction("Index");
    }
  }
}