using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace KarbowskiKalkulatorWalut
{
    public sealed partial class MainPage : Page
    {
        private SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
        private MediaElement mediaElement = new MediaElement();
        private const string daneNBP = "http://www.nbp.pl/kursy/xml/LastA.xml"; //tu adres pliku z danymi kursowymi
        private List<PozycjaTabeliA> kursyAktualne = new List<PozycjaTabeliA>(); //lista pozycji z danymi dla kolejnych walut
        
        public MainPage()
        {
            this.InitializeComponent();
            txtKwota.PlaceholderText = $"{0:f2}";
            tbPrzeliczona.Text = $"{0:f2}";
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }
        private async void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            var serwerNBP = new HttpClient();
            string dane = "";
            try
            {
                dane = await serwerNBP.GetStringAsync(new Uri(daneNBP));
                kursyAktualne.Clear();
                bool a = ApplicationData.Current.LocalSettings.Values.ContainsKey("lbxNaWalute");
                bool b = ApplicationData.Current.LocalSettings.Values.ContainsKey("lbxZWaluty");
                XDocument daneKursowe = XDocument.Parse(dane);

                kursyAktualne = (from item in daneKursowe.Descendants("pozycja")
                                 select new PozycjaTabeliA()
                                 {
                                     przelicznik = item.Element("przelicznik")?.Value,
                                     kod_waluty = item.Element("kod_waluty")?.Value,
                                     kurs_sredni = item.Element("kurs_sredni")?.Value
                                 }
                    ).ToList();
                kursyAktualne.Insert(0, new PozycjaTabeliA() { kurs_sredni = "1,0000", kod_waluty = "PLN", przelicznik = "1" });
                lbxZWaluty.ItemsSource = kursyAktualne;
                lbxNaWalute.ItemsSource = kursyAktualne;
                tbKodZWaluty.Text = ((PozycjaTabeliA)lbxZWaluty.SelectedItem).kod_waluty.ToString();
                tbKodNaWalute.Text = ((PozycjaTabeliA)lbxNaWalute.SelectedItem).kod_waluty.ToString();

                if (a && b)
                {
                    object value1 = ApplicationData.Current.LocalSettings.Values["lbxZWaluty"];
                    object value2 = ApplicationData.Current.LocalSettings.Values["lbxNaWalute"];
                    lbxNaWalute.SelectedIndex = Int32.Parse(value2.ToString());
                    lbxZWaluty.SelectedIndex = Int32.Parse(value1.ToString());

                }
                else
                {
                    lbxNaWalute.SelectedIndex = 0;
                    lbxZWaluty.SelectedIndex = 0;
                }


            }
            catch (Exception exception)
            {
                await new MessageDialog(exception.Message).ShowAsync();
            }
        }

        private void LbxZWaluty_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConvertCurrency();

            ApplicationData.Current.LocalSettings.Values["lbxZWaluty"] = lbxZWaluty.SelectedIndex;
            ApplicationData.Current.LocalSettings.Values["lbxNaWalute"] = lbxNaWalute.SelectedIndex;
        }

        private void LbxNaWalute_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConvertCurrency();
            ApplicationData.Current.LocalSettings.Values["lbxZWaluty"] = lbxZWaluty.SelectedIndex;
            ApplicationData.Current.LocalSettings.Values["lbxNaWalute"] = lbxNaWalute.SelectedIndex;
        }

        private void TxtKwota_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            double number = 0;
            if (Double.TryParse(txtKwota.Text, out number))
            {
                ConvertCurrency();
            }
            else {
                tbPrzeliczona.Text = "";
            }
        }
        private void ConvertCurrency()
        {
            var wejscIndex = lbxZWaluty.SelectedIndex;
            var wyjscIndex = lbxNaWalute.SelectedIndex;

            if (wyjscIndex < 0) wyjscIndex = 0;
            if (wejscIndex < 0) wyjscIndex = 0;

            var wejscWaluta = kursyAktualne[wejscIndex];
            var wyjscWaluta = kursyAktualne[wyjscIndex];

            var tekstWejsc = txtKwota.Text.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator,".");

            if (double.TryParse(tekstWejsc, NumberStyles.Float, CultureInfo.InvariantCulture, out var kwotaWejsc))
            {
                
                var KursSredniWejsc = wejscWaluta.kurs_sredni.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator, ".");
                var kursSredniWejscDouble = double.Parse(KursSredniWejsc, CultureInfo.InvariantCulture);
                var kwotaPLN = kwotaWejsc * kursSredniWejscDouble;
                var KursSredniWyjsc = wyjscWaluta.kurs_sredni.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator, ".");
                var kursSredniWyjscDouble = double.Parse(KursSredniWyjsc, CultureInfo.InvariantCulture);
                var kwotaDocelowa = kwotaPLN / kursSredniWyjscDouble;
                tbPrzeliczona.Text = kwotaDocelowa.ToString(CultureInfo.CurrentCulture);
                tbKodZWaluty.Text = ((PozycjaTabeliA)lbxZWaluty.SelectedItem).kod_waluty.ToString();
                tbKodNaWalute.Text = ((PozycjaTabeliA)lbxNaWalute.SelectedItem).kod_waluty.ToString();
            }
            else
                tbPrzeliczona.Text = "Brak liczby";
        }
        private void btnPomoc_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Pomoc), ((PozycjaTabeliA)lbxZWaluty.SelectedItem).kod_waluty);
        }

        private void btnOProgramie_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OProgramie));
        }

        private async void btnKoniec_Click(object sender, RoutedEventArgs e)
        {
            string message = "Czy na pewno chcesz wyjść?";
            var stream = await speechSynthesizer.SynthesizeTextToStreamAsync(message);
            var dialog = new MessageDialog(message);
            var yes = new UICommand("Tak", c => Application.Current.Exit());
            var no = new UICommand("Nie");

            dialog.Commands.Add(yes);
            dialog.Commands.Add(no);
            dialog.DefaultCommandIndex = 0;

            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
            await dialog.ShowAsync();
        }
    }
    public class PozycjaTabeliA
    {
        public string przelicznik { get; set; }
        public string kod_waluty { get; set; }
        public string kurs_sredni { get; set; }

        public double liczNaPLN(double kwota, bool czyPLN)
        {
            double kurs;
            var znakDZ = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            double.TryParse(kurs_sredni.Replace(",", znakDZ), out kurs);
            if (czyPLN)
                return kwota *= kurs;
            else
                return kurs != 0 ? kwota / kurs : 0;
        }
    }
}
