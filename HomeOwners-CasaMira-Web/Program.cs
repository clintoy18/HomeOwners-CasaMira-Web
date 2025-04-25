using HomeOwners_CasaMira_Web.Data;
using HomeOwners_CasaMira_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;

})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

  
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    // Get the RoleManager from the DI container
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<Users>>();


    // List of roles to check and create
    var roles = new[] { "Admin", "Homeowner", "Staff" };

    foreach (var role in roles)
    {
        var roleExist = await roleManager.RoleExistsAsync(role);

        if (!roleExist)
        {
            // Create each role if it does not exist
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed Admin user
    var adminEmail = "admin@casamira.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new Users
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "Admin User",
            Address = "123 Admin St",
            DateOfBirth = new DateTime(1990, 1, 1),
            EmailConfirmed = true,
            Role = "Admin" // Set the role manually
        };

        var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");
        if (result.Succeeded)
        {
            // Assign the role manually (if you're storing the role in the Role column)
            adminUser.Role = "Admin";
            await userManager.UpdateAsync(adminUser);
        }
    }
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
