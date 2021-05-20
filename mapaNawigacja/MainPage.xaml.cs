using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace mapaNawigacja
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void powMapa(object sender, RoutedEventArgs e)
        {
            MapaNawigacja.ZoomLevel++;
        }

        private void pomMapa(object sender, RoutedEventArgs e)
        {
            MapaNawigacja.ZoomLevel--;
        }

        private void trybMapy(object sender, RoutedEventArgs e)
        {
            var ab = sender as AppBarButton;
            FontIcon ficon = new FontIcon
            {
                FontFamily = FontFamily.XamlAutoFontFamily
            };
            FontFamily = FontFamily.XamlAutoFontFamily;
            if (MapaNawigacja.Style == MapStyle.Road)
            {
                MapaNawigacja.Style = MapStyle.AerialWithRoads;
                ficon.Glyph = "M";
                ab.Label = "Mapa";
                ab.Icon = ficon;
            }
            else
            {
                MapaNawigacja.Style = MapStyle.Road;
                ficon.Glyph = "S";
                ab.Label = "Satelita";
                ab.Icon = ficon;
            }
        }

        private void koordynaty(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Koordynaty));
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DaneGeograficzne.opisCelu == null)
                return;
            var pkt = new Geopoint(DaneGeograficzne.pktStartowy);
            MapIcon start = new MapIcon()
            {
                Location = pkt,
                Title = "Tu jesteś"
            };
            MapaNawigacja.MapElements.Add(start);
            var pktCelu = new Geopoint(DaneGeograficzne.pktDocelowy);
            MapaNawigacja.MapElements.Add(
                new MapIcon
                {
                    Title = DaneGeograficzne.opisCelu,
                    Location = pktCelu
                });
            Trasa(pkt, pktCelu);
            MapPolyline trasaLotem = new MapPolyline
            {
                StrokeColor = Windows.UI.Colors.Black,
                StrokeThickness = 3,
                StrokeDashed = true,
                Path = new Geopath(new List<BasicGeoposition> { DaneGeograficzne.pktStartowy, DaneGeograficzne.pktDocelowy }),
            };
            MapaNawigacja.MapElements.Add(trasaLotem);
            await MapaNawigacja.TrySetViewAsync(new Geopoint(DaneGeograficzne.pktStartowy), 8);

            var komunikat = new Windows.UI.Popups.MessageDialog(DaneGeograficzne.odleglosc + " km", $"Odległość od Twojej lokalizacji do {DaneGeograficzne.opisCelu} to:");
            await komunikat.ShowAsync();
        }

        async void Trasa(Geopoint start, Geopoint stop)
        {
            var wynik = await MapRouteFinder.GetDrivingRouteAsync(start, stop);
            if (wynik.Status == MapRouteFinderStatus.Success)
            {
                var trasa = wynik.Route;
                var trasaNaMape = new MapRouteView(wynik.Route)
                {
                    RouteColor = Windows.UI.Colors.Blue
                };
                MapaNawigacja.Routes.Add(trasaNaMape);
                await MapaNawigacja.TrySetViewBoundsAsync(trasa.BoundingBox, new Thickness(25), MapAnimationKind.Bow);
            }
            else
            {
                var dlg = new Windows.UI.Popups.MessageDialog(wynik.Status.ToString(), DaneGeograficzne.opisCelu);
                await dlg.ShowAsync();
            }
        }
    }
}
