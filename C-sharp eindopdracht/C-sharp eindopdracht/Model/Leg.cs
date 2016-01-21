using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;

namespace C_sharp_eindopdracht.Model
{
    public class Leg
    {
        public string type { get; set; } //lopen = walk  //bus = bus //trein = train
        public string name { get; set; }
        public string destination { get; set; }
        public string operatorName { get; set; }
        public int stops { get; set; }
        public string departureTime { get; set; }
        public string departureLocation { get; set; }
        public string arrivalTime { get; set; }
        public string arrivalLocation { get; set; }
        public BasicGeoposition departurePosition { get; set; }
        public BasicGeoposition arrivalPosition { get; set; }
        public Geofence Fence_a;        //fence voor arrival. departure hoeft niet, anders krijg je duplicaten of overliggende geofences

        public void SetDeparturePosition(double lat, double lon)
        {
            try
            {
                departurePosition = new BasicGeoposition() { Latitude = lat, Longitude = lon };
            }
            catch { }
            
        }

        public void SetArrivalPosition(double lat, double lon)
        {
            try
            {
                arrivalPosition = new BasicGeoposition() { Latitude = lat, Longitude = lon };
            }
            catch { }
        }

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

        public void AddFence()
        {
            Geocircle circle = new Geocircle(arrivalPosition, 25);
            MonitoredGeofenceStates masking = 0;
            masking |= MonitoredGeofenceStates.Entered;
            var geofence1 = new Geofence(destination, circle, masking, true, TimeSpan.FromSeconds(1));

            // Replace if it already exists for this maneuver key
            var oldFence = GeofenceMonitor.Current.Geofences.Where(p => p.Id == destination).FirstOrDefault();
            if (oldFence != null)
            {
                GeofenceMonitor.Current.Geofences.Remove(oldFence);
            }
            GeofenceMonitor.Current.Geofences.Add(geofence1);
            Fence_a = geofence1;
        }
    }
}
