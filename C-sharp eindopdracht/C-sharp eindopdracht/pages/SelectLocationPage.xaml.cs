using C_sharp_eindopdracht.Model;
using C_sharp_eindopdracht.pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace C_sharp_eindopdracht
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelectLocationPage : Page
    {
        LocationPageData locationData;
        public SelectLocationModel model = new SelectLocationModel();
            
        public SelectLocationPage()
        {
            this.InitializeComponent();
            model.Start();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                locationData = (LocationPageData)e.Parameter;
            }
            catch (Exception) {
                locationData = new LocationPageData(Soort.from);
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            
            Frame.Navigate(typeof(MainPage), locationData);
        }

        private void locations_ItemClick(object sender, ItemClickEventArgs e)
        {
            Location l = (Location)e.ClickedItem;
            if(locationData.soort == Soort.from)
                locationData.SetFromProperties(l.Name, l.id);
            else
                locationData.SetToProperties(l.Name,l.id);
            Frame.Navigate(typeof(MainPage), locationData);
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            Location l = new Location() { Name = InputField.Text };
            model.Addlocation(l);
        }

        private void InputField_KeyDown(object sender, KeyRoutedEventArgs e)
        {
        }
       

        private void InputField_TextChanged(object sender, TextChangedEventArgs e)
        {

           
        }

        private async void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            model.Addlocation(new Location() { Name = "laden..." });
            await model.NewRequest(InputField.Text);
        }
    }
           
}
