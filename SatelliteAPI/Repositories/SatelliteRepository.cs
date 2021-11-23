using Newtonsoft.Json;
using SatelliteAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteAPI.Repositories
{
    public static class SatelliteRepository
    {
        private static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "appliction/json");
            return client;
        }

        public static async Task<Satellite> GetSatellite(string satCode)
        {
            using (HttpClient client = GetClient())
            {
                try
                {
                    string url = $"http://aviation-edge.com/v2/public/satelliteDetails?key=15f8f4-e023f5&intldes={satCode}";

                    string json = await client.GetStringAsync(url);

                    if (json != null)
                    {
                        return JsonConvert.DeserializeObject<Satellite>(json.Substring(1, json.Length - 3));
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<SatellitesAbove.Rootobject> GetAbove(string lat, string lon)
        {
            using (HttpClient client = GetClient())
            {
                try
                {
                    string url = $"https://api.n2yo.com/rest/v1/satellite/above/{lat}/{lon}/10/20/0&apiKey=WNQB9D-3FHVUW-L6SJKF-4SFS";

                    string json = await client.GetStringAsync(url);

                    if (json != null)
                    {
                        return JsonConvert.DeserializeObject<SatellitesAbove.Rootobject>(json);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


    }
}
