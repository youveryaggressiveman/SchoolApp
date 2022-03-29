using SchoolApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Core.Builders
{
    public class CountryBuilder : StudentBuilder
    {
        public CountryBuilder(Student student) : base(student)
        {
            _student.User.Address.City.ToList()[0].Countries = new List<Country>();
        }

        public CountryBuilder WithCountry(Country country)
        {
            _student.User.Address.City.ToList()[0].Countries = new List<Country>() {country};

            return this;
        }
    }
}
