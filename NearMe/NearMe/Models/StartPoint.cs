namespace NearMe.Models
{
    /*
     * Punto di partenza per la ricerca dei luoghi nelle vicinanze
     */
    public class StartPoint
    {
        // Longitudine del punto di partenza
        public double Lat { get; set; }

        // Latitudine del punto di partenza
        public double Lon { get; set; }

        // Raggio di ricerca
        public int Radius { get; set; }

        // Indica se il punto di partenza e' stato impostato
        public bool IsSet { get; set; }
    }
}
