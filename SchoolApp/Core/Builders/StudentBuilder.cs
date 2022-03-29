using SchoolApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Core.Builders
{
    public class StudentBuilder
    {
        protected Student _student;

        public StudentBuilder() => _student = new Student();

        public StudentBuilder(Student student) => _student = student;

        public Student Build() => _student;

        public UserBuilder UserBuilder => new UserBuilder(_student);
        public AddressBuilder AddressBuilder => new AddressBuilder(_student);
        public CityBuilder CityBuilder => new CityBuilder(_student);
        public CountryBuilder CountryBuilder => new CountryBuilder(_student);

    }
}
