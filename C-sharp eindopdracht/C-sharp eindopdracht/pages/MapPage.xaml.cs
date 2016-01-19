using C_sharp_eindopdracht.Model;
using System;
using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace C_sharp_eindopdracht.pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        private ObservableCollection<Leg> list { get; set; } = new ObservableCollection<Leg>();

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
