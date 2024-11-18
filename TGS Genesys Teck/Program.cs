using Microsoft.EntityFrameworkCore;
using TGS_Genesys_Teck.ORM;
using TGS_Genesys_Teck.Repositorio;
using TGS_Genesys_Teck.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Registrar o DbContext se necess�rio
builder.Services.AddDbContext<TgsGenesysTeckContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar o reposit�rio (UsuarioRepositorio)
builder.Services.AddScoped<UsuarioRepositorio>();  // Ou AddTransient ou AddSingleton dependendo do caso
// Registrar o reposit�rio (UsuarioRepositorio)
builder.Services.AddScoped<ServicoRepositorio>();  // Ou AddTransient ou AddSingleton dependendo do caso

// Registrar outros servi�os, como controllers com views
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
