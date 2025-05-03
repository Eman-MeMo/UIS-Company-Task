using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductManagmentApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string GeneratedCode { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100, ErrorMessage = "Product Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Unit is required.")]
        public string Unit { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Initial Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Initial Quantity must be a non-negative number.")]
        public int InitialQuantity { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }

}