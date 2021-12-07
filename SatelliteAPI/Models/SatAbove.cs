using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteAPI.Models
{
    public class SatAbove
    {
        public Info info { get; set; }
        public Above[] above { get; set; }

        public class Info
        {
            public string category { get; set; }
            public int transactionscount { get; set; }
            public int satcount { get; set; }
        }

        public class Above
        {
            public int satid { get; set; }
            public string satname { get; set; }
            public string intDesignator { get; set; }
            public string launchDate { get; set; }
            public float satlat { get; set; }
            public float satlng { get; set; }
            public float satalt { get; set; }
        }
    }
}
