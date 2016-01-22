using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sharp_eindopdracht.Model
{
    public class MapPageModel
    {
        public ObservableCollection<Leg> _privateLocations = new ObservableCollection<Leg>();
        public ObservableCollection<Leg> publicLocations { get { return this._privateLocations; }}

        public MapPageModel()
        {

        }

        public void FillList(ObservableCollection<Leg> legs)
        {
            foreach (Leg l in legs)
            {
                AddLeg(l);
            }
        }

        public void AddLeg(Leg leg)
        {
            _privateLocations.Add(leg);
        }


    }
}
