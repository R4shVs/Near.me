namespace NearMe.Models
{
    /*
     * Raggio di ricerca dei luoghi nelle vicinanze
     */
    public class Radius
    {
        // Valore attuale del raggio di ricerca
        public int CurrentValue { get; set; }

        // Valore massimo del raggio di ricerca
        public int MaxValue { get; set; }

        // Valore minimo del raggio di ricerca
        public int MinValue { get; set; }

        // Indica se il raggio di ricerca e' abilitato
        public bool IsEnabled { get; set; }
    }
}
