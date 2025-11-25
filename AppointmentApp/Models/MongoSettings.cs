namespace AppointmentApp.Models
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UsersCollection { get; set; } = "users";
        public string AppointmentsCollection { get; set; } = "appointments";
    }
}
