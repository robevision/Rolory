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
        }
    }
