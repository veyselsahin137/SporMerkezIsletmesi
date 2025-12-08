using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Data;

var builder = WebApplication.CreateBuilder(args);

// ================== DATABASE / CONTEXT ==================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ================== IDENTITY + ROL AYARLARI ==================
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // Mail onayı zorunlu olmasın, kolay test edin
    options.SignIn.RequireConfirmedAccount = false;

    // Şifre kurallarını "sau" kabul edecek kadar gevşetiyoruz
    options.Password.RequiredLength = 3;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddRoles<IdentityRole>() // Admin / Uye gibi rolleri desteklesin
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ================== ROL + ADMIN KULLANICI SEED (BAŞLANGIÇ VERİSİ) ==================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    var adminRoleName = "Admin";
    var uyeRoleName = "Uye";

    // 1) Admin rolü yoksa oluştur
    if (!await roleManager.RoleExistsAsync(adminRoleName))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRoleName));
    }

    // 2) Üye rolü yoksa oluştur
    if (!await roleManager.RoleExistsAsync(uyeRoleName))
    {
        await roleManager.CreateAsync(new IdentityRole(uyeRoleName));
    }

    // 3) Yardımcı fonksiyon: Verilen öğrenci no için Admin kullanıcısı oluştur
    async Task EnsureAdminUser(string studentNo)
    {
        var email = $"{studentNo}@sakarya.edu.tr";

        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            // Şifre: "sau"
            var createResult = await userManager.CreateAsync(user, "sau");

            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user, adminRoleName);
            }
            else
            {
                // İstersen burada log veya breakpoint ile createResult.Errors'a bakabilirsin
            }
        }
        else
        {
            // Kullanıcı zaten varsa ve Admin rolünde değilse, Admin rolüne ekle
            if (!await userManager.IsInRoleAsync(user, adminRoleName))
            {
                await userManager.AddToRoleAsync(user, adminRoleName);
            }
        }
    }

    // 4) İKİ ADMIN HESABI OLUŞTUR
    await EnsureAdminUser("B221210400"); //  Ben
    await EnsureAdminUser("B211210382"); //  Veysel
}

// ================== HTTP PIPELINE ==================
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Identity için şart
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
