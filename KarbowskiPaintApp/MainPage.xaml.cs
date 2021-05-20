using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;


namespace PaintApp
{
    public sealed partial class MainPage : Page
    {
        private Point punktPoczątkowy;
        private SolidColorBrush pisak = new SolidColorBrush(Windows.UI.Colors.Black);
        private Boolean czyRysuje = false;
        private Line poprzedniaKreska;
        private Stack<Shape> stosUndo = new Stack<Shape>();
        private Stack<int> stosPunkty = new Stack<int>();
        private int punktyRysowania = 0;
        private SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
        MediaElement mediaElement = new MediaElement();

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            
        }

        private void RysowanieStart(object sender, PointerRoutedEventArgs e)
        {
            czyRysuje = true;
            punktPoczątkowy = e.GetCurrentPoint(poleRysowania).Position;
        }

        private void RysowanieStop(object sender, PointerRoutedEventArgs e)
        {
            czyRysuje = false;
            if (rdbProsta.IsChecked == true)
            {
                stosPunkty.Push(0);
                stosUndo.Push(poprzedniaKreska);
            }
            else
                stosPunkty.Push(punktyRysowania);

            punktyRysowania = 0;
            poprzedniaKreska = null;
        }

        private void Rysowanie(object sender, PointerRoutedEventArgs e)
        {
            if (czyRysuje)
            {
                Point punktAktualny = e.GetCurrentPoint(poleRysowania).Position;
                Line kreska = new Line()
                {
                    Stroke = pisak,
                    StrokeThickness = SldGrubosc.IntermediateValue,
                    X2 = punktAktualny.X,
                    Y2 = punktAktualny.Y,
                    X1 = punktPoczątkowy.X,
                    Y1 = punktPoczątkowy.Y,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };
                poleRysowania.Children.Add(kreska);

                if (rdbDowolna.IsChecked == true)
                {
                    punktPoczątkowy = punktAktualny;
                    punktyRysowania++;
                    stosUndo.Push(kreska);
                }
                else
                {
                    if (poprzedniaKreska != null)
                        poleRysowania.Children.Remove(poprzedniaKreska);
                    poprzedniaKreska = kreska;
                }
            }
        }

        private void KolorCzerwony(object sender, PointerRoutedEventArgs e)
        {
            pisak = new SolidColorBrush(Windows.UI.Colors.Red);
        }

        private void KolorCzarny(object sender, PointerRoutedEventArgs e)
        {
            pisak = new SolidColorBrush(Windows.UI.Colors.Black);
        }

        private void KolorNiebieski(object sender, PointerRoutedEventArgs e)
        {
            pisak = new SolidColorBrush(Windows.UI.Colors.Blue);
        }

        private void KolorZielony(object sender, PointerRoutedEventArgs e)
        {
            pisak = new SolidColorBrush(Windows.UI.Colors.Green);
        }

        private async void BtnKoniec_Click(object sender, RoutedEventArgs e)
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

        private void BtnCofnij_Click(object sender, RoutedEventArgs e)
        {
            if (stosUndo.Count > 0)
            {
                var drawPoints = stosPunkty.Pop();
                if (drawPoints == 0)
                {
                    var undo = stosUndo.Pop();
                    poleRysowania.Children.Remove(undo);
                }
                else
                {
                    for (var i = 0; i < drawPoints; i++)
                    {
                        var undo = stosUndo.Pop();
                        poleRysowania.Children.Remove(undo);
                    }
                }
            }
        }
    }
}
