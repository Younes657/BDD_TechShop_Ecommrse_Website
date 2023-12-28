using Humanizer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShopTech.DataAccess.Repository.IRepository;
using ShopTech.Models;
using ShopTech.Models.VModels;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.Arm;

namespace TechShopWeb.Areas.Admin.Controllers
{ 
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductController(IUnitOfWork unitofwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitofwork;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork._ProductRepository.GetAll("Products", IncludePr: "Category");
            
            return View(products);
        }
        // update + insert = upsert
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> ListCategory = _unitOfWork._CategoryRepository.GetAll("Categories").Select(x =>
                new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.Id.ToString(),
                }
            );
            ProductVM prdvm = new()
            {
                Product = new Product(),
                CategoryList = ListCategory
            };
            if (id == null || id == 0)
                return View(prdvm);
            else
            {
                //update
                prdvm.Product = _unitOfWork._ProductRepository.GetOne("Products" , "Where Id = "+id);
                return View(prdvm);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM prdVM , IFormFile? file)
        {
            var checkPrd = _unitOfWork.AppDbContext().Products.FirstOrDefault(x => x.ProductName == prdVM.Product.ProductName);
            if(checkPrd != null && prdVM.Product.Id == 0)
            {
                ModelState.AddModelError("Product.ProductName", "The Product Already Exist You can update it.");
            }
            if (ModelState.IsValid)
            {
                string rootPath = _WebHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString()+ Path.GetFileName(file.FileName);
                    //where the image should be stored
                    var ProductsPath = Path.Combine(rootPath, @"images\Products");
                    //this if is for the update
                    if (!string.IsNullOrEmpty(prdVM.Product.ImageUrl))
                    {
                        var oldImgpath = Path.Combine(rootPath, prdVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImgpath))
                        {
                            System.IO.File.Delete(oldImgpath);
                        }
                    }
                    using (FileStream stream = new FileStream(Path.Combine(ProductsPath, filename), FileMode.Create))
                    {
                        //Copies the contents of the uploaded file to the target stream.
                        file.CopyTo(stream);
                    }
                    prdVM.Product.ImageUrl = @"\images\Products\" + filename;
                }
                if (prdVM.Product.Id == 0)
                {
                    _unitOfWork._ProductRepository.Add(prdVM.Product);
                    TempData["success"] = "Product Created successfully";
                }
                else
                {
                    _unitOfWork._ProductRepository.update(prdVM.Product);
                    TempData["success"] = "Product Updated successfully";
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                prdVM.CategoryList = _unitOfWork._CategoryRepository.GetAll("Categories").Select(x =>
                    new SelectListItem
                    {
                        Text = x.CategoryName,
                        Value = x.Id.ToString()
                    }
                );
                return View(prdVM);
            }
        }
        [HttpPost]
        public IActionResult Search(string ValueToSearch , string? sortVal) 
        {
            //we should handle the submit event if the search value is empty by js;
            string sqlQuery;
            if (string.IsNullOrEmpty(sortVal))
            {
                 sqlQuery = $"Select P.* From Products as P INNER JOIN  Categories as C ON P.CategoryId = C.Id Where C.CategoryName LIKE '%{ValueToSearch}%' OR P.ProductName Like '%{ValueToSearch}%' OR P.CompanyName Like '%{ValueToSearch}%'";
            }
            else
            {  
                sqlQuery = $"Select TOP (100) PERCENT P.* From Products as P INNER JOIN  Categories as C ON P.CategoryId = C.Id Where C.CategoryName LIKE '%{ValueToSearch}%' OR P.ProductName Like '%{ValueToSearch}%' OR P.CompanyName Like '%{ValueToSearch}%' Order By" +
                   $" {sortVal.Replace(" ", "")}";
            }
            
            IEnumerable<Product> SearchPrd = _unitOfWork.AppDbContext().Products.FromSqlRaw(sqlQuery).ToList();
            foreach(var item in SearchPrd)
            {
                 _unitOfWork.AppDbContext().Entry(item).Reference(x => x.Category).Load();
                
            }
            TempData["searchValue"] = ValueToSearch;
            return View(nameof(Index),SearchPrd);
        }
        [HttpPost]
        public IActionResult Reset()
        {
            IEnumerable<Product> products = _unitOfWork._ProductRepository.GetAll("Products", IncludePr: "Category");

            return View(nameof(Index) , products);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var prd = _unitOfWork._ProductRepository.GetOne("Products","Where Id = "+id , IncludePr:"Category");

            if (prd == null)
                return NotFound();
            // we delete the image if it exists 
            string rootPath = _WebHostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(prd.ImageUrl))
            {
                var oldImgpath = Path.Combine(rootPath, prd.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImgpath))
                {
                    System.IO.File.Delete(oldImgpath);
                }
            }
            _unitOfWork._ProductRepository.Remove("Products", id);
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction(nameof(Index));
        }
        // add the sortby functionality 
        [HttpPost]
        public IActionResult SortedBy(string sortedby , string? ValueToSearch)
        {
            string sqlQuery;
            if (string.IsNullOrEmpty(ValueToSearch))
            {
                sqlQuery = $"Select TOP (100) PERCENT P.* from Products P INNER JOIN Categories C On P.CategoryId = C.ID Order By {sortedby}";
            }
            else
            {
                sqlQuery = $"Select TOP (100) PERCENT P.* From Products as P INNER JOIN  Categories as C ON P.CategoryId = C.Id Where C.CategoryName LIKE '%{ValueToSearch}%' OR P.ProductName Like '%{ValueToSearch}%' OR P.CompanyName Like '%{ValueToSearch}%' Order By {sortedby}";

			}
            var sortedProducts = _unitOfWork.AppDbContext().Products.FromSqlRaw(sqlQuery).ToList();
            foreach(var item in sortedProducts)
            {
                var Category = _unitOfWork._CategoryRepository.GetOne("Categories", "Where Id = "+item.CategoryId);
                item.Category = Category;
            }
            TempData["searchValue"] = ValueToSearch;
            return View(nameof(Index) , sortedProducts);
        }
    }
}
