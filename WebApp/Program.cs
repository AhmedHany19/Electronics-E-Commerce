using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.IRepository.ServicesRepository;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAppelcetronics.Permission;
using Infrastructure.Seeds;
using Stripe;
using WebApp.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnections")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(option =>
{
	option.LoginPath = "/Customer/Users/Login";
	option.AccessDeniedPath = "/Admin/Home/Denied";
});

builder.Services.AddSession(options =>
{
	options.Cookie.IsEssential = true;
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;

});


builder.Services.AddScoped<IServicesRepository<Category>, ServicesCategory>();
builder.Services.AddScoped<IServicesRepositoryLog<LogCategory>, ServicesLogCategory>();
builder.Services.AddScoped<IServicesRepository<Brand>, ServicesBrand>();
builder.Services.AddScoped<IServicesRepositoryLog<LogBrand>, ServicesLogBrand>();
builder.Services.AddScoped<IServicesRepository<Domain.Entity.Product>, ServicesProduct>();
builder.Services.AddScoped<IServicesRepositoryLog<LogProduct>, ServicesLogProduct>();

builder.Services.AddScoped<IServicesRepository<Slider>, ServicesSlider>();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();


var services = builder.Services.BuildServiceProvider();
try
{
	var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
	var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
	await DefaultRole.SeedAsync(roleManager);
	await DefaultUser.SeedSuperAdminAsync(roleManager, userManager);
	await DefaultUser.SeedBasicAsync(roleManager, userManager);
}
catch (Exception)
{

	throw;
}
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


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
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["SecretKey"];

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Accounts}/{action=Login}/{id?}"
    );

app.MapControllerRoute(
	name: "default",
	pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
