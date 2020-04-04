namespace EFCore.Model
{
    public class CityCompany
    {
        public int CityID { get; set; }
        public City City { get; set; }

        public int CompanyID { get; set; }
        public Company Company { get; set; }
    }
}
