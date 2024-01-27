using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopTech.DataAccess.Repository.IRepository;
using ShopTech.Models;
using ShopTech.Utility;

namespace TechShopWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _UnitOfWork;
        public CategoryController(IUnitOfWork unit)
        {
            _UnitOfWork = unit;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> Categories = _UnitOfWork._CategoryRepository.GetAll("Categories");
            return View(Categories);
        }
        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id != null && id != 0)
            {
                category = _UnitOfWork._CategoryRepository.GetOne("Categories", "WHERE Id = " + id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Upsert(Category category)
        {
            if(ModelState.IsValid) { 
                if(category.Id == 0)
                {
                    _UnitOfWork._CategoryRepository.Add(category);
                    TempData["success"] = "Category Created successfully";
                }
                else
                {
					_UnitOfWork._CategoryRepository.Update(category);
					TempData["success"] = "Category Updated successfully";
				}
				return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category);
            }
		}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Category? ctg = _UnitOfWork._CategoryRepository.GetOne("Categories","Where Id = "+id);
            if (ctg == null) return NotFound();
            else
            {
                _UnitOfWork._CategoryRepository.Remove("Categories", id);
                TempData["success"] = "Category Deleted successfully";
                return RedirectToAction(nameof(Index), "Category");
            }
        }
    }
}
