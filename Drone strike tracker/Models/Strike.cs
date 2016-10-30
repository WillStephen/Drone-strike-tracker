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
        public string Town { get; set; }
        public string Location { get; set; }
        public string Deaths { get; set; }
        public string Deaths_min { get; set; }
        public string Deaths_max { get; set; }
        public string Civilians { get; set; }
        public string Tweet_Id { get; set; }
        public string Bureau_Id { get; set; }
        public string Bij_Summary_Short { get; set; }
        public string Bik_Link { get; set; }
        public string Target { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public string[] Names { get; set; }
    }
}