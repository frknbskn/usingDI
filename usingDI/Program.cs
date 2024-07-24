using usingDI.Models;
using usingDI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProductService,AProductService>();
//Singleton; app boyunca sadece 1 instance, 1 constructor
//Transient; her ihtiya� duyuldu�unda yeni instance istiyorsak, her seferinde constructor �al��acak.
//Scoped; Ayn� http request'te fakat farkl� objelerde ayn� instance'� kullanacaksa..
builder.Services.AddSingleton<ISingleton,Singleton>();
builder.Services.AddTransient<ITransient,Transient>();
builder.Services.AddScoped<IScoped,Scoped>();
builder.Services.AddTransient<GuidService>();

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
