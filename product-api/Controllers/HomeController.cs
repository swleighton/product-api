
using System.Web.Mvc;
using productapi.Models;

namespace productapi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new HomepageViewModel());
        }
    }
}
