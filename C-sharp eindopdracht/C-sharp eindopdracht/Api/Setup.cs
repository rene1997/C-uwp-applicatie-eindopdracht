using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Web.Http;

namespace C_sharp_eindopdracht.Api
{
    public class Setup
    {
        public static async Task<string> request(string requested)
        {
            var cts = new CancellationTokenSource();
                cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();
                Uri uri = new Uri($"https://api.9292.nl/0.1/{requested}");
                var response = await client.GetAsync(uri).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                string StringResponse = await response.Content.ReadAsStringAsync();
                return StringResponse;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static ObservableCollection<Location> deserialiseLocation(string json)
        {
            ObservableCollection<Location> collection = new ObservableCollection<Location>();

            JsonObject locations;
            bool parseOK = JsonObject.TryParse(json, out locations);
            if (!parseOK)
            {
                return collection;
            }

            foreach(string s in locations.Keys)
            {
                JsonObject locationToAdd, latlongToAdd, urlsToAdd;
                try
                {
                    locationToAdd = locations.GetNamedObject("place", null);
                    latlongToAdd = locations.GetNamedObject("latlong", null);
                    urlsToAdd = locations.GetNamedObject("urls", null);

                    string name, type, latitude, longitude, url;
                    name = locationToAdd.GetNamedString("name");
                    type = locations.GetNamedString("type");
                    latitude = latlongToAdd.GetNamedString("lat");
                    longitude = latlongToAdd.GetNamedString("long");
                    url = urlsToAdd.GetNamedString("nl-NL");

                    Location l = new Location();
                    l.Name = name;
                    l.type = type;
                    l.latitude = Double.Parse(latitude);
                    l.longitude = Double.Parse(longitude);
                    l.url = url;

                    collection.Add(l);

                }
                catch
                {
                    return collection;
                }
            }

            return collection;
        }
    }

    public class Location
    {
        public string Name { get; set; }
        public string type { get; set; }
        public double latitude { get; set;}
        public double longitude { get; set; }
        public string url { get; set; }
    }
}
