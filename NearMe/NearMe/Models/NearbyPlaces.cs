using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NearMe.Models
{
    /*
     * Luoghi nelle vicinanze
     */
    public class NearbyPlaces
    {
        // Lista dei luoghi nelle vicinanze
        public ObservableCollection<Place> Places { get; set; }

        // Pagina successiva
        public string CurrentPageToken { get; set; }

        // Indica se la lista sta caricando gli elementi
        public bool IsListLoading { get; set; }

        // Indica se e' stata raggiunta l'ultima pagina
        public bool LastPageReached { get; set; }
    }
}
