using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sharp_eindopdracht.Model
{
    public class Journey
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int NumberOfChanges { get; set; }
        public ObservableCollection<ITussenStop> stops = new ObservableCollection<ITussenStop>();
        public List<Leg> legs { get; set; }

        public Journey()
        {
            legs = new List<Leg>();
        }

        public void AddLeg(Leg leg)
        {
            legs.Add(leg);
        }
    }

    public abstract class ITussenStop
    {
        public string TravelName { get; set; }
        public string VanID { get; set; }
        public string NaarID { get; set; }

    }

    public class BusTussenstop : ITussenStop
    {

    }

    public class SpoorTussenstop : ITussenStop
    {

    }

    public class LoopTussenstop : ITussenStop
    {
        public string duration { get; set; }
    }
}
