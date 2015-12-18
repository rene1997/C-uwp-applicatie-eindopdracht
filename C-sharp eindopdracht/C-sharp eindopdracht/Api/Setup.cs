﻿using C_sharp_eindopdracht.Model;
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
    public static class Setup
    {
        public async static Task<String> RequestLocations(string searchQuery)
        {
            string url = $"locations?lang=nl-NL&q={searchQuery}";
            return await request(url);
        }

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

        public async static Task<ObservableCollection<Location>> deserialiseLocation(string json)
        {
            ObservableCollection<Location> collection = new ObservableCollection<Location>();

            JsonObject jsonObject;
            bool parseOK = JsonObject.TryParse(json, out jsonObject);
            if (!parseOK)
            {
                return collection;
            }

            IJsonValue value = jsonObject.Values.FirstOrDefault();
            JsonArray locations = value.GetArray();

            foreach (IJsonValue location in locations)
            {
                JsonObject itemObj = location.GetObject();
                IJsonValue nameValue;
                IJsonValue typeValue;
                IJsonValue idValue;
                itemObj.TryGetValue("name", out nameValue);
                itemObj.TryGetValue("type", out typeValue);
                itemObj.TryGetValue("id", out idValue);

                IJsonValue latLongValue;
                itemObj.TryGetValue("latLong", out latLongValue);
                JsonObject latLongObj = latLongValue.GetObject();
                IJsonValue latVal;
                IJsonValue longVal;
                latLongObj.TryGetValue("lat", out latVal);
                latLongObj.TryGetValue("long", out longVal);

                IJsonValue urlValue;
                IJsonValue NLUrl = null;
                try {
                    itemObj.TryGetValue("urls", out urlValue);
                    JsonObject urlObject = urlValue.GetObject();
                    urlObject.TryGetValue("nl-NL", out NLUrl);
                }
                catch { }

                Location l = new Location();
                l.Name = nameValue.GetString();
                l.id = idValue.GetString();
                l.type = typeValue.GetString();
                l.latitude = latVal.GetNumber();
                l.longitude = longVal.GetNumber();
               

                try
                {
                    l.url = NLUrl.GetString();
                }
                catch { }
                

                collection.Add(l);
            }
            return collection;
        }

        public static ObservableCollection<Journey> DesJourney(string json)
        {
            return null;
        }
    }
}
