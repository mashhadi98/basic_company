using Company.Application.DependencyInjection;
using Company.Infrastructure.DependencyInjection;
using Company.Infrastructure.Identity;
using Company.Infrastructure.Persistence;
using Company.Presentation.Authorization;
using Company.Presentation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// سرویس‌های لایه Application (use case، ولیدیتور و …)
builder.Services.AddApplication();

// دیتابیس، Identity و مجوزهای داینامیک
builder.Services.AddInfrastructure(builder.Configuration);

// سیاست‌های پویا بر اساس نام مجوز (Permission:Name)
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddAuthorization();

// ارسال ایمیل (توسعه: فقط لاگ)
builder.Services.AddTransient<IEmailSender, LoggingEmailSender>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// اعمال migration و seed اولیه (ادمین + مجوزها)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

await IdentityDataSeeder.SeedAsync(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ترتیب مهم است: احراز هویت قبل از مجوز
app.UseAuthentication();
app.UseAuthorization();

// نگاشت route ناحیه مدیریت
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// مسیر پیش‌فرض
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
