using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManagmentApp.Data;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ProductManagmentApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var services = new ServiceCollection();

            services.AddDbContext<ProductManagementContext>(options =>
                options.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=ProductManagmentDB;Integrated Security=True;TrustServerCertificate=True"));

            ServiceProvider = services.BuildServiceProvider();
            UnityConfig.RegisterComponents();
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
        }
    }
}
