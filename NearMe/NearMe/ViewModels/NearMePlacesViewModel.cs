using NearMe.Models;
using NearMe.Views;
using Plugin.Connectivity;
using NearMe.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace NearMe.ViewModels
{
    public class NearMePlacesViewModel : BaseViewModel
    {
        public NearMePlacesViewModel(Page currentPage, StartPoint startPoint, PlaceType placeType)
        {
            CurrentPage = currentPage;
            Navigation = currentPage.Navigation;

            _startPoint = startPoint;
            _placeType = placeType;

            NearbyPlaces = new ObservableCollection<Place>();
            _nearbyPlaces.LastPageReached = false;
            _nearbyPlaces.CurrentPageToken = "";

            LoadDataCommand = new Command(async () => await GetNearbyPlacesAsync());
            LoadMorePlacesCommand = new Command(async () => await GetNextNearbyPlacesAsync(), CanLoadMorePlaces);
            NavToNextPageCommand = new Command<Place>(async p => await DisplayPlaceOnMapAsync(p));
            OnBackButtonPressedCommand = new Command(CancellCurrentTask);
        }

        // Token per cancellare un'attivita'
        private CancellationTokenSource _token;

        private Page CurrentPage { get; set; }
        private INavigation Navigation { get; set; }

        // Buffer che contiene i luoghi da caricare quando viene richiesto il caricamento di nuovi luoghi
        private List<Place> NearbyPlacesBuffer { get; set; } = new List<Place>();


        /* ======= *
         * Modelli *
         * ======= */

        private StartPoint _startPoint;
        private PlaceType _placeType;
        private NearbyPlaces _nearbyPlaces = new NearbyPlaces();


        /* ====================================== *
         * Proprieta' per la gestione dei modelli *
         * ====================================== */

        // NearbyPlaces

        private bool CanLoadMorePlaces()
        {
            if (_nearbyPlaces.LastPageReached)
                return false;
            return true;
        }

        public ObservableCollection<Place> NearbyPlaces
        {
            get => _nearbyPlaces.Places;
            set => _nearbyPlaces.Places = value;
        }

        public bool IsNearbyPlacesListLoading
        {
            get => _nearbyPlaces.IsListLoading;
            set
            {
                _nearbyPlaces.IsListLoading = value;
                OnPropertyChanged();
            }
        }


        /* ======= *
         * Comandi *
         * ======= */

        /*
         * [Comando da eseguire quando la pagina viene creata]
         * Permette di inizializzare la lista dei luoghi nelle vicinanze
         */
        public ICommand LoadDataCommand { get; private set; }

        // Permette di caricare altri luoghi nella lista dei luoghi nelle vicinanze
        public ICommand LoadMorePlacesCommand { get; private set; }

        // Permette di passare alla mappa dove verra' visualizzato il luogo
        public ICommand NavToNextPageCommand { get; private set; }

        // Permette di fermare l'attivita in background corrente 
        public ICommand OnBackButtonPressedCommand { get; private set; }


        /* =================================================== *
         * Azioni da eseguire quando viene eseguito un comando *
         * =================================================== */

        /*
         * Azione per inizializzare la lista dei luoghi nelle vicinanze
         * con al piu' 10 luoghi, i restanti, se ci sono, li inserisce nel buffer
         */
        public async Task GetNearbyPlacesAsync()
        {
            // Inizia il processo di inizializzazione
            IsNearbyPlacesListLoading = true;

            // Aspetta che la pagina sia pronta
            await Task.Delay(1000);

            // Lista che contiene i luoghi restituiti dalla chiamata
            List<Place> nearbyPlaces = await RequestNearbyPlacesAsync();

            // Se la chiamata e' andata a buon fine
            if (nearbyPlaces != null)
            {
                // Ma non ha prodotto risulati e non ci sono altre pagine nelle quali cercare
                if (nearbyPlaces.Count == 0 && _nearbyPlaces.CurrentPageToken == null)
                {
                    // Verra' visualizzata una finestra di dialogo di avviso
                    await CurrentPage.DisplayAlert("Non abbiamo trovato risultati",
                        "Non ci sono luoghi della tipologia selezionata. ", "Ok");

                    _nearbyPlaces.LastPageReached = true;
                }
                else
                {
                    // Se la prima chiamata non ha prodotto abbastanza risultati,
                    // cerca nelle altre pagine
                    if (nearbyPlaces.Count < 10)
                    {
                        // Finche' i luoghi sono meno di 10 e ci sono ancora pagine
                        while (nearbyPlaces.Count < 10 && _nearbyPlaces.CurrentPageToken != null)
                        {
                            // Cerca di ottenere altri luoghi
                            List<Place> nextNearbyPlaces = await RequestNearbyPlacesAsync();

                            // Se ci sono 
                            if (nearbyPlaces != null)
                            {
                                // Li aggiunge
                                nextNearbyPlaces.ForEach(x => nearbyPlaces.Add(x));
                            }
                        }

                        // Se non ci sono luoghi verra' visualizzata una finestra di dialogo di avviso
                        if (nearbyPlaces.Count == 0)
                        {
                            await CurrentPage.DisplayAlert("Non abbiamo trovato risultati",
                                "Non ci sono luoghi della tipologia selezionata.", "Ok");

                            _nearbyPlaces.LastPageReached = true;
                        }
                    }

                    // Ciclo per aggiungere i luoghi trovati
                    for (int i = 0; i < nearbyPlaces.Count; i++)
                    {
                        // Aggiunge i primi 10 nella lista
                        if (i < 10)
                        {
                            NearbyPlaces.Add(nearbyPlaces[i]);
                        }
                        // I restanti li mette nel buffer
                        else
                        {
                            NearbyPlacesBuffer.Add(nearbyPlaces[i]);
                        }
                    }
                }
            }

            // Il processo e' finito
            IsNearbyPlacesListLoading = false;
        }

        // Azione per caricare altri luoghi nella lista dei luoghi nelle vicinanze
        private async Task GetNextNearbyPlacesAsync()
        {
            IsNearbyPlacesListLoading = true;

            var pageToken = _nearbyPlaces.CurrentPageToken;

             // Controlla se il buffer ha a almeno 10 luoghi
             // o se ci sono luoghi ma non ci sono piu' pagine
            if (NearbyPlacesBuffer.Count >= 10 || (NearbyPlacesBuffer.Count > 0 && pageToken == null))
            {
                // Carica al piu' 10 luoghi nella lista dei luoghi nelle vicinanze
                for (int i = 0; (i < 10 && i < NearbyPlacesBuffer.Count); i++)
                {
                    NearbyPlaces.Add(NearbyPlacesBuffer[i]);
                }
                
                // Rimuove i luoghi aggiunti
                NearbyPlacesBuffer.RemoveRange(0, Math.Min(10, NearbyPlacesBuffer.Count));
            }
            // Se i luoghi nel buffer sono troppo pochi
            else if (pageToken != null)
            {
                // Fa una nuova richiesta per aggiungerli
                List<Place> nearbyPlaces = await RequestNearbyPlacesAsync();

                // Se ci sono 
                if (nearbyPlaces != null)
                {
                    // Li aggiunge
                    nearbyPlaces.ForEach(x => NearbyPlacesBuffer.Add(x));

                    // E esegue il comando a lui associato
                    LoadMorePlacesCommand.Execute(null);
                }
                // Se c'è stato un errore
                else
                {
                    _nearbyPlaces.LastPageReached = true;
                }
            }
            else
            {
                _nearbyPlaces.LastPageReached = true;
            }

            IsNearbyPlacesListLoading = false;
        }

        // Metodo per ottenere i luoghi nelle vicinanze
        private async Task<List<Place>> RequestNearbyPlacesAsync()
        {
            // Token per cancellare l'attivita'
            _token = new CancellationTokenSource();
            CancellationToken token = _token.Token;

            // Eventuale titolo e messaggio di errore
            string displayAlertTitle, displayAlertMessage;

            try
            {
                // Per utilizzare i servizi di Google e' necessario un accesso ad internet
                if (CrossConnectivity.Current.IsConnected)
                {
                    (List<Place> placesList, string nextPage) result;

                    // Pagina corrente
                    var pageToken = _nearbyPlaces.CurrentPageToken;

                    // Prima pagina
                    if (pageToken != null && pageToken.Length == 0)
                    {
                        // Restituisce i luoghi in ordine di rilevanza
                        if (_startPoint.Radius > 0)
                        {
                            result = await RestService.RestServiceInstance()
                                .GetNearbyPlacesRankByProminenceAsync(_startPoint.Lat, _startPoint.Lon, _startPoint.Radius, _placeType, token);
                        }
                        // Restituisce i luoghi in ordine di vicinanza
                        else
                        {
                            result = await RestService.RestServiceInstance()
                                .GetNearbyPlacesRankByDistanceAsync(_startPoint.Lat, _startPoint.Lon, _placeType, token);
                        }
                    }
                    // Pagine successive
                    else if (pageToken != null && pageToken.Length > 0)
                    {
                        // Restituisce i luoghi in ordine di rilevanza
                        if (_startPoint.Radius > 0)
                        {
                            result = await RestService.RestServiceInstance()
                                .GetNextNearbyPlacesRankByProminenceAsync(_startPoint.Lat, _startPoint.Lon,
                                _startPoint.Radius, _placeType, _nearbyPlaces.CurrentPageToken, token);
                        }
                        // Restituisce i luoghi in ordine di vicinanza
                        else
                        {

                            result = await RestService.RestServiceInstance()
                               .GetNextNearbyPlacesRankByDistanceAsync(_startPoint.Lat, _startPoint.Lon,
                               _placeType, _nearbyPlaces.CurrentPageToken, token);
                        }
                    }
                    // Non ci sono piu' pagine
                    else
                    {
                        return new List<Place>();
                    }

                    _nearbyPlaces.CurrentPageToken = result.nextPage;


                    return result.placesList;
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
                if (token.IsCancellationRequested)
                    return null;

                displayAlertTitle = "Impossibile ottenere luoghi vicino a te";
                displayAlertMessage = "Non siamo riusciti ad ottenere i luoghi vicino a te.";
            }

            _token.Dispose();
            await CurrentPage.DisplayAlert(displayAlertTitle, displayAlertMessage, "Ok");

            return null;
        }

        /*  
         * Azione per passare alla mappa contenente tutti i luoghi nelle vicinanze,
         * dove verra' visualizzato il luogo in ingresso al centro
         */
        private async Task DisplayPlaceOnMapAsync (Place place)
        {
            if (place == null)
                return;

            // Trasforma l'ObservableCollection in List
            List<Place> nearbyPlaces = new List<Place>(NearbyPlaces);

            await Navigation.PushAsync(new PlacesOnMapPage(nearbyPlaces, place));
        }

        // Azione per fermare l'attivita in background corrente 
        private void CancellCurrentTask()
        {
            try
            {
                _token.Cancel();
            }
            catch (Exception)
            {

            }
        }
    }
}
