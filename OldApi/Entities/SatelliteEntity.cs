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

        public SatelliteEntity(string satId, string guid)
        {
            this.PartitionKey = satId;
            this.RowKey = guid;
        }
        public bool IsShown { get; set; }
    }
}
