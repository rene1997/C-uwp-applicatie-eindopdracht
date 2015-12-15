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
        Location location;
            
        public SelectLocationPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                location = (Location)e.Parameter;
            }
            catch (Exception) {}


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }


        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if(location.soort == Soort.from)
                location.fromId = InputField.Text;
            else
                location.toId = InputField.Text;
            Frame.Navigate(typeof(MainPage), location);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            
            Frame.Navigate(typeof(MainPage));
        }
    }
}
