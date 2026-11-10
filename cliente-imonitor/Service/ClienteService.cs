using cliente_imonitor.Model;
using cliente_imonitor.Model.Dtos;
using CsvHelper;
using MongoDB.Driver;
using System.Globalization;
using System.Text;

namespace cliente_imonitor.Service
{
    public class ClienteService
    {
        private readonly MongoDB.Driver.IMongoCollection<Cliente> _clientesCollection;

        public ClienteService(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("MongoDb")
                ?? Environment.GetEnvironmentVariable("MONGODB_URI");
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("cliente-imonitor");
            _clientesCollection = database.GetCollection<Cliente>("Clientes");
        }

        public async Task<List<Cliente>> GetAsync(string? nomeFilter = null)
        {
            if (string.IsNullOrWhiteSpace(nomeFilter))
                return await _clientesCollection.Find(_ => true).ToListAsync();

            return await _clientesCollection
                .Find(c => c.Nome.ToLower().Contains(nomeFilter.ToLower()))
                .ToListAsync();
        }

        public async Task<Cliente?> GetByIdAsync(string id) =>
            await _clientesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Cliente newCliente) =>
            await _clientesCollection.InsertOneAsync(newCliente);

        public async Task UpdateAsync(string id, Cliente updatedCliente) =>
            await _clientesCollection.ReplaceOneAsync(x => x.Id == id, updatedCliente);


        public async Task<ImportacaoResultadoDto> ImportarClientesCsvAsync(Stream streamArquivo)
        {
            var resultado = new ImportacaoResultadoDto();

            using var reader = new StreamReader(streamArquivo, Encoding.UTF8);

            // Lê a primeira linha (cabeçalho) e descarta
            var cabecalho = await reader.ReadLineAsync();
            if (cabecalho == null) return resultado;

            int numeroLinha = 1;

            while (!reader.EndOfStream)
            {
                var linha = await reader.ReadLineAsync();
                numeroLinha++;

                if (string.IsNullOrWhiteSpace(linha)) continue;

                // Separa por vírgula (se o CSV da área comercial usar ponto e vírgula, troque para ';')
                var colunas = linha.Split(',');

                if (colunas.Length < 2)
                {
                    resultado.TotalFalhas++;
                    resultado.Erros.Add($"Linha {numeroLinha}: Colunas insuficientes.");
                    continue;
                }

                var nome = colunas[0].Trim().Trim('"');
                var telefone = colunas.Length > 1 ? colunas[1].Trim().Trim('"') : "";
                var email = colunas.Length > 2 ? colunas[2].Trim().Trim('"') : "";
                var endereco = colunas.Length > 3 ? colunas[3].Trim().Trim('"') : "";

                resultado.TotalProcessados++;

                // Validação de negócio resiliente
                if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(telefone))
                {
                    resultado.TotalFalhas++;
                    resultado.Erros.Add($"Linha {numeroLinha}: Cliente '{nome}' ignorado (Nome e Telefone são obrigatórios).");
                    continue;
                }

                var novoCliente = new Cliente
                {
                    Nome = nome,
                    Telefone = telefone,
                    Email = email,
                    Endereco = endereco
                };

                // Chama o método correto do Mongo
                await CreateAsync(novoCliente);
                resultado.TotalSucesso++;
            }

            return resultado;
        }
    }

}
