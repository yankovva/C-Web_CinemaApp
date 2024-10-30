using CinemaApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Web.Infrastucture.Extensions;
using CinemaWeb.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System.Configuration;
using Microsoft.AspNetCore.Components.Web;
using CinemaApp.Services.Mapping;
using CinemaApp.Web.ViewModels;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Data.Services.Interfaces;
using CinemaApp.Data.Services;

//internal class Program
//{
//    private static void Main(string[] args)
//    {
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
   ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<CinemaDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:Password:RequireDigits");
    options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
    options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
    options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
    options.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
    options.Password.RequiredUniqueChars = builder.Configuration.GetValue<int>("Identity:Password:RequiredUniqueChars");

    options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
    options.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
    options.SignIn.RequireConfirmedPhoneNumber = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber");

    options.User.RequireUniqueEmail = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireUniqueEmail");



})
   .AddEntityFrameworkStores<CinemaDbContext>()
  .AddRoles<IdentityRole<Guid>>()
  .AddSignInManager<SignInManager<ApplicationUser>>()
  .AddUserManager<UserManager<ApplicationUser>>();

builder.Services.ConfigureApplicationCookie(cfg =>
{
    cfg.LoginPath = "/Identity/Account/Login";
});


//builder.Services.AddScoped<IRepository<Movie, Guid>, Repository<Movie, Guid>>();
//builder.Services.AddScoped<IRepository<Cinema, Guid>, Repository<Cinema, Guid>>();
//builder.Services.AddScoped<IRepository<CinemaMovie, object>, Repository<CinemaMovie, object>>();
//builder.Services.AddScoped<IRepository<UserMovie, object>, Repository<UserMovie, object>>();

builder.Services.RegisterRepositories(typeof(ApplicationUser).Assembly);

builder.Services.RegisterUserDefinedServices(typeof(IMovieService).Assembly);

builder.Services.AddScoped<ICinemaService, CinemaService>();


builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

WebApplication app = builder.Build();

AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.ApplyMigrations(); //- syzdavame nash extension method koito apply-wa migraciite pri build na prilojenieto
app.Run();
//    }
//}