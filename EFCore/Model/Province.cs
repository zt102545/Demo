using System.Collections.Generic;

namespace EFCore.Model
{
    public class Province
    {
        public Province()
        {
            Cities = new List<City>();
        }

        public int ID { get; set; }
        public string name { get; set; }

        public int population { get; set; }

        public List<City> Cities { get; set; }
    }
}
