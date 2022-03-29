using SchoolApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Core.Builders
{
    public class PassportBuilder : StudentBuilder
    {
        public PassportBuilder(Student student) : base(student) 
        {
            student.User.Passport = new Passport();
        }

        public PassportBuilder WithID(int id)
        {
            _student.User.Passport.ID = id;
            return this;
        }

        public PassportBuilder WithPassportNumber(string passportNumber)
        {
            _student.User.Passport.PassportNumber = passportNumber;
            return this;
        }

        public PassportBuilder WithPassportSerial(string passportSerial)
        {
            _student.User.Passport.PassportSerial = passportSerial;
            return this;
        }

        public PassportBuilder WithDateBirth(DateTime dateBirth)
        {
            _student.User.Passport.DateBirth = dateBirth;
            return this;
        }
    }
}
