using System.Net.Http.Json;
using cliente_imonitor_web.Model.Dtos;

namespace cliente_imonitor_web.Services;

public class ClienteService
{
    private readonly HttpClient _http;

    public ClienteService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ClienteDto>> ObterClientesAsync(string? termoBusca = null)
    {
        var endpoint = string.IsNullOrWhiteSpace(termoBusca)
            ? "api/clientes"
            : $"api/clientes?busca={Uri.EscapeDataString(termoBusca)}";

        return await _http.GetFromJsonAsync<List<ClienteDto>>(endpoint) ?? new List<ClienteDto>();
    }

    public async Task CriarClienteAsync(ClienteDto cliente)
    {
        await _http.PostAsJsonAsync("api/clientes", cliente);
    }

    public async Task AtualizarClienteAsync(string id, ClienteDto cliente)
    {
        await _http.PutAsJsonAsync($"api/clientes/{id}", cliente);
    }
}