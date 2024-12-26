using BootstrapTutorial.WebUi.Database;
using BootstrapTutorial.WebUi.Helpers;
using BootstrapTutorial.WebUi.Models;
using BootstrapTutorial.WebUi.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace BootstrapTutorial.WebUi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Add Authentication services
        builder.Services.AddAuthentication("MyCookieAuth")
            .AddCookie("MyCookieAuth", options =>
            {
                options.Cookie.HttpOnly     = true;                      // Prevent access to cookie via JavaScript
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure cookies are sent only over HTTPS
                options.Cookie.SameSite     = SameSiteMode.Strict;       // Prevent CSRF
                options.Cookie.Name         = "MySecureAuthCookie";
                options.LoginPath           = "/Account/Login";          // Redirect unauthorized users
                options.LogoutPath          = "/Account/Logout";
                options.AccessDeniedPath    = "/Account/AccessDenied";   // Redirect unauthorized actions
                options.ExpireTimeSpan      = TimeSpan.FromMinutes(60);  // Cookie expiration
                options.SlidingExpiration   = true;                      // Refresh expiration on active use
                options.Events              = new CookieAuthenticationEvents
                {
                    // Handle cookie validation to detect tampering or session hijacking
                    OnValidatePrincipal = async context =>
                    {
                        var userPrincipal = context.Principal;
                        if (userPrincipal == null || userPrincipal.Identity == null || !userPrincipal.Identity.IsAuthenticated)
                        {
                            context.RejectPrincipal();
                            await context.HttpContext.SignOutAsync("MyCookieAuth");
                        }
                    }
                };
            });

        builder.Services.AddScoped<IGenericRepository<Product>, ProductRepository>();

        builder.Services.AddOpenApi();

        // Ensure database is created
        DatabaseHelper.EnsureDatabase(builder.Configuration.GetConnectionString("LocalDatabase"));

        // Add services to the container
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("LocalDatabase")));

        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

        builder.Services.AddAuthorization(options =>
        {
            // Admin Policy
            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireRole("Admin")
                .RequireClaim("Permission", "ManageUsers"));

            // Employee Policy
            options.AddPolicy("InternalEmployeePolicy", policy =>
                policy.RequireRole("InternalEmployee"));

            // External User Policy
            options.AddPolicy("ExternalUserPolicy", policy =>
                policy.RequireRole("ExternalUser"));
        });

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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}
