using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Drone_strike_tracker.Models;
using Newtonsoft.Json;

namespace Drone_strike_tracker
{
    public class ApiClient
    {
        public enum RequestType
        {
            Min,
            Full,
            Fake
        }

        public async Task<StrikeListContainer> GetStrikeListAsync()
        {
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 0, 5);
                using (var s = await client.GetStreamAsync(Settings.FullApiUrl))
                using (var sr = new StreamReader(s))
                using (var reader = new JsonTextReader(sr))
                {
                    var serializer = new JsonSerializer();
                    var list = await Task.Factory.StartNew(() => serializer.Deserialize<StrikeListContainer>(reader));
                    list.Strike.Reverse();
                    return list;
                }
            }
        }
    }
}