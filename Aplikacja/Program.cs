using Microsoft.EntityFrameworkCore;
using Aplikacja.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AplikacjaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AplikacjaContext") ?? throw new InvalidOperationException("Connection string 'AplikacjaContext' not found.")));

// Dodanie obsługi sesji
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true; // plik cookie niedostępny przez skrypty po stronie klienta
    options.Cookie.IsEssential = true; // śledzenie sesji podczas nawigacji
});


var app = builder.Build();

// Pobranie dostępu do serwisów aplikacji i uruchomienie seedowania danych
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<Aplikacja.Data.AplikacjaContext>();
        Aplikacja.Data.DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Wystąpił błąd podczas inicjalizacji bazy danych.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Uruchomienie sesji w aplikacji
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
