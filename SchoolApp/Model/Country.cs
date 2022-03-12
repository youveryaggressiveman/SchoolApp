using System.Collections.Generic;

namespace SchoolApp.Model
{
    public class Country
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}