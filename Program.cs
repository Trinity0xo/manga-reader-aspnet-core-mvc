using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WEBTRUYEN.Models;
using WEBTRUYEN.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<
ApplicationDbContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// add scope ở đây
// ví dụ builder.Services.AddScoped<IProductRepository, EFProductRepository>()
builder.Services.AddScoped<IGenreRepository, EFGenreRepository>();
builder.Services.AddScoped<IComicRepository, EFComicRepository>();
builder.Services.AddScoped<IChapterRepository, EFChapterRepository>();
builder.Services.AddScoped<IPageRepository, EFPageRepository>();
builder.Services.AddScoped<IReadingHistoryRepository, EFReadingHistoryRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// khởi tạo giá trị của role mỗi lận chạy dự án
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "admin", "user" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();
