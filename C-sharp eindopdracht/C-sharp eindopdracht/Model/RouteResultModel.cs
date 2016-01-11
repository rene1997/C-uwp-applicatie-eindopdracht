using C_sharp_eindopdracht.pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            foreach(Journey j in journeys)
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
            _privateJourneys.Clear();
            AddJourney(new Journey() { StartTime = "aan het laden..." });
            string s = await GetJourneys(locationsData.fromLocation.id, locationsData.toLocation.id, locationsData.datetime);
            _privateJourneys.Clear();
            ObservableCollection<Journey> journeys = Api.Setup.DesJourney(s);
            foreach(Journey j in journeys)
            {
                AddJourney(j);
            }
        }

        public async Task<string> GetJourneys(string fromId, string toId, string datetime)
        {
            string answer =  await Api.Setup.RequestJourneys(fromId, toId, datetime);
            Debug.WriteLine(answer);
            return answer;
        }
    }
}

