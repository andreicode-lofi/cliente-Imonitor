using cliente_imonitor.Model;
using cliente_imonitor.Service;
using Microsoft.AspNetCore.Mvc;

namespace cliente_imonitor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ClienteService _clienteService;

    public ClientesController(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Cliente>>> Get([FromQuery] string? busca) =>
        await _clienteService.GetAsync(busca);

    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> GetById(string id)
    {
        var cliente = await _clienteService.GetByIdAsync(id);
        return cliente is null ? NotFound() : cliente;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Cliente cliente)
    {
        await _clienteService.CreateAsync(cliente);
        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, Cliente cliente)
    {
        var existing = await _clienteService.GetByIdAsync(id);
        if (existing is null) return NotFound();

        cliente.Id = id;
        await _clienteService.UpdateAsync(id, cliente);
        return NoContent();
    }

    [HttpPost("importar")]
    public async Task<IActionResult> ImportarCsv(IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
            return BadRequest("Nenhum arquivo enviado.");

        if (!arquivo.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            return BadRequest("O arquivo enviado deve ser do tipo .csv.");

        using var stream = arquivo.OpenReadStream();
        var resultado = await _clienteService.ImportarClientesCsvAsync(stream);

        return Ok(resultado);
    }
}