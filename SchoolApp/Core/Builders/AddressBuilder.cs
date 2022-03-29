using SchoolApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Core.Builders
{
    public class AddressBuilder : StudentBuilder
    {
        public AddressBuilder(Student student) : base(student)
        {
            student.User.Address = new Address();

        }
        public AddressBuilder WithID(int id)
        {
            _student.User.Address.ID = id;
            return this;
        }

        public AddressBuilder WithAddressName(string addressName)
        {
            _student.User.Address.AddressName = addressName;
            return this;
        }

        public AddressBuilder WithAddressNumber(string addressNumber)
        {
            _student.User.Address.AddressNumber = addressNumber;
            return this;
        }
    }
}
