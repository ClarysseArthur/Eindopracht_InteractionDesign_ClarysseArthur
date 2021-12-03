using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteAPI.Models
{
    public class Satellite
    {
        public Info info { get; set; }
        public Position[] positions { get; set; }

        public class Info
        {
            public string satname { get; set; }
            public int satid { get; set; }
            public int transactionscount { get; set; }
        }

        public class Position
        {
            public float satlatitude { get; set; }
            public float satlongitude { get; set; }
            public float sataltitude { get; set; }
            public float azimuth { get; set; }
            public float elevation { get; set; }
            public float ra { get; set; }
            public float dec { get; set; }
            public int timestamp { get; set; }
            public bool eclipsed { get; set; }
        }

    }
}
