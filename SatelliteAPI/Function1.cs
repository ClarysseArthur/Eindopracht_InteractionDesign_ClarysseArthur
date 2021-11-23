using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SatelliteAPI.Models;
using SatelliteAPI.Repositories;
using System.Collections.Generic;

namespace SatelliteAPI
{
    public class Function1
    {
        public SatelliteName.Rootobject satellites { get; set; }

        //? JSON file
        [FunctionName("GetLocationJson")]
        public async Task<IActionResult> GetLocationJson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/satellitelocation/{json}")] HttpRequest req,
            string json,
            ILogger log)
        {
            satellites = JsonConvert.DeserializeObject<SatelliteName.Rootobject>(json);

            ArcgisFormat.Feature[] features = new ArcgisFormat.Feature[12];
            int i = 0;

            foreach (var item in satellites.Satellites)
            {
                Satellite satellite = await SatelliteRepository.GetSatellite(item.Name);

                ArcgisFormat.Properties properties = new ArcgisFormat.Properties()
                {
                    country = satellite.Country,
                    launchdate = satellite.Launchdate,
                    name = satellite.Name,
                    lat = satellite.result.geography.lat,
                    lon = satellite.result.geography.lon
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
    }
}
