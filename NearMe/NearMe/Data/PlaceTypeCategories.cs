using NearMe.Models;
using System.Collections.Generic;

namespace NearMe.Data
{
    /*
     * Classe contenente tutte le categorie di luoghi suddivisi per tipo
     */
    public static class PlaceTypeCategories
    {
        public static List<PlaceType> Assistance { get; private set; } = new List<PlaceType>
        {
            new PlaceType {Title="Idraulici", Type="plumber"},
            new PlaceType {Title="Elettricisti", Type="electrician"},
            new PlaceType {Title="Imbianchini", Type="painter"},
            new PlaceType {Title="Fabbri", Type="locksmith"},
            new PlaceType {Title="Meccanico", Type="car_repair"},
            new PlaceType {Title="Avvocati", Type="lawyer"},
            new PlaceType {Title="Fioristi", Type="florist"},
            new PlaceType {Title="Lavanderia", Type="laundry"},
            new PlaceType {Title="Autolavaggio", Type="car_wash"},
            new PlaceType {Title="Ufficio postale ", Type="post_office"}
        };

        public static List<PlaceType> Entertainment { get; private set; } = new List<PlaceType>
        {
            new PlaceType {Title="Acquari", Type="aquarium"},
            new PlaceType {Title="Gallerie d'arte", Type="art_gallery"},
            new PlaceType {Title="Bowling", Type="bowling_alley"},
            new PlaceType {Title="Musei", Type="museum"},
            new PlaceType {Title="Zoo", Type="zoo"},
            new PlaceType {Title="Cinema", Type="movie_theater"}
        };

        public static List<PlaceType> Food { get; private set; } = new List<PlaceType>
        {
            new PlaceType {Title="Panifici", Type="bakery"},
            new PlaceType {Title="Bar / caffetterie", Type="cafe"},
            new PlaceType {Title="D'asporto", Type="meal_takeaway"},
            new PlaceType {Title="Ristoranti", Type="restaurant"}
        };

        public static List<PlaceType> Health { get; private set; } = new List<PlaceType>
        {
            new PlaceType {Title="Farmacie", Type="pharmacy", HasRating = false},
            new PlaceType {Title="Dottori", Type="doctor", HasRating = false},
            new PlaceType {Title="Fisioterapisti", Type="physiotherapist", HasRating = false},
            new PlaceType {Title="Dentisti", Type="dentist", HasRating = false}
        };

        public static List<PlaceType> Miscellaneous { get; private set; } = new List<PlaceType>
        {
            new PlaceType {Title="Banche", Type="bank", HasRating = false},
            new PlaceType {Title="Distributori di benzina", Type="gas_station", HasRating = false},
            new PlaceType {Title="Paleste", Type="gym"},
            new PlaceType {Title="Hotel", Type="lodging"},
            new PlaceType {Title="Parchi", Type="park", HasRating = false},
            new PlaceType {Title="Parcheggi", Type="parking", HasRating = false},
            new PlaceType {Title="Negozi", Type="store"}
        };

        public static List<PlaceType> NightLife { get; private set; } = new List<PlaceType>
        {
            new PlaceType {Title="Casino", Type="casino"},
            new PlaceType {Title="Discoteche", Type="night_club"},
            new PlaceType {Title="Enoteche e negozi di alcolici", Type="liquor_store"}
        };

        public static List<PlaceType> Relax { get; private set; } = new List<PlaceType>
        {
            new PlaceType {Title="Spa", Type="spa"},
            new PlaceType {Title="Saloni di bellezza", Type="beauty_salon"},
            new PlaceType {Title="Parrucchieri", Type="hair_care"}
        };

        public static List<PlaceType> Religion { get; private set; } = new List<PlaceType>
        {
            new PlaceType {Title="Chiese", Type="church", HasRating = false},
            new PlaceType {Title="Moschee", Type="mosque", HasRating = false},
            new PlaceType {Title="Sinagoghe", Type="synagogue", HasRating = false}
        };

        public static List<PlaceType> Rescue { get; private set; } = new List<PlaceType>
        {
            new PlaceType {Title="Ospedali", Type="hospital", HasRating = false},
            new PlaceType {Title="Stazione di polizia", Type="police", HasRating = false},
            new PlaceType {Title="Caserma dei vigili del fuoco", Type="fire_station", HasRating = false}
        };

        public static List<PlaceType> Shops { get; private set; } = new List<PlaceType>
        {
            new PlaceType {Title="Negozi di biciclette", Type="bicycle_store"},
            new PlaceType {Title="Librerie", Type="book_store"},
            new PlaceType {Title="Enoteche e negozi di alcolici", Type="liquor_store"},
            new PlaceType {Title="Negozi di abbigliamento", Type="clothing_store"},
            new PlaceType {Title="Negozi di elettronica", Type="electronics_store"},
            new PlaceType {Title="Negozi di mobili", Type="furniture_store"},
            new PlaceType {Title="Gioiellerie", Type="jewelry_store"},
            new PlaceType {Title="Negozi di scarpe", Type="shoe_store"},
            new PlaceType {Title="Negozi di animali", Type="pet_store"},
            new PlaceType {Title="Supermercati", Type="supermarket"}
        };

        public static List<PlaceType> Transport { get; private set; } = new List<PlaceType>
        {
            new PlaceType {Title="Taxi", Type="taxi_stand", HasRating = false},
            new PlaceType {Title="Aeroporto", Type="airport", HasRating = false},
            new PlaceType {Title="Stazione degli autobus", Type="bus_station", HasRating = false},
            new PlaceType {Title="Stazione dei treni ", Type="train_station", HasRating = false}
        };
    }
}
