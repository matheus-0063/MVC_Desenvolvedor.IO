using MFER.App.Configuration;
using MFER.Data.Context;
using Microsoft.EntityFrameworkCore;

var services = WebApplication.CreateBuilder(args);

// Guardando a connection string do arquivo appSettings.json
var connectionString = services.Configuration.GetConnectionString("DefaultConnection");

services.Services.AddIdentityConfiguration(services.Configuration);

// Adicionando a tela de erro de banco de dados (para desenvolvimento)
services.Services.AddDatabaseDeveloperPageExceptionFilter();

// Adicionando o AutoMapper
services.Services.AddAutoMapper(typeof(Program));

services.Services.AddDbContext<MeuDBContext>(options =>
    options.UseSqlServer(connectionString));

// Adicionando o MVC
services.Services.AddControllersWithViews();

services.Services.AddMvcConfiguration();

services.Services.ResolveDependencies();

// Gerando a APP
var app = services.Build();

// Configuração conforme os ambientes
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

app.UseAuthentication();
app.UseAuthorization();

app.UseGlobalizationConfig();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
