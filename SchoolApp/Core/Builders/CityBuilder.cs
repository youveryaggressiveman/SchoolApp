using SchoolApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Core.Builders
{
    public class CityBuilder : StudentBuilder
    {
        public CityBuilder(Student student) : base(student)
        {
            student.User.Address.City = new List<City>();
        }
        public CityBuilder WithCity(City city)
        {
            _student.User.Address.City = new List<City>() { city };
            return this;
        }
    }
}
