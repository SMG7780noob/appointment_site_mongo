using AppointmentApp.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace AppointmentApp.Services
{
    public interface IMongoClientFactory
    {
        IMongoClient CreateClient();
    }

    public class MongoClientFactory : IMongoClientFactory
    {
        private readonly MongoSettings _settings;

        public MongoClientFactory(IOptions<MongoSettings> options)
        {
            _settings = options.Value;
        }

        public IMongoClient CreateClient()
        {
            return new MongoClient(_settings.ConnectionString);
        }
    }
}
