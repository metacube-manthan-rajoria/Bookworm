using Bookworm.Data;
using Bookworm.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookworm.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db; 
        }

        // GET: CategoryController
        public IActionResult Index()
        {
            List<Category>? categories = _db.Categories.ToList();
            ViewBag.categories = categories;
            return View();
        }

        [HttpGet]
        public IActionResult Add(){
            return View();
        }

        [HttpPost]
        public IActionResult Add(Category category){
            if(ModelState.IsValid){
                _db.Categories.Add(category);
                _db.SaveChanges();
            }else{
                ViewBag.error = "Could not add Category - Invalid Category";
            }
            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public IActionResult Edit(int id){
            List<Category>? categories = _db.Categories.ToList();
            Category? categoryToEdit = null;
            foreach(var category in categories!){
                if(category.Id == id) categoryToEdit = category;
            }
            return View(categoryToEdit);
        }

        [HttpPost]
        public IActionResult Edit(Category category){
            bool updated = ApplicationDbClient.RunUpdateQuery(category);
            if(!updated) ViewBag.error = "Could not update the category";
            return RedirectToAction("Index","Category");
        }

        public IActionResult Delete(int id){
            bool deleted = ApplicationDbClient.RunDeleteQuery(id);
            if(!deleted) ViewBag.error = "Could not delete the category";
            return RedirectToAction("Index","Category");
        }
    }
}
