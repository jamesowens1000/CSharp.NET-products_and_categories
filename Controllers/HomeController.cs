using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsAndCategories.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductsAndCategories.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        //This root route (ie. Index method) is simply here to redirect the user to the products route
        [HttpGet("")]
        public IActionResult Index()
        {
            return Redirect("products");
        }

//////////////////////////////PRODUCT METHODS\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        [HttpGet("products")]
        public IActionResult Products()
        {
            List<Product> EveryProduct = dbContext.Products.ToList();
            ViewBag.AllProducts = EveryProduct;
            return View("Products");
        }

        [HttpPost("InsertProduct")]
        public IActionResult InsertProduct(Product newProd)
        {
            dbContext.Add(newProd);
            dbContext.SaveChanges();
            return RedirectToAction("Products");
        }

        [HttpGet("products/{prodId}")]
        public IActionResult SpecificProduct(int prodId)
        {
            Product getProd = dbContext.Products.FirstOrDefault(p => p.ProductId == prodId);
            ViewBag.ThisProduct = getProd;

            var prodWithCats = dbContext.Products
                .Include(p => p.AssocCats)
                .ThenInclude(c => c.Category)
                .FirstOrDefault(p => p.ProductId == prodId);
            
            ViewBag.ProductWithCategories = prodWithCats;

            List<Category> EveryCategory = dbContext.Categories.ToList();
            List<Category> SomeCategories = new List<Category>();

            foreach (var c in prodWithCats.AssocCats)
            {
                SomeCategories.Add(c.Category);
            }
            List<Category> NotYetAssoc = EveryCategory.Except(SomeCategories).ToList();
            ViewBag.NotYetAssoc = NotYetAssoc;
            return View("SpecificProduct");
        }

        [HttpPost("AddCatToProd")]
        public IActionResult AddCatToProd(Association newAssoc)
        {
            dbContext.Add(newAssoc);
            dbContext.SaveChanges();
            return Redirect("/products/"+newAssoc.ProductId);
        }

//////////////////////////////CATEGORY METHODS\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        [HttpGet("categories")]
        public IActionResult Categories()
        {
            List<Category> EveryCategory = dbContext.Categories.ToList();
            ViewBag.AllCategories = EveryCategory;
            return View("Categories");
        }

        [HttpPost("InsertCategory")]
        public IActionResult InsertCategory(Category newCatgry)
        {
            dbContext.Add(newCatgry);
            dbContext.SaveChanges();
            return RedirectToAction("Categories");
        }

        [HttpGet("categories/{catId}")]
        public IActionResult SpecificCategory(int catId)
        {
            Category getCat = dbContext.Categories.FirstOrDefault(p => p.CategoryId == catId);
            ViewBag.ThisCategory = getCat;

            var catWithProds = dbContext.Categories
                .Include(p => p.AssocProds)
                .ThenInclude(c => c.Product)
                .FirstOrDefault(p => p.CategoryId == catId);
            
            ViewBag.CategoryWithProducts = catWithProds;

            List<Product> EveryProduct = dbContext.Products.ToList();
            List<Product> SomeProducts = new List<Product>();

            foreach (var p in catWithProds.AssocProds)
            {
                SomeProducts.Add(p.Product);
            }
            List<Product> NotYetAssoc = EveryProduct.Except(SomeProducts).ToList();
            ViewBag.NotYetAssoc = NotYetAssoc;
            return View("SpecificCategory");
        }

        [HttpPost("AddProdToCat")]
        public IActionResult AddProdToCat(Association newAssoc)
        {
            dbContext.Add(newAssoc);
            dbContext.SaveChanges();
            return Redirect("/categories/"+newAssoc.CategoryId);
        }
    }
}