using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using JORDAN_2T.Infrastructure.Data;
using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Utilities;
using JORDAN_2T.ApplicationCore.Models;
using JORDAN_2T.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcMovieContext") ?? throw new InvalidOperationException("Connection string 'MvcMovieContext' not found.")));

builder.Services.AddDefaultIdentity<ApplicationUser>().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MvcMovieContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//builder.Services.AddTransient<IUserOrderRepository,UserOrderRepository>();
builder.Services.AddScoped<DbInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddSingleton<IEmailSender, EmailSender>();

// Add session configuration for builder
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

SeedDatabase();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
        dbInitializer.Initialize();
    }
}
