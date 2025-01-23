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

        public IActionResult Delete(int id){
            bool deleted = ApplicationDbClient.RunDeleteQuery(id);
            ViewBag.error = "Could not delete the category";
            return RedirectToAction("Index","Category");
        }
    }
}
