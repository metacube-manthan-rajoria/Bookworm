using Bookworm.Data;
using Bookworm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public IActionResult Edit(int? id){
            if(id == null || id == 0) return NotFound();

            Category? categoryToEdit = _db.Categories.Find(id);
            if(categoryToEdit == null) return NotFound();

            return View(categoryToEdit);
        }

        [HttpPost]
        public IActionResult Edit(Category category){
            if(ModelState.IsValid){
                _db.Update(category);
                _db.SaveChanges();
            }else{
                ViewBag.error = "Could not update the category";
            }
            
            return RedirectToAction("Index","Category");
        }

        public IActionResult Delete(int? id){
            if(id == null || id == 0) return NotFound();

            Category? category = _db.Categories.Find(id);
            if(category == null) return NotFound();
            
            _db.Categories.Remove(category);
            _db.SaveChanges();
                
            return RedirectToAction("Index","Category");
        }
    }
}
