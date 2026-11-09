namespace cliente_imonitor_web.Model.Dtos
{
    public class ClienteDto
    {
        public string? Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;

    }

}
