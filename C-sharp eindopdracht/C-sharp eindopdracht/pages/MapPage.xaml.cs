using C_sharp_eindopdracht.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace C_sharp_eindopdracht.pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        private ObservableCollection<Leg> list { get; set; }
        private DispatcherTimer timer = new DispatcherTimer();
        private Geolocator locator;
        private Geoposition position;
        MapIcon user = new MapIcon();

        public MapPage()
        {
            this.InitializeComponent();
            MapView.MapElements.Add(user);

            locator = new Geolocator()
            {
                ReportInterval = 1500
            };
            locator.DesiredAccuracy = PositionAccuracy.High;
            locator.PositionChanged += Locator_PositionChanged;

            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;
        }

        private void Locator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (args.Position != null)
                this.position = args.Position;
        }

        private void Timer_Tick(object sender, object e)
        {
            UpdateMap();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            timer.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            timer.Stop();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void fillList(List<Leg> legs)
        {
            list = new ObservableCollection<Leg>();
            foreach (Leg l in legs)
            {
                list.Add(l);
            }
        }

        private async void UpdateMap()
        {
            if (!list.Any())
            {
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                var route = await Getroute();
                MapRouteView path = new MapRouteView(route.Route);
                path.RouteColor = Colors.AliceBlue;

                MapView.Routes.Clear();
                MapView.Routes.Add(path);
            }
        }

        private void Setpushpin()
        {

        }

        private async Task<MapRouteFinderResult> Getroute()
        {
            try
            {
                List<Geopoint> geolist = new List<Geopoint>();
                geolist.Add(position.Coordinate.Point);

                return await MapRouteFinder.GetWalkingRouteFromWaypointsAsync(geolist);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void RefreshMapLocation()
        {
            if (position != null)
            {
                MapView.Center = position.Coordinate.Point;
                user.Location = position.Coordinate.Point;
                user.NormalizedAnchorPoint = new Point(0.5, 0.5);
            }
        }
    }
}
