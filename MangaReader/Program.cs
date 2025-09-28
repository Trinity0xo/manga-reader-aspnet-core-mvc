using MangaReader.Areas.Identity;
using MangaReader.Filters;
using MangaReader.Models;
using MangaReader.Repositories;
using MangaReader.Repositories.EF;
using MangaReader.Services;
using MangaReader.Services.Implementations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add identity
builder.Services.AddIdentity<User, Role>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});

// External Authentication - Google
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        // Load credentials from appsettings.json
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    });


// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GlobalViewBagFilter>();
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClaimsTransformation, CustomClaimsTransformer>();

builder.Services.AddScoped<IMangaRepository, EFMangaRepository>();
builder.Services.AddScoped<IChapterRepository, EFChapterRepository>();
builder.Services.AddScoped<IGenreRepository, EFGenreRepository>();
builder.Services.AddScoped<IPageRepository, EFPageRepository>();
builder.Services.AddScoped<IFollowRepository, EFFollowingRepository>();
builder.Services.AddScoped<IReadingHistoryRepository, EFReadingHistoryRepository>();

builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IChapterService, ChapterService>();
builder.Services.AddScoped<IMangaService, MangaService>();
builder.Services.AddScoped<IAppConfigService, AppConfigService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHomeContentService, HomeContentService>();
builder.Services.AddScoped<IFollowingService, FollowingService>();
builder.Services.AddScoped<IReadingHistoryService, ReadingHistoryService>();
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddScoped<GlobalViewBagFilter>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

string[] blockedIdentityEndpoints =
[
    // Account pages
    "/Identity/Account/ConfirmEmail",
    "/Identity/Account/ConfirmEmailChange",
    "/Identity/Account/Lockout",

    // Manage pages
    "/Identity/Account/Manage/PersonalData",
    "/Identity/Account/Manage/DownloadPersonalData",
    "/Identity/Account/Manage/ExternalLogins",
    "/Identity/Account/Manage/TwoFactorAuthentication",
    "/Identity/Account/Manage/ResetAuthenticator",
    "/Identity/Account/Manage/GenerateRecoveryCodes",
    "/Identity/Account/Manage/EnableAuthenticator",
    "/Identity/Account/Manage/Disable2fa"
];

foreach (var path in blockedIdentityEndpoints)
{
    app.MapGet(path, context =>
    {
        context.Response.StatusCode = 404;
        return Task.CompletedTask;
    });
}

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// create roles
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

    var roles = new Dictionary<string, string>
    {
        { "SuperAdmin", "Siêu quản trị viên" },
        { "Admin", "Quản trị viên" },
        { "User", "Người dùng" }
    };

    foreach (var role in roles)
    {
        string roleName = role.Key;
        string displayName = role.Value;

        var existingRole = await roleManager.FindByNameAsync(roleName);

        if (existingRole == null)
        {
            await roleManager.CreateAsync(new Role
            {
                Name = roleName,
                DisplayName = displayName
            });
            logger.LogInformation("Role created: {RoleName}", roleName);
        }
        else
        {
            if (existingRole.DisplayName != displayName)
            {
                existingRole.DisplayName = displayName;
                existingRole.UpdatedAt = DateTime.Now;
                await roleManager.UpdateAsync(existingRole);
                logger.LogInformation(
                     "Role updated: {RoleName} display name changed to {DisplayName}",
                     roleName, displayName
                 );
            }
            else
            {
                logger.LogInformation("Role already exists, skipping: {RoleName}", roleName);
            }
        }
    }
}

app.Run();
