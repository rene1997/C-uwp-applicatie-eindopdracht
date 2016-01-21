using C_sharp_eindopdracht.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Data.Json;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace C_sharp_eindopdracht.Api
{
    public static class Setup
    {
        public async static Task<string> RequestJourneys(string fromId, string toId, string datetime, bool isVertrek)
        {
            //string url = $"journeys? before = 1 & sequence = 1 & byFerry = true & bySubway = true & byBus = true & byTram = true & byTrain = true & lang = nl - NL & from = {fromId} & dateTime = {today.Year} - {today.Month} - {today.Day}T{today.Hour}{DateTime.Today.Minute} & searchType = departure & interchangeTime = standard & after = 5 & to = {toId}";
            string url = "";
            if (isVertrek)
                url = $"journeys?before=1&sequence=1&byFerry=true&bySubway=true&byBus=true&byTram=true&byTrain=true&lang=nl-NL&from={fromId}&dateTime={datetime}&searchType=departure&interchangeTime=standard&after=5&to={toId}";
            else
                url = $"journeys?before=1&sequence=1&byFerry=true&bySubway=true&byBus=true&byTram=true&byTrain=true&lang=nl-NL&from={fromId}&dateTime={datetime}&searchType=arrival&interchangeTime=standard&after=5&to={toId}";

            return await request(url);
        }

        public async static Task<Location> RequestLocationFromCoordinate(string latlong)
        {
            string url = $"locations?lang=nl-NL&latlong={latlong}";
            string answer = await request(url);
            ObservableCollection<Location> locations = await deserialiseLocation(answer);
            return locations.First();
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

                if (!response.IsSuccessStatusCode || (response.StatusCode == HttpStatusCode.RequestTimeout))
                {
                    var dialog = new Windows.UI.Popups.MessageDialog("");
                    dialog.Content = "Er is iets misgegaan. Dit kan komen doordat er geen verbinding kan worden gemaakt met de 9292 api, of omdat er geen route is tussen de gekozen punten.";
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("Oke") { Id = 0 });
                    await dialog.ShowAsync();
                    
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
                try
                {
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

        /// <summary>
        /// this method deserialized all journeys
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
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
                try
                {
                    journeyCollection.Add(DesOneJourney(journeyJson));
                }catch { }
                
            }
            return journeyCollection;
        }

        /// <summary>
        /// deserialize 1 journey
        /// </summary>
        /// <param name="journeyJson"></param>
        /// <returns></returns>
        private static Journey DesOneJourney(IJsonValue journeyJson)
        {
            Journey journey = new Journey();
            JsonObject itemObj = journeyJson.GetObject();
            IJsonValue departureValue;
            IJsonValue arrivalValue;
            //try get arrival and departure time from highest level in json
            try
            {
                itemObj.TryGetValue("departure", out departureValue);
                journey.SetStartTime(departureValue.GetString());
            }
            catch { }
            try
            {
                itemObj.TryGetValue("arrival", out arrivalValue);
                journey.SetEndTime(arrivalValue.GetString());
            }
            catch { }

            //try to get amount of changes
            IJsonValue changesValues;
            try
            {
                itemObj.TryGetValue("numberOfChanges", out changesValues);
                journey.NumberOfChanges = (int)changesValues.GetNumber();
            }
            catch { }

            //try to get legs from the route 

            //get value legs:
            IJsonValue legsValue;
            itemObj.TryGetValue("legs", out legsValue);
            JsonArray legsObjects = legsValue.GetArray();


            //for each leg in the route
            ///position = journeys>0>legs>0
            foreach (IJsonValue leg in legsObjects)
            {
                try {
                    Leg l = DesOneLeg(leg);
                    journey.AddLeg(l);
                }
                catch (Exception ex)  {
                    ex.GetType();
                }
            }
            return journey;
        }

        /// <summary>
        /// this methods deserialized one leg of a route
        /// </summary>
        /// <param name="leg"></param>
        /// <returns>leg object</returns>
        private static Leg DesOneLeg(IJsonValue leg)
        {
            Leg legObject = new Leg();
            JsonObject legItemObj = leg.GetObject();
            //get destination of the leg ("Maastricht")
            IJsonValue legDestinationValue;
            try
            {
                legItemObj.TryGetValue("destination", out legDestinationValue);
                legObject.destination = legDestinationValue.GetString();
            }
            catch { }

            //get name and type of leg
            ///position = journeys>0>legs>0>mode
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

                if (legObject.type.Contains("walk"))
                {
                    
                    legObject = DesWalkLeg(legItemObj, legObject);

                }

                if (legObject.type.Contains("bus"))
                {
                    IJsonValue lineNumberValue;
                    legItemObj.TryGetValue("service", out lineNumberValue);
                    legObject.name = lineNumberValue.GetString();
                }
            }
            catch { }
            ///position ..


            //get operator of leg
            ///position = journeys>0>legs>0>Operator
            IJsonValue operatorlegvalue;
            try
            {
                legItemObj.TryGetValue("operator", out operatorlegvalue);
                JsonObject operatorlegObject = operatorlegvalue.GetObject();
                //try get operator name ("NS")
                IJsonValue legOperatorName;
                typemodeObject.TryGetValue("name", out legOperatorName);
                legObject.operatorName = legOperatorName.GetString();
            }
            catch { }
            ///position ..

            //try to get stops in the leg 
            //get value stops:
            ///position = journeys>0>legs>0>stops
            ///
            try { 
            IJsonValue stopsValue;
            legItemObj.TryGetValue("stops", out stopsValue);

            //get amount of stops
            JsonArray stopsObjects = stopsValue.GetArray();
                legObject = DesStops(stopsObjects, legObject);
            }
            catch {
                legObject.departureLocation = string.Empty;
                legObject.arrivalLocation = string.Empty;
                legObject.destination = String.Empty;
                legObject.operatorName = "onbekend";
            }
            try
            {
                legObject.AddFence();
            }
            catch { }
            
            return legObject;
        }

        private static Leg DesWalkLeg(JsonObject legItemObj, Leg leg)
        {
            try
            {
                IJsonValue durationValue;
                legItemObj.TryGetValue("duration", out durationValue);
                leg.departureTime = "duur:";
                leg.arrivalTime = durationValue.GetString();

                IJsonValue stopsValue;
                legItemObj.TryGetValue("stops", out stopsValue);
                JsonArray stopsObjects = stopsValue.GetArray();

                //get first stop
                if(stopsObjects.Count > 0)
                {
                    //get position of first stop
                    IJsonValue firstStopValue = stopsObjects.First();
                    JsonObject firstStop = firstStopValue.GetObject();
                    IJsonValue firstlocationValue;
                    firstStop.TryGetValue("location", out firstlocationValue);
                    JsonObject firstlocationObject = firstlocationValue.GetObject();
                    Tuple<double, double> locationPositions = DesPositionFromStop(firstlocationObject);
                    leg.SetDeparturePosition(locationPositions.Item1, locationPositions.Item2);

                    //get position of last stop
                    IJsonValue lastStopValue = stopsObjects.Last();
                    JsonObject lastStop = lastStopValue.GetObject();

                    IJsonValue lastlocationValue;
                    lastStop.TryGetValue("location", out lastlocationValue);
                    JsonObject lastlocationObject = lastlocationValue.GetObject();
                    Tuple<double, double> lastlocationPositions = DesPositionFromStop(lastlocationObject);
                    leg.SetArrivalPosition(lastlocationPositions.Item1, locationPositions.Item2);


                }

                /*
                IJsonValue testValue;
                j.TryGetValue("arrival", out testValue);

                IJsonValue locationValue;
                j.TryGetValue("location", out locationValue);
                JsonObject locationObject = locationValue.GetObject();
                Tuple<double, double> locationPositions = DesPositionFromStop(stopsObjects.First().GetObject());
                if (stopsObjects.First() == j)
                {
                    leg.SetDeparturePosition(locationPositions.Item1, locationPositions.Item2);
                }
                else if (stopsObjects.Last() == j)
                {
                    leg.SetArrivalPosition(locationPositions.Item1, locationPositions.Item2);
                }*/


            }
            catch { }
            
            return leg;
        }

        /// <summary>
        /// desirialize data of stops
        /// </summary>
        /// <param name="stopsObjects"></param>
        /// <param name="legObject"></param>
        /// <returns></returns>
        private static Leg DesStops(JsonArray stopsObjects, Leg legObject)
        {
            int amountOfStops = stopsObjects.Count + 1;
            legObject.stops = amountOfStops;

            if (amountOfStops >= 1)
            {
                ///position = journeys>0>legs>0>stops>0
                IJsonValue firstValue = stopsObjects.First();
                JsonObject firstObject = firstValue.GetObject();
                //try get operator departure time
                IJsonValue firstStoptime;
                firstObject.TryGetValue("departure", out firstStoptime);
                legObject.SetDepartureTime(firstStoptime.GetString());

                //try get departure place of the leg
                ///position = journeys>0>legs>0>stops>0>location
                IJsonValue firstLocationValue;
                firstObject.TryGetValue("location", out firstLocationValue);
                JsonObject firstLocationObject = firstLocationValue.GetObject();

                IJsonValue firstLocationIdValue;
                firstLocationObject.TryGetValue("id", out firstLocationIdValue);
                legObject.departureLocation = firstLocationIdValue.GetString();

                //get coordinates of first stop
                Tuple<double, double> firstCoordinates = DesPositionFromStop(firstLocationObject);
                legObject.SetDeparturePosition(firstCoordinates.Item1, firstCoordinates.Item2);

                ///position = journeys>0>legs>0>stops>last>
                //try get arrival time
                IJsonValue lastValue = stopsObjects.Last();
                JsonObject lastObject = lastValue.GetObject();

                IJsonValue lastStopTime;
                lastObject.TryGetValue("arrival", out lastStopTime);
                legObject.SetArrivalTime(lastStopTime.GetString());

                //try get destination place of the leg
                ///position = journeys>0>legs>0>stops>last>location
                IJsonValue lastLocationValue;
                lastObject.TryGetValue("location", out lastLocationValue);
                JsonObject lastLocationObject = lastLocationValue.GetObject();

                IJsonValue lastLocationIdValue;
                lastLocationObject.TryGetValue("id", out lastLocationIdValue);
                legObject.arrivalLocation = lastLocationIdValue.GetString();

                //get coordinates of latest stop
                Tuple<double,double> lastCoordinates = DesPositionFromStop(lastLocationObject);
                legObject.SetArrivalPosition(lastCoordinates.Item1, lastCoordinates.Item2);
            }
            return legObject;
        }

        private static Tuple<double,double> DesPositionFromStop(JsonObject json)
        {
            //try get latlong
            ///position journeys>0>legs>0>stops>n>location>latLong
            IJsonValue LocationLatLongValue;
            json.TryGetValue("latLong", out LocationLatLongValue);
            JsonObject LocationLatLongObject = LocationLatLongValue.GetObject();

            //try get latitude
            IJsonValue LocationLatValue;
            LocationLatLongObject.TryGetValue("lat", out LocationLatValue);
            //try get longitude
            IJsonValue LocationLongValue;
            LocationLatLongObject.TryGetValue("long", out LocationLongValue);

            Tuple<double, double> tuple = new Tuple<double, double>(LocationLatValue.GetNumber(), LocationLongValue.GetNumber());

            return tuple;
        }
    }
}
