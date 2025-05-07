using Microsoft.AspNetCore.Authentication;
using TaskManagement.Core.TypeFinder;
using TaskManagement.Core;
using TaskManagement.Services.SettingsStore;
using TaskManagement.Web.Middlewares;
using TaskManagement.Services.LoginServices;
using TaskManagement.Services;
namespace TaskManagement.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            Settings.LoadFromConfiguration(builder.Configuration);


            // Register Dependency Injection
            ITypeFinder typeFinder = new TypeFinder();

            builder.Services.AddSingleton<ITypeFinder>(typeFinder);

            builder.Services.AddHttpContextAccessor();

            // Register other services from all layers
            builder.Services.RegisterDependencies(typeFinder, builder.Configuration);


            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Authentication/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authentication}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
