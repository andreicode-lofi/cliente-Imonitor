using cliente_imonitor.Service;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os controllers e a documentação
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 1. Registra o ClienteService no container de DI
builder.Services.AddSingleton<ClienteService>();

// 2. Libera o CORS para o Blazor
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 3. Aplica o CORS antes dos controllers
app.UseCors("AllowBlazor");

app.UseAuthorization();
app.MapControllers();

app.Run();