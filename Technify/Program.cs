using Microsoft.AspNetCore.Identity;
using Technify.Entities;
using Technify.Data;
using Technify.Utilities.EmailHandler.Abstract;
using Technify.Utilities.EmailHandler.Concrete;
using Microsoft.EntityFrameworkCore;
using Technify;
using Technify.Utilities.EmailHandler.Models;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;


}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

var emailConfiguration = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfiguration);
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();
app.MapControllerRoute(
    name: "areas",
     pattern: "{area:exists}/{controller=Dashboard}/{action=index}/{id?}"
    );


app.MapControllerRoute(

   name: "default",
   pattern: "{controller=account}/{action=register}"
    );
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    DbInitializer.Seed(userManager, roleManager);
}

app.Run();










