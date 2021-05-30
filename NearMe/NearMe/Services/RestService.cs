using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NearMe.Models;
using Newtonsoft.Json.Linq;

namespace NearMe.Services
{
    /*
     * Classe che interagisce con le API
     */
    class RestService
    {
        private static RestService _restServiceInstance = null;

        protected RestService()
        {
            _client = new HttpClient();
        }

        public static RestService RestServiceInstance()
        {
            if (_restServiceInstance == null)
                _restServiceInstance = new RestService();
            return _restServiceInstance;
        }

        private HttpClient _client;

        // Metodo per mandare inviare una richiesta GET all'URI specificato come parametro
        private async Task<JObject> SendRequestAsync(string uriParameters)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uriParameters);
                return await ReadResponseContentAsync(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /* Metodo per mandare inviare una richiesta GET all'URI specificato come parametro
         * Permette inoltre di interrompere la richiesta
         */
        private async Task<JObject> SendRequestAsync(string uriParameters, CancellationToken token)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uriParameters, token);
                return await ReadResponseContentAsync(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Metodo per leggere il contenuto di una chiamata HTTP 
        private async Task<JObject> ReadResponseContentAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                // Se la richiesta va a buon fine la restituisce
                string content = await response.Content.ReadAsStringAsync();

                return JObject.Parse(content);
            }
            else
            {
                return null;
            }
        }

        // Metodo per mandare una richiesta a un URI e ricevere la relativa risposta
        private string SendWebRequest(string uriParameters)
        {
            WebRequest myWebRequest = WebRequest.Create(uriParameters);
            WebResponse myWebResponse = myWebRequest.GetResponse();

            string url = myWebResponse.ResponseUri.ToString();

            myWebResponse.Close();

            return url;
        }
        
        // Metodo per formattare le coordinate geografiche nel formato richesto dalle A.P.I.
        private string FormattedCordinate(double coordinate)
        {
            return coordinate.ToString().Replace(',', '.');
        }

        // Metodo per converte una risposta JSON in una lista di luoghi
        private List<Place> DeserializePlaces(JArray response, PlaceType placeType, CancellationToken token)
        {
            List<Place> nearbyPlaces = new List<Place>();

            for (int i = 0; i < response.Count; i++)
            {
                // Se il processo viene cancellato
                // interrompe il ciclo
                if (token.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    // Alcuni luoghi potrebbero non avere un'immagine
                    bool HasImageURL = response[i].ToString().Contains("photos");
                    bool HasRating = placeType.HasRating;

                    // Se il luogo non ha l'immagine verra' visualizzata
                    // l'icona relativa alla tipologia da ottenere
                    string URL = HasImageURL ?
                         GetPlacePhotoUrl((string)response[i]["photos"][0]["photo_reference"]) : $"{placeType.Type}.png";

                    // Creazione del luogo 
                    Place currentPlace = new Place
                    {
                        ImageURL = URL,
                        Name = (string)response[i]["name"],
                        Address = (string)response[i]["vicinity"],
                        Lat = (double)response[i]["geometry"]["location"]["lat"],
                        Lon = (double)response[i]["geometry"]["location"]["lng"],
                        HasImageURL = HasImageURL,
                        HasRating = HasRating
                    };

                    // Se il luogo necessita di una valutazione
                    if (HasRating)
                    {
                        currentPlace.Rating = (double)response[i]["rating"];
                        currentPlace.TotalRatings = (int)response[i]["user_ratings_total"];
                    }

                    nearbyPlaces.Add(currentPlace);
                }
                catch (Exception)
                {
                }
            }

            return nearbyPlaces;
        }

        /* ================================== *
         * Metodoti per interagire con le API *
         * ================================== */

        /*
         * ReverseGeocoding
         */

        // Converte le coordinate geografiche in un indirizzo formattato
        public async Task<string> GetFormattedAddress(double lat, double lon)
        {
            string uri =
                GoogleService.ReverseGeocodingEndpoint + "/json?key=" +
                GoogleService.GoogleApiKEY + "&latlng=" + FormattedCordinate(lat) + "," + FormattedCordinate(lon);

            JObject responseJSON = await SendRequestAsync(uri);
            JArray responseResults = (JArray)responseJSON["results"];

            string formattedAddress = (string)responseResults[0]["formatted_address"];

            return formattedAddress;
        }

        /*
         * NearbySearch
         */

        /*
         * Metodi per ottenere i luoghi vicini alle coordinate geografiche fornite come parametro
         * nel raggio di richerca fornito come parametro
         * del tipo specificato come parametro
         */

        // Restituisce i luoghi in ordine di rilevanza
        public async Task<(List<Place> placesList, string nextPage)> GetNearbyPlacesRankByProminenceAsync(double lat, double lon, int radius, PlaceType placeType, CancellationToken token)
        {
            string uriParameters =
                "&location=" + FormattedCordinate(lat) + "," + FormattedCordinate(lon) +
                "&radius=" + (radius*1000) + "&type=" + placeType.Type;

            return await GetNearbyPlacesAsync(uriParameters, placeType, token);
        }

        // Restituisce i successivi luoghi in ordine di rilevanza
        public async Task<(List<Place> placesList, string nextPage)> GetNextNearbyPlacesRankByProminenceAsync(double lat, double lon, int radius, PlaceType placeType, string nextPage, CancellationToken token)
        {
            string uriParameters =
                "&location=" + FormattedCordinate(lat) + "," + FormattedCordinate(lon) +
                "&radius=" + (radius * 1000) + "&type=" + placeType.Type + "&pagetoken=" + nextPage;

            return await GetNearbyPlacesAsync(uriParameters, placeType, token);
        }

        // Restituisce i luoghi in ordine di vicinanza
        public async Task<(List<Place> placesList, string nextPage)> GetNearbyPlacesRankByDistanceAsync(double lat, double lon, PlaceType placeType, CancellationToken token)
        {
            string uriParameters =
                "&location=" + FormattedCordinate(lat) + "," + FormattedCordinate(lon) +
                "&rankby=distance&type=" + placeType.Type;

            return await GetNearbyPlacesAsync(uriParameters, placeType, token);
        }

        // Restituisce i successivi luoghi in ordine di vicinanza
        public async Task<(List<Place> placesList, string nextPage)> GetNextNearbyPlacesRankByDistanceAsync(double lat, double lon, PlaceType placeType, string nextPage, CancellationToken token)
        {
            string uriParameters =
                "&location=" + FormattedCordinate(lat) + "," + FormattedCordinate(lon) +
                "&rankby=distance&type=" + placeType.Type + "&pagetoken=" + nextPage;

            return await GetNearbyPlacesAsync(uriParameters, placeType, token);
        }

        /*
         * Ottiene i luoghi presenti in un'area specifica fornita nella richiesta
         * Ritorna al piu' 20 luoghi e la pagina successiva (se presente)
         */
        private async Task<(List<Place> placesList, string nextPage)> GetNearbyPlacesAsync(string uriParameters, PlaceType placeType, CancellationToken token)
        {
            string uri = GoogleService.NearbySearchEndpoint + "/json?key=" +
                         GoogleService.GoogleApiKEY + uriParameters;

            JObject responseJSON = await SendRequestAsync(uri, token);

            string status = (string)responseJSON["status"];

            // Talvolta la richiesta puo' essere invalida per ragioni temporali
            // Come per esempio quando proviamo ad usare un token non ancora attivo
            if (status.Contains("INVALID_REQUEST"))
            {
                // Aspettiamo
                await Task.Delay(300);
                // E proviamo ancora
                responseJSON = await SendRequestAsync(uri, token);
                // Se la richiesta sara' nuovamente invalita il campo
                // result della risposta sara' presente, ma vuoto
            }

            JArray responseResults = (JArray)responseJSON["results"];
            string responseNextPage;

            // Controlla se ci sono altre pagine
            if (responseJSON.ContainsKey("next_page_token"))
            {
                responseNextPage = (string)responseJSON["next_page_token"];
            }
            else
            {
                responseNextPage = null;
            }

            // Converte i risultati in luoghi
            List<Place> nearbyPlaces = await Task.Run(() => DeserializePlaces(responseResults, placeType, token));

            return (nearbyPlaces, responseNextPage);

        }

        /*
         * PlacePhoto
         */

        // Ottiene l'URL dell'immagine del luogo fornito nella richiesta
        private string GetPlacePhotoUrl(string photoReference)
        {

            string uri =
                GoogleService.PlacePhotoEndpoint + "/photo?key=" +
                GoogleService.GoogleApiKEY + "&maxwidth=400&photoreference=" +
                photoReference;

            string url = SendWebRequest(uri);

            return url;
        }
    }
}
