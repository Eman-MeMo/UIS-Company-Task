using Microsoft.Extensions.DependencyInjection;
using ProductManagmentApp.Data;
using ProductManagmentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductManagmentApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductManagementContext context;

        public ProductController(ProductManagementContext _context)
        {
            context = _context;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAll()
        {
            var products = context.Products.ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if(ModelState.IsValid)
            {
                product.GeneratedCode = Guid.NewGuid().ToString();
                context.Products.Add(product);
                context.SaveChanges();
                
                return Json(new { success = true, message = "Product created successfully!" });
            }

            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                           .ToDictionary(
                               kvp => kvp.Key,
                               kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                           );

            return Json(new { success = false, errors });
        }

    }
}