namespace NearMe.Services
{
    /*
     * Classe che fornisce informazioni per l'utilizzo dele A.P.I. di Google
     */
    public static class GoogleService
    {
        // Chiave per l'utilizzo delle A.P.I.
        public const string GoogleApiKEY = Data.Secrets.GoogleApiKEY;

        // Servizio per convertire le coordinate geografiche in indirizzo
        public const string ReverseGeocodingEndpoint = "https://maps.googleapis.com/maps/api/geocode";

        // Servizio per ottenere i luoghi presenti in un'area specifica
        public const string NearbySearchEndpoint = "https://maps.googleapis.com/maps/api/place/nearbysearch";

        // Servizio per ottenere l'immagine di un luogo
        public const string PlacePhotoEndpoint = "https://maps.googleapis.com/maps/api/place";
    }
}
