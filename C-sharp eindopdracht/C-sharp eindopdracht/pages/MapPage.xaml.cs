using C_sharp_eindopdracht.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private ObservableCollection<Leg> list { get; set; }

        public MapPage()
        {
            this.InitializeComponent();
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

            }
        }

        private void Setpushpin()
        {

        }
    }
}
