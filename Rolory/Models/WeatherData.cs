using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rolory.Models
{
    public class WeatherData
    {
       
        public class Coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }
        public string Base { get; set; }
        public class Main
        {
            public float temp { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public float temp_min { get; set; }
            public float temp_max { get; set; }
        }

        public class Wind
        {
            public double speed {get; set;}
            public double deg { get; set; }
        }
        public class Clouds
        {
            //percentage of cloudiness
            public int all { get; set; }
        }
            
            public int Dt { get; set; }
        public class Sys
        {
            public int type { get; set; }
            public int id { get; set; }
            public double message { get; set; }
            public string country { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
        }
            public int id { get; set; }
            public string name { get; set; }
            public int cod { get; set; }
    }
}