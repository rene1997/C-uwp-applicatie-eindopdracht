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
        public LocationPageData LocationsData;

        public RouteResultModel(RouteResultPage page, LocationPageData locations)
        {
            this.page = page;
            this.LocationsData = locations;
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
            string s = await GetJourneys(LocationsData.fromLocation.id, LocationsData.toLocation.id);
            
        }

        public async Task<string> GetJourneys(string fromId, string toId)
        {
            string answer =  await Api.Setup.RequestJourneys(fromId, toId);
            Debug.WriteLine(answer);
            return answer;
        }
    }
}

