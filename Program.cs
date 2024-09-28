using Microsoft.EntityFrameworkCore;
using VillaApp.Application.Common.Interfaces;
using VillaApp.Infrastructure.Data;
using VillaApp.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add DbContext
//MySql Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DbConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DbConnection"))));
builder.Services.AddScoped<IVillaRepository, VillaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
