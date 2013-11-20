using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace CuantoSabesDeStarWars
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : CuantoSabesDeStarWars.Common.LayoutAwarePage
    {
        string respValida = "";
        int contador = 0;
        int numRespUsada = 0;
        static int intentos = 3;

        public MainPage()
        {
            this.InitializeComponent();
            SettingsPane.GetForCurrentView().CommandsRequested += SettingCharmManager_CommandsRequested;
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
                DataRequestedEventArgs>(this.ShareTextHandler);
            CargarDatos();
        }

        private void SettingCharmManager_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand("privacypolicy", "Politicas de Privacidad", OpenPrivacyPolicy));
        }

        private async void OpenPrivacyPolicy(IUICommand command)
        {
            Uri uri = new Uri("http://wilsonvargas6.wix.com/privacypolicy");
            await Launcher.LaunchUriAsync(uri);
        }

        #region PrimerBotón
        private void r1_Click(object sender, RoutedEventArgs e)
        {
            if (btnResp1.Content.ToString().Equals(respValida))
            {
                contador++;
                Contador.Text = contador.ToString();
                CargarDatos();
            }
            else
            {
                ComprobarPuntuacion();
            }
        }

        #endregion

        #region SegundoBotón
        private void r2_Click(object sender, RoutedEventArgs e)
        {
            if (btnResp2.Content.ToString().Equals(respValida))
            {
                contador++;
                Contador.Text = contador.ToString();
                CargarDatos();
            }
            else
            {
                ComprobarPuntuacion();
            }
        }
        #endregion

        #region TercerBotón
        private void r3_Click(object sender, RoutedEventArgs e)
        {
            if (btnResp3.Content.ToString().Equals(respValida))
            {
                contador++;
                Contador.Text = contador.ToString();
                CargarDatos();
            }
            else
            {
                ComprobarPuntuacion();
            }
        }
        #endregion

        #region CuartoBotón
        private void r4_Click(object sender, RoutedEventArgs e)
        {
            if (btnResp4.Content.ToString().Equals(respValida))
            {
                contador++;
                Contador.Text = contador.ToString();
                CargarDatos();
            }
            else
            {
                ComprobarPuntuacion();
            }
        }
        #endregion

        #region QuintoBotón
        private void r5_Click(object sender, RoutedEventArgs e)
        {
            if (btnResp5.Content.ToString().Equals(respValida))
            {
                contador++;
                Contador.Text = contador.ToString();
                CargarDatos();
            }
            else
            {
                ComprobarPuntuacion();
            }
        }
        #endregion

        #region BotónJugarOtraVez
        private void ButtonReload_Click(object sender, RoutedEventArgs e)
        {
            contador = 0;
            Contador.Text = contador.ToString();
            MainContainer.Visibility = Visibility.Visible;
            LoseContainer.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region BotónCompartir
        private void ButtonShare_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }
        #endregion

        #region HyperLinkButton
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView.TryUnsnap();
        }
        #endregion

        #region Metodos
        private void ComprobarPuntuacion()
        {
            intentos--;
            if (intentos == 0)
            {
                Intento3.Visibility = Visibility.Visible;
                OverMessage.Text = "Obtuviste " + contador + " puntos en ¿Cuánto sabes de StarWars? \nSi deseas puedes compartir tu puntaje";
                MainContainer.Visibility = Visibility.Collapsed;
                LoseContainer.Visibility = Visibility.Visible;
                Intento1.Visibility = Visibility.Collapsed;
                Intento2.Visibility = Visibility.Collapsed;
                Intento3.Visibility = Visibility.Collapsed;
                DateTime dueTime = DateTime.Now.AddSeconds(1);
                Random rand = new Random();
                int idNumber = rand.Next(0, 10000000);
                ScheduleTileWithStringManipulation(contador.ToString(), dueTime, idNumber);
                intentos = 3;
                Contador.Text = contador.ToString();
                CargarDatos();
            }
            else
            {
                switch (intentos)
                {
                    case 2:
                        Intento1.Visibility = Visibility.Visible;
                        CargarDatos();
                        break;
                    case 1:
                        Intento2.Visibility = Visibility.Visible;
                        CargarDatos();
                        break;
                }

            }
        }
        private void CargarDatos()
        {
            int num1, num2, num3, num4;

            //Se direcciona al Diccionario de preguntas
            string path = Path.Combine("DiccionarioPreguntas.xml");
            XDocument xDoc = XDocument.Load(path);

            var preguntas = (from p in xDoc.Descendants("preguntas").Descendants("pregunta") select p).OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            var preg = preguntas.Elements("contenido").ToList();

            tbPregunta.Text = preg.FirstOrDefault().Value;

            var respuestavalida = preguntas.Elements("r1").ToList();
            respValida = respuestavalida.FirstOrDefault().Value;

            Random rn = new Random();
            numRespUsada = rn.Next(1, 6);

            var primerarespuesta = preguntas.Elements("r" + numRespUsada).ToList();
            btnResp1.Content = primerarespuesta.FirstOrDefault().Value;
            num1 = numRespUsada;

            # region SegundaRespuesta

            numRespUsada = rn.Next(1, 6);
            while (numRespUsada == num1)
            {
                numRespUsada = rn.Next(1, 6);
            }
            var segundarespuesta = preguntas.Elements("r" + numRespUsada).ToList();
            btnResp2.Content = segundarespuesta.FirstOrDefault().Value;
            num2 = numRespUsada;

            #endregion

            #region TerceraRespuesta
            numRespUsada = rn.Next(1, 6);
            while (numRespUsada == num1 || numRespUsada == num2)
            {
                numRespUsada = rn.Next(1, 6);
            }
            var a = preguntas.Elements("r" + numRespUsada).ToList();
            btnResp3.Content = a.FirstOrDefault().Value;
            num3 = numRespUsada;
            #endregion

            #region CuartaRespuesta
            numRespUsada = rn.Next(1, 6);
            while (numRespUsada == num1 || numRespUsada == num2 || numRespUsada == num3)
            {
                numRespUsada = rn.Next(1, 6);
            }

            var b = preguntas.Elements("r" + numRespUsada).ToList();
            btnResp4.Content = b.FirstOrDefault().Value;
            num4 = numRespUsada;
            #endregion

            #region ObtenerUltimaRespuestaYposicion

            numRespUsada = rn.Next(1, 6);
            while (numRespUsada == num1 || numRespUsada == num2 || numRespUsada == num3 || numRespUsada == num4)
            {
                numRespUsada = rn.Next(1, 6);
            }
            var quintarespuesta = preguntas.Elements("r" + numRespUsada).ToList();
            btnResp5.Content = quintarespuesta.FirstOrDefault().Value;

            #endregion
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = "Este es mi puntaje en ¿Cuanto Sabes de StarWars?";
            request.Data.Properties.Description = "Yo obtuve " + contador + " puntos!!";
            request.Data.SetText("Yo obtuve " + contador + " puntos!! En ¿Cuanto sabes de StarWars?");
        }
        #endregion

        #region [Notifications]
        void ScheduleTileWithStringManipulation(String puntaje, DateTime dueTime, int idNumber)
        {
            try
            {
                string tileXmlString = "<tile>"
                             + "<visual version='2'>"
                             + "<binding template='TileWide310x150PeekImage02'>"
                             + "<image id='1' src='/Assets/WideLogo.png' alt='alt text'/>"
                             + "<text id='1'>Ultimo puntaje</text>"
                             + "<text id='2'>Tu ultimo puntaje fue: " + puntaje + "</text>"
                             + "<text id='3'>Fecha: " + dueTime.ToLocalTime() + "</text>"
                             + "</binding>"
                             + "<binding template='TileSquarePeekImageAndText01'>"
                             + "<image id='1' src='/Assets/Logo.png' alt='alt text'/>"
                             + "<text id='1'>Ultimo puntaje</text>"
                             + "<text id='2'>Tu ultimo puntaje fue:" + puntaje + "</text>"
                             + "<text id='3'>" + dueTime.ToLocalTime() + "</text>"
                             + "</binding>"
                             + "</visual>"
                             + "</tile>";


                Windows.Data.Xml.Dom.XmlDocument tileDOM = new Windows.Data.Xml.Dom.XmlDocument();
                tileDOM.LoadXml(tileXmlString);
                ScheduledTileNotification futureTile = new ScheduledTileNotification(tileDOM, dueTime);
                futureTile.Id = "Tile" + idNumber;
                TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(futureTile);
            }
            catch (Exception ex)
            {

                new MessageDialog(ex.Message).ShowAsync();
            }

        }
        #endregion

    }
}
