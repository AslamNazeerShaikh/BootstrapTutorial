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
            // Cookie Security Settings
            options.Cookie.HttpOnly     = true;                      // Prevent access to cookie via JavaScript
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure cookies are sent only over HTTPS
            options.Cookie.SameSite     = SameSiteMode.Strict;       // Prevent CSRF
            options.Cookie.Name         = "MySecureAuthCookie";      // Custom cookie name

            // Cookie Lifetime Management
            options.ExpireTimeSpan      = TimeSpan.FromMinutes(60);  // Cookie expiration
            options.SlidingExpiration   = true;                      // Refresh expiration on active use

            // Redirect Paths
            options.LoginPath           = "/Account/Login";          // Redirect unauthorized users
            options.LogoutPath          = "/Account/Logout";         // Redirect on logout
            options.AccessDeniedPath    = "/Account/AccessDenied";   // Redirect unauthorized actions

            // Custom Events for Cookie Validation
            options.Events = new CookieAuthenticationEvents
            {
                OnValidatePrincipal = async context =>
                {
                    var userPrincipal = context.Principal;
                    if (userPrincipal == null || userPrincipal.Identity == null || !userPrincipal.Identity.IsAuthenticated)
                    {
                        context.RejectPrincipal(); // Reject if the user is not authenticated
                        await context.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme); // Use Identity default scheme
                    }
                }
            };
        });

        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit            = true;                     // Require at least one numeric digit.
            options.Password.RequireLowercase        = true;                     // Require at least one lowercase character.
            options.Password.RequireNonAlphanumeric  = true;                     // Require at least one special character.
            options.Password.RequireUppercase        = true;                     // Require at least one uppercase character.
            options.Password.RequiredLength          = 8;                        // Minimum length of 6 characters.
            options.Password.RequiredUniqueChars     = 1;                        // At least one unique character in the password.

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan   = TimeSpan.FromMinutes(5);  // Lockout duration after failed attempts.
            options.Lockout.MaxFailedAccessAttempts  = 5;                        // Max failed login attempts allowed.
            options.Lockout.AllowedForNewUsers       = true;                     // Enable lockout for newly created users.

            // User settings.
            options.User.AllowedUserNameCharacters   =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; // Allowed characters in usernames.
            options.User.RequireUniqueEmail          = true;                           // Require each user to have a unique email.
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
