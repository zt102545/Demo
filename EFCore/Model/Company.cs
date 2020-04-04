using System;
using System.Collections.Generic;

namespace EFCore.Model
{
    public class Company
    {
        public Company()
        {
            CityCompanies = new List<CityCompany>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public string LegalPerson { get; set; }
        public List<CityCompany> CityCompanies { get; set; }
    }
}
