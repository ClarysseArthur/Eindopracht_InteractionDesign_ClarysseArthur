using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SatelliteAPI.Entities;

namespace SatelliteAPI
{
    public static class Favorite
    {
        [FunctionName("GetFavorites")]
        public static async Task<IActionResult> GetFavorites(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/favorite")] HttpRequest req,
            ILogger log)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=stinteractiondesign;AccountKey=Z04MMiQZNqcMaRVm5dtcadog++7Ze6JB0h3shxF2LxOZ20CG0zXST4pUKkS1PuN/HPAV2p02A1l17fzZF3cj8Q==;EndpointSuffix=core.windows.net";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable cloudTable = cloudTableClient.GetTableReference("SatelliteFavorites");

            TableQuery<SatelliteEntity> selectQuery = new TableQuery<SatelliteEntity>();
            var queryResult = await cloudTable.ExecuteQuerySegmentedAsync<SatelliteEntity>(selectQuery, null);

            return new OkObjectResult(queryResult);
        }

        [FunctionName("AddFavorite")]
        public static async Task<IActionResult> AddFavorite(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/favorite/{satId}")] HttpRequest req,
            string satId,
            ILogger log)
        {
            string guid = Guid.NewGuid().ToString();
            SatelliteEntity satelliteEntity = new SatelliteEntity(guid)
            {
                SatelliteId = satId,
                IsShown = true
            };

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=stinteractiondesign;AccountKey=Z04MMiQZNqcMaRVm5dtcadog++7Ze6JB0h3shxF2LxOZ20CG0zXST4pUKkS1PuN/HPAV2p02A1l17fzZF3cj8Q==;EndpointSuffix=core.windows.net";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable cloudTable = cloudTableClient.GetTableReference("SatelliteFavorites");

            TableOperation insertOperation = TableOperation.Insert(satelliteEntity);
            await cloudTable.ExecuteAsync(insertOperation);

            return new OkObjectResult("Gelukt!");
        }
    }
}
