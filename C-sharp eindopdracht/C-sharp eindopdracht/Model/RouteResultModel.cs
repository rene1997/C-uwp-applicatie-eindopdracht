﻿using C_sharp_eindopdracht.pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace C_sharp_eindopdracht.Model
{
    public class RouteResultModel
    {
        private ObservableCollection<Journey> _privateJourneys = new ObservableCollection<Journey>();
        public ObservableCollection<Journey> publicJourneys { get { return this._privateJourneys; } }
        public RouteResultPage page { get; set; }
        public LocationPageData locationsData;

        public RouteResultModel(RouteResultPage page, LocationPageData locations)
        {
            this.page = page;
            this.locationsData = locations;
        }

        public void SetLocations(List<Journey> journeys)
        {
            _privateJourneys.Clear();
            foreach (Journey j in journeys)
            {
                AddJourney(j);
            }
        }

        public void AddJourney(Journey journey)
        {
            _privateJourneys.Add(journey);
        }

        public async void Start()
        {
            try
            {
                _privateJourneys.Clear();
                AddJourney(new Journey() { StartTime = "aan het laden..." });
                string s = await GetJourneys(locationsData.fromLocation.id, locationsData.toLocation.id, locationsData.datetime);
                _privateJourneys.Clear();
                ObservableCollection<Journey> journeys = Api.Setup.DesJourney(s);
                foreach (Journey j in journeys)
                {
                    AddJourney(j);
                }
            }
            catch
            {
                page.toMainPage();
            }
        }

        public async Task<string> GetJourneys(string fromId, string toId, string datetime)
        {
            string c = "current position";
            if (fromId.Equals(c) || toId.Equals(c))
            {
                // Get my current location.
                Geolocator myGeolocator = new Geolocator();
                Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
                string lat = myGeoposition.Coordinate.Point.Position.Latitude.ToString();
                lat.Replace(',', '.');
                Location l = await Api.Setup.RequestLocationFromCoordinate($"{myGeoposition.Coordinate.Point.Position.Latitude.ToString().Replace(',', '.')},{myGeoposition.Coordinate.Point.Position.Longitude.ToString().Replace(',', '.')}");
                if (fromId.Equals(c))
                {
                    fromId = l.id;
                }
                if (toId.Equals(c))
                {
                    toId = l.id;
                }
            }
            string answer;
            answer = await Api.Setup.RequestJourneys(fromId, toId, datetime, locationsData.isDeparture);
            if (answer.Equals(String.Empty))
            {
                page.toMainPage();
            }
            //Debug.WriteLine(answer);
            return answer;
        }
    }
}

