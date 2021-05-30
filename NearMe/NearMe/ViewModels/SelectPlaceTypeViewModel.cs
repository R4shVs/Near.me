using NearMe.Models;
using NearMe.Views;
using NearMe.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NearMe.ViewModels
{
    public class SelectPlaceTypeViewModel : BaseViewModel
    {
        public SelectPlaceTypeViewModel(Page currentPage, StartPoint startPoint)
        {
            Navigation = currentPage.Navigation;

            _startPoint = startPoint;

            LoadDataCommand = new Command(async () => await InitializeCategoriesListAsync());
            NavToNextPageCommand = new Command<PlaceType>(async pt => await SelectPlaceTypeAsync(pt));
        }

        private INavigation Navigation { get; set; }

        // Lista contenente le categorie di luoghi
        public List<PlaceCategory> Categories { get; private set; } = new List<PlaceCategory>();

        // Indica se la pagaina ha iniziato a caricare i dati
        public bool IsLoadingData { get; set; }


        /* ======= *
         * Modelli *
         * ======= */

        private StartPoint _startPoint;

        /* ======= *
         * Comandi *
         * ======= */

        /*
         * [Comando da eseguire quando la pagina viene creata]
         * Inizializza la lista di categorie
         */
        public ICommand LoadDataCommand { get; private set; }

        /* 
         * Permette di passare alla pagina contenente la lista di luoghi
         * della tipologia scelta
         */
        public ICommand NavToNextPageCommand { get; private set; }


        /* =================================================== *
         * Azioni da eseguire quando viene eseguito un comando *
         * =================================================== */

        // Azione per inizializzare la lista contenente le categorie di luoghi suddivisi per tipo
        private async Task InitializeCategoriesListAsync()
        {
            // Inizia il processo di inizializzazione
            IsLoadingData = true;
            OnPropertyChanged(nameof(IsLoadingData));

            // Aspetta che la pagina sia pronta
            await Task.Delay(1000);
            
            Categories = new List<PlaceCategory>
            {
                new PlaceCategory{Categoty = "Di vario tipo", Types=PlaceTypeCategories.Miscellaneous},
                new PlaceCategory{Categoty = "Intrattenimento", Types=PlaceTypeCategories.Entertainment},
                new PlaceCategory{Categoty = "Vita notturna", Types=PlaceTypeCategories.NightLife},
                new PlaceCategory{Categoty = "Cibo", Types=PlaceTypeCategories.Food},
                new PlaceCategory{Categoty = "Negozi", Types=PlaceTypeCategories.Shops},
                new PlaceCategory{Categoty = "Soccorsi", Types=PlaceTypeCategories.Rescue},
                new PlaceCategory{Categoty = "Assistenza", Types=PlaceTypeCategories.Assistance},
                new PlaceCategory{Categoty = "Salute", Types=PlaceTypeCategories.Health},
                new PlaceCategory{Categoty = "Relax", Types=PlaceTypeCategories.Relax},
                new PlaceCategory{Categoty = "Trasporti", Types=PlaceTypeCategories.Transport},
                new PlaceCategory{Categoty = "Religione", Types=PlaceTypeCategories.Religion},
            };

            // Avverte che la lista si e' aggiornata
            OnPropertyChanged(nameof(Categories));

            // Il processo e' finito
            IsLoadingData = false;

            // Avverte che i dati sono stati caricati
            OnPropertyChanged(nameof(IsLoadingData));
        }

        // Azione per passare alla pagina contenente la lista di luoghi della tipologia scelta
        private async Task SelectPlaceTypeAsync(PlaceType placeType)
        {
            if (placeType == null)
                return;

            await Navigation.PushAsync(new NearMePlacesPage(_startPoint, placeType));
        }
    }
}