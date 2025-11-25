using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AppointmentApp.Models
{
    public class Appointment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Service { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
    }
}
