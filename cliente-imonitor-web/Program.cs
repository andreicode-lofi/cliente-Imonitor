using cliente_imonitor_web;
using cliente_imonitor_web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. Aponta para a porta HTTP da API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5146/")
});

// 2. Registra o serviço
builder.Services.AddScoped<ClienteService>();

await builder.Build().RunAsync();