
﻿using C_sharp_eindopdracht.pages;
﻿using C_sharp_eindopdracht.Api;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace C_sharp_eindopdracht
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        protected Setup setup;
        pages.Location location = new pages.Location(Soort.from) { fromId = "Selecteer locatie", toId = "Selecteer locatie"};

        public MainPage()
        {
            this.setup = new Setup();
            this.InitializeComponent();
            

        }

        private void fromTextbox_Click(object sender, RoutedEventArgs e)
        {
            location.soort = Soort.from;
            Frame.Navigate(typeof(SelectLocationPage), location);
        }

        private void toTextbox_Click(object sender, RoutedEventArgs e)
        {
            location.soort = Soort.to;
            Frame.Navigate(typeof(SelectLocationPage), location);
        }

        /// <summary>
        /// this method closes the application when the user press the exit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        /// <summary>
        /// This method brings the user to the settting page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarSettingButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settingspage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try {
                    Settings s = (Settings)e.Parameter;
                    return;
            }
            catch {}

            try {
                pages.Location l = (pages.Location)e.Parameter;
                fromTextbox.Content = l.fromId;
                toTextbox.Content = l.toId;
                location = l;
                return;
            }
            catch { }
            
            
        }

        private void AppBarMapButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MapPage));
        }
    }
}
