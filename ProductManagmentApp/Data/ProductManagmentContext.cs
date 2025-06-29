using Microsoft.EntityFrameworkCore;
using ProductManagmentApp.Models;

namespace ProductManagmentApp.Data
{
    public class ProductManagementContext : DbContext
    {

        public ProductManagementContext(DbContextOptions<ProductManagementContext> options)
            : base(options)
        {
                 
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; } 
    }
}
