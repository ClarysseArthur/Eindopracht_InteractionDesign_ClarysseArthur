using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteAPI.Models
{
    public class ArcgisFormat
    {
        public string type { get { return "FeatureCollection"; } }
        public MetaData metadata { get; set; }
        public Feature[] features { get; set; }
        public class MetaData
        {
            public int generated { get { return 1637515625; } }
            public string title { get { return "Satellites"; } }
            public int status { get { return 200; } }
        }

        public class Feature
        {
            public string type { get { return "Feature"; } }
            public Properties properties { get; set; }
            public Geometry geometry { get; set; }
            public string id { get; set; }
        }

        public class Properties
        {
            public string type { get { return "Satellite"; } }
            public string country { get; set; }
            public string launchdate { get; set; }
            public string name { get; set; }
            public float lat { get; set; }
            public float lon { get; set; }
        }

        public class Geometry
        {
            public string type { get { return "Point"; } }
            public float[] coordinates { get; set; }
        }
    }
}
