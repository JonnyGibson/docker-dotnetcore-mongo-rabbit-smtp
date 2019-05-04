namespace BookRecomenderApi.Models
{
    using BookRecomenderApi;
    using MongoDB.Driver;
    using System;
    public class RecomendationContext: IRecomendationContext
    {
        private readonly IMongoDatabase _db;
        public RecomendationContext(MongoDBConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);
        }
        public IMongoCollection<Recomendation> Recomendations => _db.GetCollection<Recomendation>("books");
    }
}