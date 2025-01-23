using Bookworm.Data;
using Bookworm.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookworm.Controllers
{
    public class CategoryController : Controller
    {
        // GET: CategoryController
        public ActionResult Index()
        {
            List<Category>? categories = ApplicationDbClient.RunSelectQuery();
            ViewBag.categories = categories;
            return View();
        }
    }
}
