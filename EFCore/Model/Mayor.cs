namespace EFCore.Model
{
    public class Mayor
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int sex { get; set; }

        public int CityID { get; set; }
        public City City { get; set; }
    }
}
