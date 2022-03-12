using System.Collections.Generic;

namespace SchoolApp.Model
{
    public class City
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Country> Countries { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}