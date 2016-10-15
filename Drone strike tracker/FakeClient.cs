using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Drone_strike_tracker.Models;
using Newtonsoft.Json;

namespace Drone_strike_tracker
{
    public class FakeClient
    {
        public async Task<StrikeListContainer> GetStrikeListAsync()
        {
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 0, 5);
                using (var s = await client.GetStreamAsync(Settings.ApiUri))
                using (var sr = new StreamReader(s))
                using (var reader = new JsonTextReader(sr))
                {
                    var serializer = new JsonSerializer();
                    var list = await Task.Factory.StartNew(() => serializer.Deserialize<StrikeListContainer>(reader));
                    list.Strike.Insert(0, new Strike
                    {
                        Date = new DateTime(2016, 10, 10),
                        Country = "United States of America",
                        Deaths = "300 million",
                        Narrative = "Russia launched multiple ICBMs toward New York at 3PM PST."
                    });
                    list.Strike.Insert(0, new Strike
                    {
                        Date = new DateTime(2016, 10, 10),
                        Country = "United States of America",
                        Deaths = "300 million",
                        Narrative = "Russia launched multiple ICBMs toward New York at 3PM PST."
                    });
                    return list;

                }
            }
        }
    }
}