
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookRecomenderApi.Models
{
    public class Recomendation
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public string author { get; set; }
        public string country { get; set; }
        public string imageLink { get; set; }
        public string language { get; set; }
        public string link { get; set; }
        public int pages { get; set; }
        public string title { get; set; }
        public int year { get; set; }
    }
}