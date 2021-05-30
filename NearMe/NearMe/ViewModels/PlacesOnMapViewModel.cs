using NearMe.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace NearMe.ViewModels
{
    public class PlacesOnMapViewModel : BaseViewModel
    {
        public PlacesOnMapViewModel(List<Place> places, Place focusPlace)
        {
            Places = places;
            _focusPlace = focusPlace;

            LoadDataCommand = new Command<Map>(async map => await FocusPlaceOnMapAsync(map));
        }

        // Luoghi da visualizzare nella mappa
        private List<Place> Places { get; set; }

        // Pin che rappresentano i luoghi nella mappa
        public List<Pin> Pins { get; private set; }

        /* ======= *
         * Modelli *
         * ======= */

        private Place _focusPlace;


        /* ======= *
         * Comandi *
         * ======= */

        /*
         * [Comando da eseguire quando la pagina viene creata]
         * Permette di inizializzare la lista dei luoghi nelle vicinanze
         */
        public ICommand LoadDataCommand { get; private set; }


        /* =================================================== *
         * Azioni da eseguire quando viene eseguito un comando *
         * =================================================== */

        // Azione per centrare la visuale della mappa e visualizzare i pin
        private async Task FocusPlaceOnMapAsync(Map map)
        {
            // Centra la visuale della mappa sul luogo selezionato
            map.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(_focusPlace.Lat, _focusPlace.Lon), Distance.FromMeters(100)));

            // Aspetta che la mappa si sia caricata
            await Task.Delay(1000);

            // Lista temporanea che conterra' i pin
            List<Pin> tmp = new List<Pin>();

            // Conversione dei luoghi in pin
            Places.ForEach(x => tmp.Add(PlaceToMapPin(x)));
            
            // Assenga la lista temporanea
            Pins = tmp;

            // Avverte che la lista si e' aggiornata
            OnPropertyChanged(nameof(Pins));
        }


        // Metodo per convertire i luoghi in pin
        private Pin PlaceToMapPin(Place place)
        {
            Pin pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(place.Lat, place.Lon),
                Address = place.Address,
                Label = place.Name
            };

            return pin;
        }

    }
}
