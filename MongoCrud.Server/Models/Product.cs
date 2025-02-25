using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoCrud.Server.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("price")]
        public decimal Price { get; set; }
    }
}
