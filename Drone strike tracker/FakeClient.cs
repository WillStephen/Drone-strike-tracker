using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drone_strike_tracker.Models;

namespace Drone_strike_tracker
{
    public class FakeClient
    {
        public async Task<StrikeListContainer> GetStrikeListAsync()
        {
            var list = new StrikeListContainer
            {
                Strike = new List<Strike>
                {
                    new Strike
                    {
                        Date = new DateTime(2016, 10, 10),
                        Country = "Pakistan",
                        Deaths = "300 million",
                        Narrative = "Russia launched multiple ICBMs toward New York at 3PM PST."
                    },
                    new Strike
                    {
                        Date = new DateTime(2016, 10, 10),
                        Country = "Pakistan",
                        Deaths = "300 million",
                        Narrative = "Russia launched multiple ICBMs toward New York at 3PM PST."
                    },
                    new Strike
                    {
                        Date = new DateTime(2016, 10, 10),
                        Country = "Pakistan",
                        Deaths = "300 million",
                        Narrative = "Russia launched multiple ICBMs toward New York at 3PM PST."
                    }
                }
            };
            return list;
        }
    }
}