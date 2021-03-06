﻿using C_sharp_eindopdracht.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string datetime { get; set; }
        public bool isDeparture { get; set; }
        public ObservableCollection<Leg> legs { get; set; }

        public LocationPageData()
        {

        }

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
