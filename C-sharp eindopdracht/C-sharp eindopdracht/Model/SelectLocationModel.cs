using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sharp_eindopdracht.Model
{
    public class SelectLocationModel
    {
        private ObservableCollection<Location> _privateLocations = new ObservableCollection<Location>();
        public ObservableCollection<Location> publicLocations { get { return this._privateLocations; } }

        public async Task<bool> NewRequest(string q)
        {
            //get data from api
            string answer = await Api.Setup.RequestLocations(q);

            //parse data to objects
            ObservableCollection<Location> locations  = await Api.Setup.deserialiseLocation(answer);
            _privateLocations.Clear();
            foreach(Location l in locations)
            {
                Addlocation(l);
            }
            return true;
        }

        public void Addlocation(Location l)
        {
            _privateLocations.Add(l);
        }

        public async void Requested(string q)
        {
            if (q.Length % 3 == 0)
            {
                await NewRequest(q);
            }
        }

        




    }
}
