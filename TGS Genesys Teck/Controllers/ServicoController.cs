using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TGS_Genesys_Teck.Models;

namespace TGS_Genesys_Teck.Controllers
{
    public class ServicoController : Controller
    {
        private readonly ILogger<ServicoController> _logger;

        public ServicoController(ILogger<ServicoController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
