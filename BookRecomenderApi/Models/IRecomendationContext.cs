namespace BookRecomenderApi.Models
{
    using MongoDB.Driver;
    public interface IRecomendationContext
    {
        IMongoCollection<Recomendation> Recomendations { get; }
    }
}