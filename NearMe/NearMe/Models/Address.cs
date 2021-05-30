namespace NearMe.Models
{
    /*
     * Indirizzo relativo alla posizione attuale
     */
    public class Address
    {
        // Indirizzo formattato
        public string FormattedAddress { get; set; }

        // Stato dell'ottenimento dell'indirizzo formattato
        public bool IsLoading { get; set; }

    }
}
