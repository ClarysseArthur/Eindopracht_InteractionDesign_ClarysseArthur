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
                    id = i.ToString()
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
    }
}