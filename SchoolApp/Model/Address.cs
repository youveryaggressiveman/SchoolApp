using System.Collections.Generic;

namespace SchoolApp.Model
{
    public class Address
    {
        public int ID { get; set; }
        public string AddressName { get; set; }
        public string AddressNumber { get; set; }

        public virtual ICollection<City> City { get; set; }
    }
}