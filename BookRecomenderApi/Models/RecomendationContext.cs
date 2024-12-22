using MongoDB.Driver;

namespace BookRecomenderApi.Models
{
    public class RecomendationContext : IRecomendationContext
    {
        private readonly IMongoDatabase _database;

        public RecomendationContext(MongoDbConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _database = client.GetDatabase(config.Database);
        }

        public IMongoCollection<Recomendation> Recomendations => _database.GetCollection<Recomendation>("Recomendations");
    }
}