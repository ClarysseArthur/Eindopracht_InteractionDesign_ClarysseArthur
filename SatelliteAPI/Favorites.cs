using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SatelliteAPI.Repositories;
using SatelliteAPI.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SatelliteAPI.Entities;
using System.Collections.Generic;

namespace SatelliteAPI
{
    public static class Favorites
    {
        [FunctionName("GetFavorites")]
        public static async Task<IActionResult> GetFavorites(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/favorites")] HttpRequest req,
            ILogger log)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=stinteractiondesign;AccountKey=Z04MMiQZNqcMaRVm5dtcadog++7Ze6JB0h3shxF2LxOZ20CG0zXST4pUKkS1PuN/HPAV2p02A1l17fzZF3cj8Q==;EndpointSuffix=core.windows.net";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable cloudTable = cloudTableClient.GetTableReference("SatelliteFavorites");

            TableQuery<SatelliteEntity> selectQuery = new TableQuery<SatelliteEntity>();
            var queryResult = await cloudTable.ExecuteQuerySegmentedAsync<SatelliteEntity>(selectQuery, null);

            ArcgisFormat.Feature[] features = new ArcgisFormat.Feature[queryResult.Results.Count];
            int i = 0;

            foreach (var satellite in queryResult)
            {
                Satellite satData = await SatelliteRepository.GetSatellite(satellite.RowKey);

                ArcgisFormat.Geometry geometry = new ArcgisFormat.Geometry
                {
                    coordinates = new float[] { satData.positions[0].satlatitude, satData.positions[0].satlongitude, 40000}
                };

                ArcgisFormat.Properties properties = new ArcgisFormat.Properties
                {
                    SatID = satellite.RowKey,
                    SatName = satData.info.satname,
                    SatAltitude = satData.positions[0].sataltitude,
                    SatLatitude = satData.positions[0].satlatitude,
                    SatLongitude = satData.positions[0].satlongitude,
                    isfavorite = await SatelliteRepository.IsFavorite(satellite.RowKey),
                    isshown = await SatelliteRepository.isShown(satellite.RowKey)
                };

                ArcgisFormat.Feature feature = new ArcgisFormat.Feature()
                {
                    properties = properties,
                    geometry = geometry,
                    id = i
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/favorites/{satId}")] HttpRequest req,
            string satId,
            ILogger log)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=stinteractiondesign;AccountKey=Z04MMiQZNqcMaRVm5dtcadog++7Ze6JB0h3shxF2LxOZ20CG0zXST4pUKkS1PuN/HPAV2p02A1l17fzZF3cj8Q==;EndpointSuffix=core.windows.net";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable cloudTable = cloudTableClient.GetTableReference("SatelliteFavorites");

            SatelliteEntity satellite = new SatelliteEntity(satId)
            {
                IsShown = true
            };

            TableOperation insert = TableOperation.Insert(satellite);
            await cloudTable.ExecuteAsync(insert);
            
            return new OkObjectResult(satellite);
        }

        [FunctionName("UpdateFavorite")]
        public static async Task<IActionResult> UpdateFavorite(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/favorites/{satId}")] HttpRequest req,
            string satId,
            ILogger log)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=stinteractiondesign;AccountKey=Z04MMiQZNqcMaRVm5dtcadog++7Ze6JB0h3shxF2LxOZ20CG0zXST4pUKkS1PuN/HPAV2p02A1l17fzZF3cj8Q==;EndpointSuffix=core.windows.net";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable cloudTable = cloudTableClient.GetTableReference("SatelliteFavorites");

            TableQuery<SatelliteEntity> selectQuery = new TableQuery<SatelliteEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", "eq", satId));
            var queryResult = await cloudTable.ExecuteQuerySegmentedAsync<SatelliteEntity>(selectQuery, null);

            SatelliteEntity old = queryResult.Results.ToArray()[0];

            if (old.IsShown)
            {
                SatelliteEntity newEntity = old;
                newEntity.IsShown = false;

                TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(newEntity);
                cloudTable.ExecuteAsync(insertOrReplaceOperation);
                return new OkObjectResult(newEntity);
            }
            else
            {
                SatelliteEntity newEntity = old;
                newEntity.IsShown = true;

                TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(newEntity);
                cloudTable.ExecuteAsync(insertOrReplaceOperation);
                return new OkObjectResult(newEntity);
            }
        }

        [FunctionName("DeleteFavorite")]
        public static async Task<IActionResult> DeleteFavorite(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "v1/favorites/{satId}")] HttpRequest req,
            string satId,
            ILogger log)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=stinteractiondesign;AccountKey=Z04MMiQZNqcMaRVm5dtcadog++7Ze6JB0h3shxF2LxOZ20CG0zXST4pUKkS1PuN/HPAV2p02A1l17fzZF3cj8Q==;EndpointSuffix=core.windows.net";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable cloudTable = cloudTableClient.GetTableReference("SatelliteFavorites");

            TableQuery<SatelliteEntity> selectQuery = new TableQuery<SatelliteEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", "eq", satId));
            var queryResult = await cloudTable.ExecuteQuerySegmentedAsync<SatelliteEntity>(selectQuery, null);

            SatelliteEntity old = queryResult.Results.ToArray()[0];

            TableOperation delete = TableOperation.Delete(old);
            await cloudTable.ExecuteAsync(delete);

            return new OkObjectResult("200 ok");
        }

        [FunctionName("GetAbove")]
        public static async Task<IActionResult> GetAbove(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/above")] HttpRequest req,
            ILogger log)
        {
            int i = 0;

            SatAbove all = await SatelliteRepository.GetAbove();

            ArcgisFormat arcgisFormat = new ArcgisFormat()
            {
                features = new ArcgisFormat.Feature[all.above.Length],
                metadata = new ArcgisFormat.MetaData()
            };
            

            foreach (var satellite in all.above)
            {
                ArcgisFormat.Feature feature = new ArcgisFormat.Feature();

                ArcgisFormat.Properties properties = new ArcgisFormat.Properties()
                {
                    SatID = satellite.satid.ToString(),
                    SatName = satellite.satname,
                    SatAltitude = satellite.satalt,
                    SatLatitude = satellite.satalt,
                    SatLongitude = satellite.satlng,
                    isfavorite = false,
                    isshown = true,
                };

                ArcgisFormat.Geometry geometry = new ArcgisFormat.Geometry()
                {
                    coordinates = new float[] { satellite.satlat, satellite.satlng, 400000 }
                };

                feature.properties = properties;
                feature.geometry = geometry;
                feature.id = i;

                arcgisFormat.features[i] = feature;

                i++;
            }

            return new OkObjectResult(arcgisFormat);
        }
    }
}
