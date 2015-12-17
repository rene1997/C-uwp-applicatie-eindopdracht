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

        public async void NewRequest(string q)
        {
            //get data from api
            string answer = await Api.Setup.RequestLocations(q);
            
            //parse data to objects
            ///TODO: call correct method from Api.Setup
        }

        public void Addlocation(Location l)
        {
            _privateLocations.Add(l);
        }




    }
}
