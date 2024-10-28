using Clinical_Management_System.Data;
using Clinical_Management_System.DbInitializer;
using Clinical_Management_System.DBInitializer;
using Clinical_Management_System.Models;
using Clinical_Management_System.Models.DB_Entities;
using Clinical_Management_System.Repository;
using Clinical_Management_System.Repository.IRepositery;
using Clinical_Management_System.Utitlity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()/*(options => options.SignIn.RequireConfirmedAccount = true)*/
	.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Identity/Account/Login"; // Path to the login page
	options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Path for access denied
	options.LogoutPath = "/Identity/Account/Logout"; // Path for logout
});
builder.Services.AddAuthentication().AddFacebook(option => {
	option.AppId = "1511817046199494";
	option.AppSecret = "1e1f947672b6983f6d3ac8c51f7cf47a";

});
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IDBInitializer,DBInitializer>();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy(Sd.Role_Doctor,policy=>policy.RequireRole(Sd.Role_Doctor));
	options.AddPolicy(Sd.Role_Patient, policy => policy.RequireRole(Sd.Role_Patient));
	options.AddPolicy(Sd.Role_Admin,policy=>policy.RequireRole(Sd.Role_Admin));
});

var app = builder.Build();

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
SeedDatabase();
app.MapRazorPages();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

void SeedDatabase()
{
	using (var scope = app.Services.CreateScope())
	{
		var dbInitializer = scope.ServiceProvider.GetRequiredService<IDBInitializer>();
		dbInitializer.Initialize();
	}
}
