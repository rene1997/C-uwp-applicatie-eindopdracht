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
    }
}
