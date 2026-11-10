namespace cliente_imonitor_web.Model.Dtos
{
    public class ImportacaoResultadoDto
    {
        public int TotalProcessados { get; set; }
        public int TotalSucesso { get; set; }
        public int TotalFalhas { get; set; }
        public List<string> Erros { get; set; } = new();
    }
}
