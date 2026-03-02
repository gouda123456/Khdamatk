using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
