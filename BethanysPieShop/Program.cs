using BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args); //load -default log- setting from appsetting.json and make sure the kestrel is included and iis
var connectionString = builder.Configuration.GetConnectionString
    ("BethanysPieShopDBContextConnection") ?? throw new InvalidOperationException("Connection" +
    " string 'BethanysPieShopDBContextConnection' not found.");

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();


builder.Services.AddScoped<IShoppingCart,ShoppingCart>(sp => ShoppingCart.GetCart(sp));
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(option =>
    {
        option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); //the app will know about MVC
builder.Services.AddRazorPages();
builder.Services.AddDbContext<BethanysPieShopDBContext>(option =>
{
    option.UseSqlServer(connectionString);
});

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<BethanysPieShopDBContext>();

builder.Services.AddServerSideBlazor();

var app = builder.Build();

//app.MapGet("/", () => "Hello World!"); //request and response with "Hello World!"
app.UseStaticFiles(); // this Middleware Component it looks for incomming request for static file such as jpeg or css and wwwroot
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); //An Exeption page
}

app.MapDefaultControllerRoute();
/*to navigate our pages and handle incomming request and endpoint middleware 
  add support for routting and controller actions,it use a default controller route which is
 "{controller=Home}/{action=Index}/{id}"*/

/*app.MapControllerRoute(
    name : "default",
    pattern : "{controller=Home}/{action=Index}/{id}");
*/
app.MapRazorPages();

app.MapBlazorHub();
app.MapFallbackToPage("/app/{*catchall}", "/App/Index");

DbInitializer.Seed(app);
app.Run();
