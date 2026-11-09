using cliente_imonitor.Model;
using MongoDB.Driver;

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
    }
}
