using Microsoft.EntityFrameworkCore;
using ProductManagmentApp.Data;
using System.Configuration;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace ProductManagmentApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            string connectionString = ConfigurationManager.ConnectionStrings["ProductManagmentConnection"].ConnectionString;

            container.RegisterFactory<ProductManagementContext>(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ProductManagementContext>();
                optionsBuilder.UseSqlServer(connectionString);
                return new ProductManagementContext(optionsBuilder.Options);
            });

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

        }
    }
}