namespace SchoolApp.Model
{
    public class Address
    {
        public int Id { get; set; }
        public string AddressNumber { get; set; }
        public string AddressName { get; set; }
        public int CityId { get; set; }

        public virtual City City { get; set; }
    }
}