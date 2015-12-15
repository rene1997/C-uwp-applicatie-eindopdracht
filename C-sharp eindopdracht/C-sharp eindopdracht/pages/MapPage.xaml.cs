using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace C_sharp_eindopdracht.pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        public MapPage()
        {
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void AppBarButton_Click_Location(object sender, RoutedEventArgs e)
        {
            // Get my current location.
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            
            Geopoint myPoint = new Geopoint(new BasicGeoposition() { Latitude = myGeoposition.Coordinate.Latitude, Longitude = myGeoposition.Coordinate.Longitude });
            //create POI
            MapIcon myPOI = new MapIcon { Location = myPoint, NormalizedAnchorPoint = new Point(0.5, 1.0), Title = "U bevindt zich hier", ZIndex = 0 };
            
            // add to map and center it
            MapView.MapElements.Add(myPOI);
            MapView.Center = myPoint;
            MapView.ZoomLevel = 15;
        }
    }
}
