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
// Registrar o reposit�rio (ServicoRepositorio)
builder.Services.AddScoped<ServicoRepositorio>();  // Ou AddTransient ou AddSingleton dependendo do caso
// Registrar o reposit�rio (AgendamentoRepositorio)
builder.Services.AddScoped<AtendimentoRepositorio>();  // Ou AddTransient ou AddSingleton dependendo do caso

// Adicionar suporte a sess�es
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Defina o tempo de expira��o da sess�o
    options.Cookie.HttpOnly = true; // Torna o cookie acess�vel apenas via HTTP
});

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

// Adicionar o middleware de sess�o
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
