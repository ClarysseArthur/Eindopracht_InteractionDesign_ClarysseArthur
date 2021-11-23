using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteAPI.Models
{
    public class Satellite
    {
        public string Country { get; set; }
        public string Launchdate { get; set; }
        public string Name { get; set; }

        public Result result { get; set; }

        public class Result
        {
            public Geography geography { get; set; }
        }

        public class Geography
        {
            public float alt { get; set; }
            public float lat { get; set; }
            public float lon { get; set; }
        }
    }
}
