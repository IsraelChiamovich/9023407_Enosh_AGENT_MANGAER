using Microsoft.AspNetCore.Mvc;

namespace TargetsMvc.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
