using DataUploader_DadarToTaloja.Data;
using DataUploader_DadarToTaloja.Interfaces;
using DataUploader_DadarToTaloja.Models;
using DataUploader_DadarToTaloja.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register your application services
builder.Services.AddScoped<IUserDetailsExportService, UserDetailsExportService>();
builder.Services.AddSingleton<UploadProgress>();

// Register database connection factories
builder.Services.AddSingleton<LocalDbConnectionFactory>();
builder.Services.AddSingleton<ServerDbConnectionFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default route for MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Export}/{action=UploadDetails}/{id?}");

app.Run();
