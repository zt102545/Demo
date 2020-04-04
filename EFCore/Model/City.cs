using System.Collections.Generic;

namespace EFCore.Model
{
    public class City
    {
        public City()
        {
            CityCompanies = new List<CityCompany>();
        }

        public int ID { get; set; }
        public string name { get; set; }
        public string areaCode { get; set; }

        public int ProvinceID { get; set; }
        public Province Province { get; set; }

        public List<CityCompany> CityCompanies { get; set; }

        public Mayor Mayor { get; set; }

    }
}
