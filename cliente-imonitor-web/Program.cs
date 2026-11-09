using cliente_imonitor_web;
using cliente_imonitor_web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. Aponta para a URL real com o sufixo gerado pelo Render (-wamt)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://imonitor-api-wamt.onrender.com/")
});

// 2. Registra o serviço do frontend
builder.Services.AddScoped<ClienteService>();

await builder.Build().RunAsync();