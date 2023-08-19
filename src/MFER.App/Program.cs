using MFER.App.Configuration;
using MFER.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Configuração de App.Services para cada tipo de ambiente (Dev/Prod/ ...)
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

// Guardando a connection string do arquivo appSettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddIdentityConfiguration(builder.Configuration);

// Adicionando a tela de erro de banco de dados (para desenvolvimento)
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Adicionando o AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<MeuDBContext>(options =>
    options.UseSqlServer(connectionString));

// Adicionando o MVC
builder.Services.AddControllersWithViews();

builder.Services.AddMvcConfiguration();

builder.Services.ResolveDependencies();

// Gerando a APP
var app = builder.Build();

// Configuração conforme os ambientes
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/erro/500");
    app.UseStatusCodePagesWithRedirects("/erro/{0}");
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
