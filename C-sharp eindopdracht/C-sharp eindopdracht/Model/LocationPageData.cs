using C_sharp_eindopdracht.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sharp_eindopdracht.pages
{
    public enum Soort { from, to}
    public class LocationPageData
    {
        
        public Soort soort { get; set; }
        public Location fromLocation{ get; set; }
        public Location toLocation { get; set; }

        public LocationPageData(Soort soort)
        {
            this.soort = soort;
        }

        public void SetFromProperties(Location location)
        {
            fromLocation = location;
        }

        public void SetToProperties(Location location)
        {
            toLocation = location;
        }
    }
}
