using MongoDB.Driver;

namespace BookRecomenderApi.Models
{
    public interface IRecomendationContext
    {
        IMongoCollection<Recomendation> Recomendations { get; }
    }
}