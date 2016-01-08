using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace C_sharp_eindopdracht.Model
{
    public class JourneyModel
    {
        private ObservableCollection<Leg> _legs = new ObservableCollection<Leg>();
        public ObservableCollection<Leg> publicLegs { get { return this._legs; } }
        public Journey journey {get;}

        public JourneyModel(Journey journey)
        {
            
            foreach(Leg l in journey.legs)
            {
                AddLeg(l);
            }
        }

        private void AddLeg(Leg leg)
        {
            _legs.Add(leg);
        }
    }
}
