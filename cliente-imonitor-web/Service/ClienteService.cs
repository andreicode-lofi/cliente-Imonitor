using cliente_imonitor_web.Model.Dtos;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

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

    public async Task<ImportacaoResultadoDto?> ImportarCsvAsync(IBrowserFile arquivo)
    {
        try
        {
            using var content = new MultipartFormDataContent();

            // Passa a cota de 5MB na abertura da stream do arquivo
            using var stream = arquivo.OpenReadStream(maxAllowedSize: 1024 * 1024 * 5);
            using var streamContent = new StreamContent(stream);

            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(arquivo.ContentType);
            content.Add(streamContent, "arquivo", arquivo.Name);

            var response = await _http.PostAsync("api/clientes/importar", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ImportacaoResultadoDto>();
            }

            return new ImportacaoResultadoDto
            {
                TotalFalhas = 1,
                Erros = new List<string> { $"Erro na API HTTP (Status {(int)response.StatusCode}): {response.ReasonPhrase}" }
            };
        }
        catch (Exception ex)
        {
            return new ImportacaoResultadoDto
            {
                TotalFalhas = 1,
                Erros = new List<string> { $"Erro na requisição HTTP: {ex.Message}" }
            };
        }
    }
}