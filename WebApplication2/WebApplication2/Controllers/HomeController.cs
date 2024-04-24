using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [Route("api/home")]
    public class HomeController : Controller
    {
        [Route("page")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
