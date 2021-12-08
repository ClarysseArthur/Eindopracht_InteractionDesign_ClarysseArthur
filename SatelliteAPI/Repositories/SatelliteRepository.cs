using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using SatelliteAPI.Entities;
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

        public static async Task<Satellite> GetSatellite(string satId)
        {
            using (HttpClient client = GetClient())
            {
                try
                {
                    string url = $"https://api.n2yo.com/rest/v1/satellite/positions/{satId}/41.702/-76.014/0/1/&apiKey=WNQB9D-3FHVUW-L6SJKF-4SFS";

                    string json = await client.GetStringAsync(url);

                    if (json != null)
                    {
                        return JsonConvert.DeserializeObject<Satellite>(json);
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

        public static async Task<bool> IsFavorite(string satId)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=stinteractiondesign;AccountKey=Z04MMiQZNqcMaRVm5dtcadog++7Ze6JB0h3shxF2LxOZ20CG0zXST4pUKkS1PuN/HPAV2p02A1l17fzZF3cj8Q==;EndpointSuffix=core.windows.net";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable cloudTable = cloudTableClient.GetTableReference("SatelliteFavorites");

            TableQuery<SatelliteEntity> selectQuery = new TableQuery<SatelliteEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", "eq", satId));
            var queryResult = await cloudTable.ExecuteQuerySegmentedAsync<SatelliteEntity>(selectQuery, null);

            foreach (var item in queryResult)
            {
                if(item != null)
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task<bool> isShown(string satId)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=stinteractiondesign;AccountKey=Z04MMiQZNqcMaRVm5dtcadog++7Ze6JB0h3shxF2LxOZ20CG0zXST4pUKkS1PuN/HPAV2p02A1l17fzZF3cj8Q==;EndpointSuffix=core.windows.net";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable cloudTable = cloudTableClient.GetTableReference("SatelliteFavorites");

            TableQuery<SatelliteEntity> selectQuery = new TableQuery<SatelliteEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", "eq", satId));
            var queryResult = await cloudTable.ExecuteQuerySegmentedAsync<SatelliteEntity>(selectQuery, null);

            foreach (var item in queryResult)
            {
                if (item.IsShown)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public static async Task<SatAbove> GetAbove()
        {
            using (HttpClient client = GetClient())
            {
                try
                {
                    string url = "https://api.n2yo.com/rest/v1/satellite/above/50.973739/3.666197/0/20/0/&apiKey=WNQB9D-3FHVUW-L6SJKF-4SFS";

                    string json = await client.GetStringAsync(url);

                    if (json != null)
                    {
                        return JsonConvert.DeserializeObject<SatAbove>(json);
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
