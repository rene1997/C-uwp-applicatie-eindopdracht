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
        public async static Task<string> RequestJourneys(string fromId, string toId, string datetime)
        {
            //string url = $"journeys? before = 1 & sequence = 1 & byFerry = true & bySubway = true & byBus = true & byTram = true & byTrain = true & lang = nl - NL & from = {fromId} & dateTime = {today.Year} - {today.Month} - {today.Day}T{today.Hour}{DateTime.Today.Minute} & searchType = departure & interchangeTime = standard & after = 5 & to = {toId}";
            string url = $"journeys?before=1&sequence=1&byFerry=true&bySubway=true&byBus=true&byTram=true&byTrain=true&lang=nl-NL&from={fromId}&dateTime={datetime}&searchType=departure&interchangeTime=standard&after=5&to={toId}";
            return await request(url);
        }

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
            ObservableCollection<Journey> journeyCollection = new ObservableCollection<Journey>();
            JsonObject jsonObject;
            bool parseOk = JsonObject.TryParse(json, out jsonObject);
            if (!parseOk)
                return null;
            //get jsonvalues
            IJsonValue value = jsonObject.Values.FirstOrDefault();
            //array of journeys in json format
            JsonArray journeys = value.GetArray();
            //for each journey:
            foreach (IJsonValue journeyJson in journeys)
            {
                Journey journey = new Journey();
                JsonObject itemObj = journeyJson.GetObject();
                IJsonValue departureValue;
                IJsonValue arrivalValue;
                //try get arrival and departure time from highest level in json
                try {
                    itemObj.TryGetValue("departure", out departureValue);
                    journey.SetStartTime(departureValue.GetString());
                }catch { }
                try
                {
                    itemObj.TryGetValue("arrival", out arrivalValue);
                    journey.SetEndTime(arrivalValue.GetString());
                }catch { }

                //try to get amount of changes
                IJsonValue changesValues;
                try
                {
                    itemObj.TryGetValue("numberOfChanges", out changesValues);
                    journey.NumberOfChanges = (int)changesValues.GetNumber();
                }catch { }

                //try to get legs from the route
                //get value legs:
                IJsonValue legsValue ;
                itemObj.TryGetValue("legs", out legsValue);
                JsonArray legsObjects = legsValue.GetArray();


                //for each leg in the route
                try {
                    foreach (IJsonValue leg in legsObjects)
                    {
                        Leg legObject = new Leg();
                        JsonObject legItemObj = leg.GetObject();
                        //get destination of the leg ("Maastricht")
                        IJsonValue legDestinationValue;
                        try
                        {
                            legItemObj.TryGetValue("destination", out legDestinationValue);
                            legObject.destination = legItemObj.GetString();
                        } catch { }

                        //get name and type of leg
                        IJsonValue typemodeValue;
                        legItemObj.TryGetValue("mode", out typemodeValue);
                        JsonObject typemodeObject = typemodeValue.GetObject();
                        //try get type ("train")
                        IJsonValue modetype;
                        //try get name ("intercity")
                        IJsonValue modename;
                        try
                        {
                            typemodeObject.TryGetValue("name", out modename);
                            typemodeObject.TryGetValue("type", out modetype);

                            legObject.name = modename.GetString();
                            legObject.type = modetype.GetString();
                        } catch { }

                        //get operator of leg
                        IJsonValue operatorlegvalue;
                        try
                        {
                            legItemObj.TryGetValue("operator", out typemodeValue);
                            JsonObject operatorlegObject = typemodeValue.GetObject();
                            //try get operator name ("NS")
                            IJsonValue legOperatorName;
                            typemodeObject.TryGetValue("name", out legOperatorName);
                            legObject.operatorName = legOperatorName.GetString();
                        } catch { }

                        //try to get stops in the leg 
                        //get value stops:
                        IJsonValue stopsValue;
                        itemObj.TryGetValue("stops", out stopsValue);

                        //get amount of stops
                        JsonArray stopsObjects = legsValue.GetArray();
                        try
                        {
                            int amountOfStops = stopsObjects.Count + 1;
                            legObject.stops = amountOfStops;
                        } catch { }
                        journey.AddLeg(legObject);
                    }
                    journeyCollection.Add(journey);
                }catch { }
            }
            return journeyCollection;
        }
    }
}
