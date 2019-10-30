using Newtonsoft.Json;
using Rolory.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Rolory.Controllers
{
    public class WeatherManagement
    {
        ApplicationDbContext db;
        Geography geography;
    public WeatherManagement()
        {
            db = new ApplicationDbContext();
            geography = new Geography();
        }
        public double ConvertKelvinToFahrenheit(double temp)
        {
            double convertedTemp = (temp - 273.15) * 9 / 5 + 32;
            return convertedTemp;
        }
        public double ConvertKelvinToCelsius(double temp)
        {
            double convertedTemp = temp - 273.15;
            return convertedTemp;
        }
        public double ConvertCelsiusToFahrenheit(double temp)
        {
            double convertedTemp = (temp * 9 / 5) + 32;
            return convertedTemp;
        }
        public int GetCurrentSeason()
        {
            int season;
            int currentIntMonth = DateTime.Today.Month;
            string currentMonth;
            IEnumerable<string> winter = new List<string>() { "December", "January", "February" };
            IEnumerable<string> summer = new List<string>() { "June", "July", "August" };
            IEnumerable<string> spring = new List<string>() { "March", "April", "May" };
            IEnumerable<string> fall = new List<string>() { "September", "October", "November" };
            switch (currentIntMonth)
            {
                case 1:
                    currentMonth = "January";
                    break;
                case 2:
                    currentMonth = "February";
                    break;
                case 3:
                    currentMonth = "March";
                    break;
                case 4:
                    currentMonth = "April";
                    break;
                case 5:
                    currentMonth = "May";
                    break;
                case 6:
                    currentMonth = "June";
                    break;
                case 7:
                    currentMonth = "July";
                    break;
                case 8:
                    currentMonth = "August";
                    break;
                case 9:
                    currentMonth = "September";
                    break;
                case 10:
                    currentMonth = "October";
                    break;
                case 11:
                    currentMonth = "November";
                    break;
                case 12:
                    currentMonth = "December";
                    break;
                default:
                    currentMonth = "Break";
                    break;
            }

            if(winter.Contains(currentMonth))
            {
                season = 1;
            }
            if (summer.Contains(currentMonth))
            {
                season = 3;
            }
            if (spring.Contains(currentMonth))
            {
                season = 2;
            }
            if (fall.Contains(currentMonth))
            {
                season = 4;
            }
            else
            {
                season = 0;
            }

            return season;
        }
        private Address GetZipCoordinates(Address address)
        {
            try
            {
                string zip = Convert.ToString(address.ZipCode);
                string url = "";
                url = "http://maps.googleapis.com/maps/api/geocode/xml?components=postal_code:" + zip.Trim() + "&sensor=false";

                var result = new System.Net.WebClient().DownloadString(url);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                XmlNodeList parentNode = doc.GetElementsByTagName("location");
                var lat = "";
                var lng = "";
                foreach (XmlNode childrenNode in parentNode)
                {
                    lat = childrenNode.SelectSingleNode("lat").InnerText;
                    lng = childrenNode.SelectSingleNode("lng").InnerText;
                }
                address.Latitude = Convert.ToUInt16(lat);
                address.Longitude = Convert.ToUInt16(lng);
                return address;
            }
            catch
            {
                return null;
            }
        }
  
        public async Task<Address> SetLatLong(Address address)
            {
                // This is the geoDecoderRing 
                try
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(address.StreetAddress.Replace(" ", "+"));
                    stringBuilder.Append(";");
                    stringBuilder.Append(address.Locality.Replace(" ", "+"));
                    stringBuilder.Append(";");
                    stringBuilder.Append(address.Region.Replace(" ", "+"));
                    // example: string url = @"https://maps.googleapis.com/maps/api/geocode/json?address={stringBuilder.ToString()}1600+Amphitheatre+Parkway,+Mountain+View,+CA&key=YOUR_API_KEY";
                    string url = @"https://maps.googleapis.com/maps/api/geocode/json?address=" +
                        stringBuilder.ToString() + "&key=" + Models.Access.geo;

                    // httpclient

                    WebRequest request = WebRequest.Create(url);
                    WebResponse response = await request.GetResponseAsync();
                    System.IO.Stream data = response.GetResponseStream();
                    // tried this System.IO.Stream data = await GetGoogleGeocodeResponse(url);
                    StreamReader reader = new StreamReader(data);
                    // json-formatted string from maps api
                    string responseFromServer = reader.ReadToEnd();
                    response.Close();

                    var root = JsonConvert.DeserializeObject<Geography.AddressAPIData>(responseFromServer);
                    var location = root.results[0].geometry.location;
                    address.Latitude = location.lat;
                    address.Longitude = location.lng;
                    db.Entry(address).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return address;
                }
                catch
                {
                    return null;
                }
            }
        public async Task<string> FindWeatherToday(int? id)
        {
            string weatherToday;
            Address contactAddress = db.Addresses.Where(a => a.Id == id).Select(a => a).SingleOrDefault();
            if(contactAddress.Latitude == null || contactAddress.Longitude == null)
            {
                if(contactAddress.StreetAddress != null && contactAddress.Region != null && contactAddress.Locality != null && contactAddress.ZipCode != null)
                {
                    var newAddress = SetLatLong(contactAddress);
                    if (newAddress.IsCompleted)
                    {
                        db.Entry(newAddress).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
               
                }
                else if(contactAddress.ZipCode != null)
                {
                    var newAddress = GetZipCoordinates(contactAddress);
                    if (newAddress != null)
                    {
                        db.Entry(newAddress).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    return("Not enough information to find coordinates.");
                }
            }
            var latitude = db.Addresses.Where(a => a.Id == id).Select(a => a.Latitude).SingleOrDefault();
            var longitude = db.Addresses.Where(a => a.Id == id).Select(a => a.Longitude).SingleOrDefault();
            StringBuilder stringBuilder = new StringBuilder();
            string url = string.Format("https://api.openweathermap.org/data/2.5/weather?" + "lat=" + latitude + "&lon=" + longitude + "&units=metric" + "&key=" + Models.Access.weath);
            WebRequest request = WebRequest.Create(url);
            WebResponse response = await request.GetResponseAsync();
            System.IO.Stream data = response.GetResponseStream();
            StreamReader reader = new StreamReader(data);
            string responseFromServer = reader.ReadToEnd();
            response.Close();

            var root = JsonConvert.DeserializeObject<WeatherData.RootObject>(responseFromServer);
            List <WeatherData.Weather> weekWeather = root.weather;
            weatherToday = weekWeather[0].main;
            return weatherToday;
        }
        public async Task<string> FindWeatherInFourDays(int? id)
        {
            string weatherInFourDays;
            var latitude = db.Addresses.Where(a => a.Id == id).Select(a => a.Latitude).SingleOrDefault();
            var longitude = db.Addresses.Where(a => a.Id == id).Select(a => a.Longitude).SingleOrDefault();
            StringBuilder stringBuilder = new StringBuilder();
            string url = @"https://api.openweathermap.org/data/2.5/weather?" + "lat=" + latitude + "&lon=" + longitude + "&units=metric" + "&key=" + Models.Access.weath;
            WebRequest request = WebRequest.Create(url);
            WebResponse response = await request.GetResponseAsync();
            System.IO.Stream data = response.GetResponseStream();
            StreamReader reader = new StreamReader(data);
            string responseFromServer = reader.ReadToEnd();
            response.Close();

            var root = JsonConvert.DeserializeObject<WeatherData.RootObject>(responseFromServer);
            List<WeatherData.Weather> weekWeather = root.weather;
            weatherInFourDays = weekWeather[4].main;
            return weatherInFourDays;
        }
        public string[] GetInitialWeatherStream(int? id)
        {
            string descriptor = null;
            var contact = db.Contacts.Where(c => c.Id == id).SingleOrDefault();
            var addressId = contact.AddressId;
            string[] noWeatherData = new string[2] { "", "Add An Address For The Current Temperature In Their Area" };
            Address address = db.Addresses.Where(a => a.Id == addressId).Select(a=>a).SingleOrDefault();

            double latitude = 0;
            double longitude = 0;
            var rawLatitude = db.Addresses.Where(a => a.Id == addressId).Select(a => a.Latitude).SingleOrDefault();
            if(rawLatitude == null)
            {
                //if(address.Region != null && address.Locality != null)
                //{
                //    //locationapi
                //}
                if(address.Region != null)
                {
                    ContactsManagement cm = new ContactsManagement();
                    var indexer = cm.stateList.FindIndex(n=>n.Value.Equals(address.Region));
                    latitude = cm.latLongList[indexer][0];
                    longitude = cm.latLongList[indexer][1];
                    descriptor = "~";
                }
            }
            if(rawLatitude != null)
            {
                latitude = Convert.ToDouble(rawLatitude.Value);
                latitude = Math.Round(latitude);
            }
            var rawLongitude = db.Addresses.Where(a => a.Id == addressId).Select(a => a.Longitude).SingleOrDefault();
            if (rawLongitude != null)
            {
                longitude = Convert.ToDouble(rawLongitude.Value);
                longitude= Math.Round(longitude);
            }
            string url = @"https://api.weather.gov/points/" + latitude + "," + longitude;
            if(latitude != 0 && longitude != 0)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = Access.email;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                WebResponse response = request.GetResponse();
                System.IO.Stream data = response.GetResponseStream();
                StreamReader reader = new StreamReader(data);
                string responseFromServer = reader.ReadToEnd();
                response.Close();

                var root = JsonConvert.DeserializeObject<GovWeatherData.Context>(responseFromServer);
                string observationStations = root.properties.observationStations;
                string secondUrl = root.properties.forecastHourly;
                //int coordOne = root.properties.gridX;
                //int coordTwo = root.properties.gridY;
                return FindCurrentTemperature(descriptor, secondUrl);
            }
            return noWeatherData;

        }
        public string[] FindCurrentTemperature(string descriptor, string forecastHourly)
        {
            //string url = @"https://api.weather.gov/gridpoints/" + "TOP/" + coordOne + "," + coordTwo + "/forecast/hourly/";
            string url = forecastHourly;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = Access.email;
            WebResponse response = request.GetResponse();
           
            System.IO.Stream data = response.GetResponseStream();
            StreamReader reader = new StreamReader(data);
            string responseFromServer = reader.ReadToEnd();
            response.Close();
            var root = JsonConvert.DeserializeObject<GovWeatherData.GetHourlyForecastContext>(responseFromServer);
            string temperatureWithUnit = descriptor + root.properties.periods.Where(p=>p.startTime.Hour == DateTime.Now.Hour).Select(p=>p.temperature).FirstOrDefault() + " " + "°" + root.properties.periods.Where(p => p.startTime.Hour == DateTime.Now.Hour).Select(p => p.temperatureUnit).FirstOrDefault();
            string forecastDescription = root.properties.periods.Where(p => p.startTime.Hour == DateTime.Now.Hour).Select(p => p.shortForecast).FirstOrDefault();
            string[] weatherInfo = new string[2] {temperatureWithUnit, forecastDescription };
            return weatherInfo;
        }
        public string GetWeatherConditions(string weather)
        {
            string condition;
            switch (weather.ToLower())
            {
                case "clouds":
                    condition = "outdoor";
                    break;
                case "clear":
                    condition = "outdoor";
                    break;
                case "tornado":
                    condition = "danger";
                    break;
                case "squall":
                    condition = "indoor";
                    break;
                case "ash":
                    condition = "danger";
                    break;
                case "dust":
                    condition = "danger";
                    break;
                case "fog":
                    condition = "indoor";
                    break;
                case "sand":
                    condition = "indoor";
                    break;
                case "haze":
                    condition = "indoor";
                    break;
                case "smoke":
                    condition = "indoor";
                    break;
                case "mist":
                    condition = "open";
                    break;
                case "snow":
                    condition = "winter-open";
                    break;
                case "rain":
                    condition = "indoor";
                    break;
                case "drizzle":
                    condition = "open";
                    break;
                case "thunderstorm":
                    condition = "indoor";
                    break;
                default:
                    condition = "open";
                    break;
            }
            return condition;  

        }
    }
    }
