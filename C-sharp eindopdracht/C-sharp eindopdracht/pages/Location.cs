using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sharp_eindopdracht.pages
{
    public enum Soort { from, to}
    public class Location
    {
        
        public Soort soort { get; set; }
        public string fromId { get; set; }
        public string toId { get; set; }

        public Location(Soort soort)
        {
            this.soort = soort;
        }

        public void SetFromID(string id)
        {
            fromId = id;
        }

        public void SetToID(string id)
        {
            toId = id;
        }
    }
}
