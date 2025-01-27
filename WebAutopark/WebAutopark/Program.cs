using WebAutopark.Controllers;
using WebAutopark.Data;
using WebAutopark.Data.Repositories;
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

            builder.Services.AddTransient<IRepository<VehicleType>, VehicleTypeRepository>();
            builder.Services.AddTransient<IRepository<Vehicle>, VehicleRepository>();
            builder.Services.AddTransient<IRepository<Order>, OrderRepository>();
            builder.Services.AddTransient<IRepository<OrderItem>, OrderItemRepository>();
            builder.Services.AddTransient<IRepository<Component>, ComponentRepository>();
            

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
