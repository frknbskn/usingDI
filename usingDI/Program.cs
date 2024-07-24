using usingDI.Models;
using usingDI.Services;
using usingDI.Tenants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProductService, AProductService>();
//Singleton; app boyunca sadece 1 instance, 1 constructor
//Transient; her ihtiya� duyuldu�unda yeni instance istiyorsak, her seferinde constructor �al��acak.
//Scoped; Ayn� http request'te fakat farkl� objelerde ayn� instance'� kullanacaksa..
builder.Services.AddSingleton<ISingleton, Singleton>();
builder.Services.AddTransient<ITransient, Transient>();
builder.Services.AddScoped<IScoped, Scoped>();
builder.Services.AddTransient<GuidService>();


/*MultiTenant bir sistemde m��terinin kulland��� alt yap�ya g�re dependency injection.
 * Test ortam� local sql server
 * Canl�da sunucudaki sql server
 * 1. tenant provider kulland�k. Burada m��terinin tercih etti�i alt yap�lar var(SQLTenant, OracleTenan vs) var.
 * 2. Se�ilen tenant provider'a g�re nesneye karar verdik: Bu
 */
builder.Services.AddScoped<ITenantService, SQLTenant>();
builder.Services.AddScoped<IDatabaseClient>(services =>
{
    var provider = services.GetRequiredService<ITenantService>(); //ilgili servise daha �nce eklenmi� yap�y� getirir.
    //ITenantService'e SQLTenant d�nd�recek.
    var tenant = provider.GetTenantId();
    if (tenant == "SQL Connection")  //�yle d�nd�rm��t�k de�eri Sql Tenant'�n GetTenantId methodunda
    {
        return new SqlClient();
    }
    else if (tenant =="Oracle Connection")
    {
        return new OracleClient();
    }

    throw new ArgumentException($"Herhangi bir tenant servis eklenmemi� ya da tan�nm�yor.");
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
