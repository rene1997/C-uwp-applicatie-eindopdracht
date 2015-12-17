using C_sharp_eindopdracht.Model;
using C_sharp_eindopdracht.pages;
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
            model.Addlocation(new Location() { Name = "is heel mooi" });
            model.Addlocation(new Location() { Name = "is ook heel mooi" });
            model.Addlocation(new Location() { Name = "is even mooi" });
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                locationData = (LocationPageData)e.Parameter;
            }
            catch (Exception) {}


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }


        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if(locationData.soort == Soort.from)
                locationData.fromId = InputField.Text;
            else
                locationData.toId = InputField.Text;
            Frame.Navigate(typeof(MainPage), locationData);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            
            Frame.Navigate(typeof(MainPage));
        }

        private void locations_ItemClick(object sender, ItemClickEventArgs e)
        {
            Location l = (Location)e.ClickedItem;
            if(locationData.soort == Soort.from)
                locationData.fromId = l.Name;
            else
                locationData.toId = l.Name;
            Frame.Navigate(typeof(MainPage), locationData);
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            Location l = new Location() { Name = InputField.Text };

        }
    }
}
