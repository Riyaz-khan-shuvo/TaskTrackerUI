using Microsoft.AspNetCore.Mvc;

namespace TaskTracker.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public DashboardController()
        {
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
