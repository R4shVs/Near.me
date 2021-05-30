using System.Collections.Generic;
using System.Linq;

namespace NearMe.Models
{
    /*
     * Categoria di luoghi 
     */
    public class PlaceCategory
    {
        // Nome categoria
        public string Categoty { get; set; }

        // Lista di tipologie
        private List<PlaceType> _types;
        public List<PlaceType> Types
        {
            get => _types;
            set
            {
                _types = value.OrderBy(o => o.Title).ToList();
            }
        }
    }
}
