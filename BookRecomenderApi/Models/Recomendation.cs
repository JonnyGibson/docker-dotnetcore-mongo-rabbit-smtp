using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookRecomenderApi.Models
{
    public class Recomendation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("author")]
        public required string Author { get; set; }

        [BsonElement("country")]
        public required string Country { get; set; }

        [BsonElement("imageLink")]
        public required string ImageLink { get; set; }

        [BsonElement("language")]
        public required string Language { get; set; }

        [BsonElement("link")]
        public required string Link { get; set; }

        [BsonElement("pages")]
        public int Pages { get; set; }

        [BsonElement("title")]
        public required string Title { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }
    }
}