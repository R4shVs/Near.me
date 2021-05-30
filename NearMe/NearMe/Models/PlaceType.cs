namespace NearMe.Models
{
    /*
     * Tipo di luogo
     */
    public class PlaceType
    {
        public PlaceType()
        {
            HasRating = true;
        }

        // Titolo per indicare la tipologia
        public string Title { get; set; }

        // Valore per identificare la tipologia durante la ricerca dei luoghi nelle vicinanze
        public string Type { get; set; }

        // Indica se la tipologia necessita di una valutazione
        public bool HasRating { get; set; }
    }
}
