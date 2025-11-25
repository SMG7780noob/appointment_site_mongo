using AppointmentApp.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentApp.Repositories
{
    public interface IAppointmentRepository
    {
        Task<bool> Create(Appointment appointment);
        Task<List<Appointment>> GetAll();
        Task<Appointment?> GetById(string id);
    }

    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IMongoCollection<Appointment> _collection;

        public AppointmentRepository(IMongoClient client, Microsoft.Extensions.Options.IOptions<MongoSettings> settings)
        {
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<Appointment>(settings.Value.AppointmentsCollection);
        }

        public async Task<bool> Create(Appointment appointment)
        {
            await _collection.InsertOneAsync(appointment);
            return true;
        }

        public async Task<List<Appointment>> GetAll()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Appointment?> GetById(string id)
        {
            return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
        }
    }
}
