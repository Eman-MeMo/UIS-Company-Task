using ProductManagmentApp.Data;
using ProductManagmentApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductManagmentApp.Controllers
{
    public class TransactionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAll(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
                startDate = DateTime.MinValue;
            if (!endDate.HasValue)
                endDate = DateTime.MaxValue;

            using (var dbContext = new ProductManagmentContext())
            {
                var transactions = dbContext.Transactions
                     .Where(t => t.Date >= startDate.Value && t.Date <= endDate.Value)
                     .Include(t => t.Product)
                     .Select(t => new
                     {
                         t.Quantity,
                         t.Date,
                         t.TotalPrice,
                         Product = new
                         {
                             t.Product.Name,
                             t.Product.Unit
                         }
                     })
                     .ToList();

                ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
                ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
                return Json(transactions, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Create(int productId)
        {
            using (var context = new ProductManagmentContext())
            {
                var product = context.Products.Find(productId);
                if (product != null)
                {
                    var transaction = new Transaction
                    {
                        ProductId = product.Id,
                    };
                    return View(transaction);
                }
                else
                {
                    return HttpNotFound("Product not found");
                }
            }
        }
        [HttpPost]
        public ActionResult Create(Transaction transaction)
        {
            if (transaction.Date > DateTime.Now)
            {
                ModelState.AddModelError("Date", "Transaction date cannot be in the future.");
            }

            if (ModelState.IsValid)
            {
                using (var context = new ProductManagmentContext())
                {
                    var product = context.Products.Find(transaction.ProductId);

                    if (product != null)
                    {
                        if (transaction.Quantity > product.InitialQuantity)
                        {
                            ModelState.AddModelError("Quantity", $"Transaction quantity cannot be greater than the available stock, which is {product.InitialQuantity}.");
                            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                                .ToDictionary(
                                    kvp => kvp.Key,
                                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                );
                            return Json(new { success = false, errors });
                        }

                        var newTransaction = new Transaction
                        {
                            ProductId = transaction.ProductId,
                            Quantity = transaction.Quantity,
                            Date = transaction.Date,
                            TotalPrice = product.Price * transaction.Quantity
                        };

                        product.InitialQuantity -= transaction.Quantity;

                        context.Transactions.Add(newTransaction);
                        context.Entry(product).State = EntityState.Modified;

                        context.SaveChanges();

                        return Json(new { success = true, message = "Transaction created successfully!" });
                    }

                    return Json(new { success = false, message = "Product not found!" });
                }
            }
            else
            {
                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return Json(new { success = false, errors });
            }
        }
    }
}