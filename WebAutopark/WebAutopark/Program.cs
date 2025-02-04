using WebAutopark.Controllers;
using WebAutopark.Data;
using WebAutopark.Data.Repositories.Implementations;
using WebAutopark.Data.Repositories.Interfaces;
using WebAutopark.Middleware;
using WebAutopark.Models;

namespace WebAutopark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<DapperDbContext>(provider => 
                new DapperDbContext(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddTransient<IVehicleTypeRepository, VehicleTypeRepository>();
            builder.Services.AddTransient<IVehicleRepository, VehicleRepository>();
            builder.Services.AddTransient<IOrderRepository, OrderRepository>();
            builder.Services.AddTransient<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddTransient<IComponentRepository, ComponentRepository>();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
