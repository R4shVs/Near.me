using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using NearMe.Models;
using NearMe.Views;
using System.Threading.Tasks;
using NearMe.Services;
using Plugin.Connectivity;

namespace NearMe.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel(Page currentPage)
        {
            CurrentPage = currentPage;
            Navigation = currentPage.Navigation;

            IsRadiusEnable = true;
            RadiusValue = 10;
            RadiusMaxValue = 50;
            RadiusMinValue = 1;

            LoadDataCommand = new Command(async () => await GetUserLocationAsync());
            UpdateUserPositionCommand = LoadDataCommand;
            NavToNextPageCommand = new Command(async () => await NavToSelectPlaceTypePageAsync());
        }

        private Page CurrentPage { get; set; }
        private INavigation Navigation { get; set; }


        /* ======= *
         * Modelli *
         * ======= */

        private StartPoint _startPoint = new StartPoint();
        private Address _address = new Address();
        private Radius _radius = new Radius();


        /* ====================================== *
         * Proprieta' per la gestione dei modelli *
         * ====================================== */

        // StartPoint
        public bool EnableNavigation
        {
            get => _startPoint.IsSet;
            set
            {
                _startPoint.IsSet = value;
                OnPropertyChanged();
            }
        }

        // Radius
        public bool IsRadiusEnable
        {
            get => _radius.IsEnabled;
            set
            {
                _radius.IsEnabled = value;
                OnPropertyChanged();
            }
        }

        public int RadiusValue
        {
            get => _radius.CurrentValue;
            set
            {
                _radius.CurrentValue = value;
                OnPropertyChanged();
            }
        }
        
        public int RadiusMaxValue { get; set; }

        public int RadiusMinValue { get; set; }

        // Address
        public string FormattedAddress
        {
            get => _address.FormattedAddress;
            set
            {
                _address.FormattedAddress = value;
                OnPropertyChanged();
            }
        }

        public bool IsAddressLoading
        {
            get => _address.IsLoading;
            set
            {
                _address.IsLoading = value;
                OnPropertyChanged();
            }
        }


        /* ======= *
         * Comandi *
         * ======= */

        /*
         * [Comando da eseguire quando la pagina viene creata]
         * Permette di ottenere le coordinate geografiche e
         * tradurle in un indirizzo
         */
        public ICommand LoadDataCommand { get; private set; }

        // Permette di aggiornare le coordinate geografiche e il relativo indirizzo
        public ICommand UpdateUserPositionCommand { get; private set; }

        // Permette di passare alla selezione del tipo di luoghi da cercare
        public ICommand NavToNextPageCommand { get; private set; }


        /* =================================================== *
         * Azioni da eseguire quando viene eseguito un comando *
         * =================================================== */

        // Azione per ottenere le coordinate geografiche relative alla posizione attuale dell'utente
        private async Task<Location> GetLocationCoordinatesAsync()
        {
            // Eventuale messaggio di errore
            string displayAlertMessage;

            try
            {
                // Geolocation.GetLocationAsync permette di recuperare le coordinate
                // di georilevazione correnti del dispositivo
                Location location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium
                });

                return location;
            }
            catch (FeatureNotSupportedException)
            {
                displayAlertMessage = "Il tuo dispositivo non è supportato.";
            }
            catch (FeatureNotEnabledException)
            {
                displayAlertMessage = "Verifica che la geolocalizzazione sia attiva.";
            }
            catch (PermissionException)
            {
                displayAlertMessage = "Verifica di aver autorizzato l'applicazione ad accedere alla tua posizione.";
            }
            catch(Exception)
            {
                displayAlertMessage = "Non siamo riusciti ad ottenere la tua posizione attuale.";
            }

            // In caso di errore verra' visualizzata una finestra di dialogo di avviso
            await CurrentPage.DisplayAlert("Impossibile utilizzare i servizi G.P.S.", displayAlertMessage, "Ok");

            return null;
        }

        /*
         * Azione per convertire le coordinate geografiche in un indirizzo formattato
         * Se non riesce ad ottenerlo restituisce una stringa contentente un messaggio di errore
         */
        private async Task<string> GetLocationAddressAsync(double lat, double lon)
        {
            // Eventuale titolo e messaggio di errore
            string displayAlertTitle, displayAlertMessage;

            try
            {
                // Per utilizzare i servizi di Google e' necessario un accesso ad internet
                if (CrossConnectivity.Current.IsConnected)
                {
                    string formattedAdress =
                        await RestService.RestServiceInstance().GetFormattedAddress(lat, lon);
                    
                    // Se la richiesta ha prodotto risultati restituisce l'indirizzo formattato
                    if (formattedAdress != null)
                    {
                        return formattedAdress;
                    }
                    else
                    {
                        displayAlertTitle = "Impossibile ottenere il tuo indirizzo";
                        displayAlertMessage = "Riprova più tardi.";
                    }
                }
                else
                {
                    displayAlertTitle = "Impossibile accedere ad internet";
                    displayAlertMessage = "Controlla la tua connessione e riprova.";
                }
            }
            catch (PermissionException)
            {
                displayAlertTitle = "Impossibile utilizzare internet";
                displayAlertMessage = "Verifica di aver autorizzato l'applicazione ad utilizzare la tua connessione.";
            }
            catch (Exception)
            {
                displayAlertTitle = "Impossibile ottenere il tuo indirizzo";
                displayAlertMessage = "Non siamo riusciti ad ottenere il tuo indirizzo attuale.";
            }

            // In caso di errore verra' visualizzata una finestra di dialogo di avviso
            await CurrentPage.DisplayAlert(displayAlertTitle, displayAlertMessage, "Ok");

            // E invece dell'indirizzo restituisco il messaggio di errore
            return displayAlertTitle;
        }

        // Azione per mostrare all'utente l'indirizzo relativo alla propria posizione attuale
        private async Task GetUserLocationAsync()
        {
            // Inizia il processo di acquisizione delle coordinate geografiche
            // e della relativa conversione in indirizzo
            IsAddressLoading = true;
            // Disabilita la possibilita' di navigare verso un'altra pagina
            EnableNavigation = false;

            // Ottiene le coordinate geografiche
            Location locationCoordinates = await GetLocationCoordinatesAsync();

            string address;

            // Controlla se e' riuscito ad ottenere le coordinate geografiche
            if (locationCoordinates != null)
            {
                _startPoint.Lat = locationCoordinates.Latitude;
                _startPoint.Lon = locationCoordinates.Longitude;

                // Una volta ottenute le coordinate geografiche e' possibile navigare verso la pagina successiva
                EnableNavigation = true;

                // Converte le coordinate geografiche
                address = await GetLocationAddressAsync(_startPoint.Lat, _startPoint.Lon);
            }
            else
            {
                // Altrimenti invece dell'indirizzo verra' visualizzato il seguente messaggio di errore
                address = "Impossibile utilizzare i servizi G.P.S.";
            }

            FormattedAddress = address;

            // Il processo e' finito
            IsAddressLoading= false;
        }

        // Azione per passare alla pagine per la selezione del tipo di luoghi da cercare
        private async Task NavToSelectPlaceTypePageAsync()
        {
            if (IsRadiusEnable)
            {
                _startPoint.Radius = RadiusValue;
            }
            else
            {
                _startPoint.Radius = -1;
            }

            await Navigation.PushAsync(new SelectPlaceTypePage(_startPoint));
        }
    }
}
