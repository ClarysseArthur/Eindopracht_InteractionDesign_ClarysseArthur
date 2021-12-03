using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteAPI.Models
{
    public class SatelliteName
    {
        public class Rootobject
        {
            public Satellite[] Satellites { get; set; }
        }

        public class Satellite
        {
            public string Name { get; set; }
        }
    }
}
