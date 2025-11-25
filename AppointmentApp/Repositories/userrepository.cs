using AppointmentApp.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentApp.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IMongoClient client, Microsoft.Extensions.Options.IOptions<MongoSettings> settings)
        {
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<User>(settings.Value.UsersCollection);
        }

        public async Task<bool> Exists(string email)
        {
            var user = await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
            return user != null;
        }

        public async Task<bool> Register(User user)
        {
            await _collection.InsertOneAsync(user);
            return true;
        }

        public async Task<List<User>> GetAll()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<User?> GetById(string id)
        {
            return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }
    }
}
