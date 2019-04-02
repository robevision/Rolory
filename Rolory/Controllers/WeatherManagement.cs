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
            List <WeatherData.Weather> weekWeather = root.weather;
            weatherToday = weekWeather[0].description;
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
            weatherInFourDays = weekWeather[4].description;
            return weatherInFourDays;
        }
    }
    }
