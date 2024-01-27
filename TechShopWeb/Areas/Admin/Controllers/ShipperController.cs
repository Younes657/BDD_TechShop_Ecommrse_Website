using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopTech.DataAccess.Repository;
using ShopTech.DataAccess.Repository.IRepository;
using ShopTech.Models;
using ShopTech.Utility;

namespace TechShopWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    [Area("Admin")]
	public class ShipperController : Controller
	{
		private readonly IUnitOfWork UnitOfWork;
        public ShipperController(IUnitOfWork unitofwork)
        {
			UnitOfWork = unitofwork;   
        }
		public IActionResult Index()
		{
			IEnumerable<Shipper> Shippers = UnitOfWork._ShipperRepository.GetAll("Shippers");
			return View(Shippers);
		}
		public IActionResult Upsert(int? id)
		{
			Shipper Shipper = new Shipper();
			if (id != null && id != 0)
			{
				Shipper = UnitOfWork._ShipperRepository.GetOne("Shippers", "WHERE Id = " + id);
				if (Shipper == null)
				{
					return NotFound();
				}
				return View(Shipper);
			}
			return View(Shipper);
		}
		[HttpPost]
		public IActionResult Upsert(Shipper shipper)
		{
			if (ModelState.IsValid)
			{
				if (shipper.Id == 0)
				{
					UnitOfWork._ShipperRepository.Add(shipper);
					TempData["success"] = "shipper Created successfully";
				}
				else
				{
					UnitOfWork._ShipperRepository.update(shipper);
					TempData["success"] = "shipper Updated successfully";
				}
				return RedirectToAction(nameof(Index));
			}
			else
			{
				return View(shipper);
			}
		}
		[HttpPost]
		public IActionResult Delete(int id)
		{
			Shipper? shipper = UnitOfWork._ShipperRepository.GetOne("Shippers", "Where Id = " + id);
			if (shipper == null) return NotFound();
			else
			{
				UnitOfWork._ShipperRepository.Remove("Shippers", id);
				TempData["success"] = "Shipper Deleted successfully";
				return RedirectToAction(nameof(Index), "Shipper");
			}
		}
	}
}
