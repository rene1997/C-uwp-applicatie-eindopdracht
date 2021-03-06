﻿using C_sharp_eindopdracht.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
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
        private MapPageModel model;
        //private ObservableCollection<Leg> list { get; set; }
        private DispatcherTimer timer = new DispatcherTimer();
        private Geolocator locator;
        private Geoposition position;
        MapIcon user = new MapIcon();

        public MapPage()
        {
            this.InitializeComponent();
            MapView.MapElements.Add(user);
            model = new MapPageModel();
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
            RefreshMapLocation();
            UpdateMap();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                LocationPageData l = (LocationPageData)e.Parameter;
                fillList(l.legs);
            }
            catch { }

            GeofenceMonitor.Current.GeofenceStateChanged += Current_GeofenceStateChanged;
            Setpushpin();
            timer.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            GeofenceMonitor.Current.GeofenceStateChanged -= Current_GeofenceStateChanged;
            timer.Stop();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void fillList(ObservableCollection<Leg> legs)
        {
            model.FillList(legs);
        }

        private async void UpdateMap()
        {
            if (!model.publicLocations.Any() || model.publicLocations == null)
            {
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                MapView.Routes.Clear();
                foreach (Leg l in model.publicLocations)
                {
                    try
                    {
                        if (l.type.Equals("walk"))
                        {
                            var route = await Getroute(l);
                            MapRouteView path = new MapRouteView(route.Route);
                            DrawRoute(path.Route);
                        }
                    }
                    catch  { }
                    
                }
            }
        }

        private void Setpushpin()
        {
            MapIcon icon = new MapIcon();
            icon.Location = new Geopoint(model.publicLocations.Last().arrivalPosition);
            icon.NormalizedAnchorPoint = new Point(0.5, 0.5);
            MapView.MapElements.Add(icon);
        }

        private async Task<MapRouteFinderResult> GetrouteWithUser()
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

        private async Task<MapRouteFinderResult> Getroute(Leg l)
        {
            try
            {
                List<Geopoint> geolist = new List<Geopoint>();
                geolist.Add(new Geopoint(l.departurePosition));
                geolist.Add(new Geopoint(l.arrivalPosition));

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

        private void Current_GeofenceStateChanged(GeofenceMonitor sender, object args)
        {
            if (model.publicLocations.Count() > 1)
            {
                model._privateLocations.RemoveAt(0);
            }
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void DrawRoute(MapRoute route)
        {
            //Draw a semi transparent fat green line
            var color = Colors.Blue;
            color.A = 150;
            //MapView.MapElements.Clear();
            var line = new MapPolyline
            {

                StrokeThickness = 10,
                StrokeColor = color,
                StrokeDashed = false,
                ZIndex = 5
            };

            // Route has a Path containing all points to draw the route
            line.Path = new Geopath(route.Path.Positions);

            MapView.MapElements.Add(line);
        }

    }
}
