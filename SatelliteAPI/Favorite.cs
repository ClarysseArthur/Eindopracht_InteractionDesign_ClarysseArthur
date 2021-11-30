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
using SatelliteAPI.Models;
using SatelliteAPI.Repositories;

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

            //.Where(TableQuery.GenerateFilterConditionForBool("IsShown", "eq", true))
            TableQuery<SatelliteEntity> selectQuery = new TableQuery<SatelliteEntity>();
            var queryResult = await cloudTable.ExecuteQuerySegmentedAsync<SatelliteEntity>(selectQuery, null);

            ArcgisFormat.Feature[] features = new ArcgisFormat.Feature[queryResult.Results.Count];
            int i = 0;

            foreach (var item in queryResult)
            {
                Satellite satellite = await SatelliteRepository.GetSatellite(item.PartitionKey);

                ArcgisFormat.Properties properties = new ArcgisFormat.Properties()
                {
                    country = satellite.Country.ToLower(),
                    launchdate = satellite.Launchdate,
                    name = satellite.Name,
                    lat = satellite.result.geography.lat,
                    lon = satellite.result.geography.lon,
                    isfavorite = true,
                    isshown = item.IsShown,
                    satelliteid = item.PartitionKey
                };

                ArcgisFormat.Geometry geometry = new ArcgisFormat.Geometry()
                {
                    coordinates = new float[] { satellite.result.geography.lat, satellite.result.geography.lon, 408000 }
                };

                ArcgisFormat.Feature feature = new ArcgisFormat.Feature()
                {
                    properties = properties,
                    geometry = geometry,
                    id = $"{i}"
                };

                features[i] = feature;
                i++;
            }

            ArcgisFormat arcgisFormat = new ArcgisFormat()
            {
                features = features,
                metadata = new ArcgisFormat.MetaData()
            };

            return new OkObjectResult(arcgisFormat);
        }

        [FunctionName("GetFavoriteEnebled")]
        public static async Task<IActionResult> GetFavoriteEnebled(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/favoriteenabled")] HttpRequest req,
            ILogger log)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=stinteractiondesign;AccountKey=Z04MMiQZNqcMaRVm5dtcadog++7Ze6JB0h3shxF2LxOZ20CG0zXST4pUKkS1PuN/HPAV2p02A1l17fzZF3cj8Q==;EndpointSuffix=core.windows.net";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable cloudTable = cloudTableClient.GetTableReference("SatelliteFavorites");

            TableQuery<SatelliteEntity> selectQuery = new TableQuery<SatelliteEntity>().Where(TableQuery.GenerateFilterConditionForBool("IsShown", "eq", true));
            var queryResult = await cloudTable.ExecuteQuerySegmentedAsync<SatelliteEntity>(selectQuery, null);

            ArcgisFormat.Feature[] features = new ArcgisFormat.Feature[queryResult.Results.Count];
            int i = 0;

            foreach (var item in queryResult)
            {
                Satellite satellite = await SatelliteRepository.GetSatellite(item.PartitionKey);

                ArcgisFormat.Properties properties = new ArcgisFormat.Properties()
                {
                    country = satellite.Country.ToLower(),
                    launchdate = satellite.Launchdate,
                    name = satellite.Name,
                    lat = satellite.result.geography.lat,
                    lon = satellite.result.geography.lon,
                    isfavorite = true,
                    isshown = item.IsShown,
                    satelliteid = item.PartitionKey
                };

                ArcgisFormat.Geometry geometry = new ArcgisFormat.Geometry()
                {
                    coordinates = new float[] { satellite.result.geography.lat, satellite.result.geography.lon, 408000 }
                };

                ArcgisFormat.Feature feature = new ArcgisFormat.Feature()
                {
                    properties = properties,
                    geometry = geometry,
                    id = $"{i}"
                };

                features[i] = feature;
                i++;
            }

            ArcgisFormat arcgisFormat = new ArcgisFormat()
            {
                features = features,
                metadata = new ArcgisFormat.MetaData()
            };

            return new OkObjectResult(arcgisFormat);
        }

        [FunctionName("AddFavorite")]
        public static async Task<IActionResult> AddFavorite(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/favorite/{satId}")] HttpRequest req,
            string satId,
            ILogger log)
        {
            string guid = Guid.NewGuid().ToString();
            SatelliteEntity satelliteEntity = new SatelliteEntity(satId, guid)
            {
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

        [FunctionName("ChangeEnable")]
        public static async Task<IActionResult> ChangeEnable(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/favorite/{satId}")] HttpRequest req,
            string satId,
            ILogger log)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=stinteractiondesign;AccountKey=Z04MMiQZNqcMaRVm5dtcadog++7Ze6JB0h3shxF2LxOZ20CG0zXST4pUKkS1PuN/HPAV2p02A1l17fzZF3cj8Q==;EndpointSuffix=core.windows.net";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable cloudTable = cloudTableClient.GetTableReference("SatelliteFavorites");

            TableQuery<SatelliteEntity> selectQuery = new TableQuery<SatelliteEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", "eq", satId));
            var queryResult = await cloudTable.ExecuteQuerySegmentedAsync<SatelliteEntity>(selectQuery, null);

            SatelliteEntity old = queryResult.Results.ToArray()[0];

            if (old.IsShown)
            {
                SatelliteEntity newEntity = old;
                newEntity.IsShown = false;

                TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(newEntity);
                cloudTable.ExecuteAsync(insertOrReplaceOperation);
            }
            else
            {
                SatelliteEntity newEntity = old;
                newEntity.IsShown = true;

                TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(newEntity);
                cloudTable.ExecuteAsync(insertOrReplaceOperation);
            }

            return new OkObjectResult("Updatet!");
        }
    }
}
