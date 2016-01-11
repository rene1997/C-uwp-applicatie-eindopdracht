using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sharp_eindopdracht.Model
{
    public class Leg
    {
        public string type { get; set; }
        public string name { get; set; }
        public string destination { get; set; }
        public string operatorName { get; set; }
        public int stops { get; set; }
        public string departureTime { get; set; }
        public string departureLocation { get; set; }
        public string arrivalTime { get; set; }
        public string arrivalLocation { get; set; }

        public void SetDepartureTime(string departureTime)
        {
            try
            {
                this.departureTime = departureTime.Split('T').Last();
            }
            catch { }
        }

        public void SetArrivalTime(string arrivalTime)
        {
            try
            {
                this.arrivalTime = arrivalTime.Split('T').Last();
            }
            catch { }
        }
    }
}
