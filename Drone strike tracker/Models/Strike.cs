using System;

namespace Drone_strike_tracker.Models
{
    public class Strike
    {
        public string _Id { get; set; }
        public int Number { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public string Narrative { get; set; }
        public string Deaths { get; set; }
        public string Tweet_Id { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
    }
}