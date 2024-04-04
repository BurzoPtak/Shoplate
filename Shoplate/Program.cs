using Microsoft.EntityFrameworkCore;
using Shoplate.Data;
using Shoplate.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("ShoplateDBContext")));

builder.Services.AddScoped<InterfaceUserRepository,UserRepository>();
builder.Services.AddScoped<InterfaceItemRepository,ItemRepository>();
builder.Services.AddScoped<InterfaceOrderRepository, OrderRepository>();

builder.Services.AddSession();
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

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
