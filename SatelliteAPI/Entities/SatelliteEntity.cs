using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteAPI.Entities
{
    public class SatelliteEntity : TableEntity
    {
        public SatelliteEntity()
        {

        }

        public SatelliteEntity(string guid)
        {
            this.PartitionKey = "Satellite";
            this.RowKey = guid;
        }

        public string SatelliteId { get; set; }
        public bool IsShown { get; set; }
    }
}
