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
        public string fromName { get; set; }
        public string fromId { get; set; }
        public string toName { get; set; }
        public string toId{ get; set; }

        public LocationPageData(Soort soort)
        {
            this.soort = soort;
        }

        public void SetFromProperties(string name, string id)
        {
            fromName = name;
            fromId = id;
        }

        public void SetToProperties(string name, string id)
        {
            toName = name;
            toId = id;
        }
    }
}
