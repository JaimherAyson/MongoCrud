using MongoDB.Driver;

namespace MongoCrud.Server.Data
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}