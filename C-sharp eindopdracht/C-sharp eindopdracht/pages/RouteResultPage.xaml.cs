using C_sharp_eindopdracht.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace C_sharp_eindopdracht.pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RouteResultPage : Page
    {
        public RouteResultModel model;

        public RouteResultPage()
        {
            this.InitializeComponent();
            model = new RouteResultModel(this, null);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                model.LocationsData = (LocationPageData)e.Parameter;
            }
            catch (Exception)
            {
                model.LocationsData = new LocationPageData(Soort.from);
            }

            model.Start();

        }

        private void GoHomeButton(object sender, RoutedEventArgs e)
        {

        }

        private void ShowHelpButton(object sender, RoutedEventArgs e)
        {

        }

        private void journeys_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
