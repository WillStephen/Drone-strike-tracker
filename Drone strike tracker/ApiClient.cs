using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Drone_strike_tracker.Helpers;
using Drone_strike_tracker.Models;
using Newtonsoft.Json;

namespace Drone_strike_tracker
{
    public class ApiClient
    {
        public async Task<StrikeListContainer> GetStrikeListAsync()
        {
            using (var client = new HttpClient())
            using (var s = await client.GetStreamAsync(Settings.ApiUri))
            using (var sr = new StreamReader(s))
            using (var reader = new JsonTextReader(sr))
            {
                var serializer = new JsonSerializer();
                return await Task.Factory.StartNew(() => serializer.Deserialize<StrikeListContainer>(reader));
            }
        }
    }
}