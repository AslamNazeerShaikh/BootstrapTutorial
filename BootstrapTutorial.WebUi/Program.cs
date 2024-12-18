using BootstrapTutorial.WebUi.Helpers;
using BootstrapTutorial.WebUi.Models;
using BootstrapTutorial.WebUi.Repositories;
using Scalar.AspNetCore;

namespace BootstrapTutorial.WebUi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddScoped<IGenericRepository<Product>, ProductRepository>();

        builder.Services.AddOpenApi();

        // Ensure database is created
        DatabaseHelper.EnsureDatabase();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        else
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}
