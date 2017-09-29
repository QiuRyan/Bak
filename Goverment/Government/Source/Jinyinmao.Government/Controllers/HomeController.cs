using System.Web.Mvc;

namespace Jinyinmao.Government.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            this.ViewBag.Title = "Jinyinmao.Government";
            return this.View();
        }
    }
}