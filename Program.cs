using Scentify.Data;
using Scentify.Repositories;
using Scentify.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ProductoRepository>();
builder.Services.AddScoped<TransaccionPagoRepository>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<PedidoRepository>();
builder.Services.AddScoped<DetallePedidoRepository>();
builder.Services.AddScoped<ResenaRepository>();
builder.Services.AddScoped<CarritoCompraRepository>();
builder.Services.AddScoped<MarcaRepository>();
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<BitacoraTransaccionesRepository>();
builder.Services.AddScoped<ClienteDashboardRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(SesionActivaAttribute));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthorization();
app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
