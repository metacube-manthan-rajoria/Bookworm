using Bookworm.Data;
using Bookworm.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookworm.Controllers
{
    public class CategoryController : Controller
    {
        // GET: CategoryController
        public IActionResult Index()
        {
            List<Category>? categories = ApplicationDbClient.GetCategoryList();
            int? maxDisplayOrder = ApplicationDbClient.RunMaxDisplayOrderQuery();
            ViewBag.categories = categories;
            ViewBag.maxDisplayOrder = maxDisplayOrder;
            return View();
        }

        [HttpGet]
        public IActionResult Add(){
            return View();
        }

        [HttpPost]
        public IActionResult Add(Category category){
            ApplicationDbClient.RunInsertQuery(category);
            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public IActionResult Edit(int id){
            List<Category>? categories = ApplicationDbClient.GetCategoryList();
            Category? categoryToEdit = null;
            foreach(var category in categories!){
                if(category.Id == id) categoryToEdit = category;
            }
            return View(categoryToEdit);
        }

        [HttpPost]
        public IActionResult Edit(Category category){
            bool updated = ApplicationDbClient.RunCrudUpdateQuery(category);
            if(!updated) ViewBag.error = "Could not update the category";
            return RedirectToAction("Index","Category");
        }

        public IActionResult Delete(int id){
            bool deleted = ApplicationDbClient.RunCrudDeleteQuery(id);
            if(!deleted) ViewBag.error = "Could not delete the category";
            return RedirectToAction("Index","Category");
        }

        public IActionResult IncrementDO(int id){
            bool increasedDo = ApplicationDbClient.RunDOQuery(id);
            
            if(!increasedDo) ViewBag.error = "Could not update the category";
            return RedirectToAction("Index","Category");
        }
    }
}
