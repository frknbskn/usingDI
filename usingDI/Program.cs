using usingDI.Models;
using usingDI.Services;
using usingDI.Tenants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProductService, AProductService>();
//Singleton; app boyunca sadece 1 instance, 1 constructor
//Transient; her ihtiyaç duyulduðunda yeni instance istiyorsak, her seferinde constructor çalýþacak.
//Scoped; Ayný http request'te fakat farklý objelerde ayný instance'ý kullanacaksa..
builder.Services.AddSingleton<ISingleton, Singleton>();
builder.Services.AddTransient<ITransient, Transient>();
builder.Services.AddScoped<IScoped, Scoped>();
builder.Services.AddTransient<GuidService>();


/*MultiTenant bir sistemde müþterinin kullandýðý alt yapýya göre dependency injection.
 * Test ortamý local sql server
 * Canlýda sunucudaki sql server
 * 1. tenant provider kullandýk. Burada müþterinin tercih ettiði alt yapýlar var(SQLTenant, OracleTenan vs) var.
 * 2. Seçilen tenant provider'a göre nesneye karar verdik: Bu
 */
builder.Services.AddScoped<ITenantService, SQLTenant>();
builder.Services.AddScoped<IDatabaseClient>(services =>
{
    var provider = services.GetRequiredService<ITenantService>(); //ilgili servise daha önce eklenmiþ yapýyý getirir.
    //ITenantService'e SQLTenant döndürecek.
    var tenant = provider.GetTenantId();
    if (tenant == "SQL Connection")  //öyle döndürmüþtük deðeri Sql Tenant'ýn GetTenantId methodunda
    {
        return new SqlClient();
    }
    else if (tenant =="Oracle Connection")
    {
        return new OracleClient();
    }

    throw new ArgumentException($"Herhangi bir tenant servis eklenmemiþ ya da tanýnmýyor.");
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
