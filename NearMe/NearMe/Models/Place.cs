namespace NearMe.Models
{
    /*
     * Luogo
     */
    public class Place
    {
        // Immagine
        public string ImageURL { get; set; }
        
        // Indica se ha un'immagine presa esternamente
        public bool HasImageURL { get; set; }

        // Nome
        public string Name { get; set; }

        // Indirizzo
        public string Address { get; set; }

        // Longitudine
        public double Lat { get; set; }

        // Latitudine
        public double Lon { get; set; }

        // Indica se appartiene ad una tipologia che necessita di una valutazione
        public bool HasRating { get; set; }

        // Valutazione
        public double Rating { get; set; }

        // Numero di recensioni
        public int TotalRatings { get; set; }
    }
}
