namespace BookRecomenderApi.Models
{
    public class ServerConfig
    {
        public MongoDbConfig MongoDB { get; set; } = new();
        public RabbitMQConfig RabbitMQ { get; set; } = new();
    }

    public class MongoDbConfig
    {
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string Database { get; set; } = "BookRecommender";
    }

    public class RabbitMQConfig
    {
        public string HostName { get; set; } = "localhost";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
    }
} 