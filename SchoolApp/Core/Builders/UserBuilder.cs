using SchoolApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Core.Builders
{
    public class UserBuilder : StudentBuilder
    {
        public UserBuilder(Student student) : base(student)
        {
            student.User = new User();
        }
        public PassportBuilder PassportBuilder => new PassportBuilder(_student);

        public UserBuilder WithID(Guid id)
        {
            _student.User.ID = id;
            return this;
        }

        public UserBuilder WithFullName(string[] fullName)
        {
            if (fullName[2] == null)
            {
                _student.User.FirstName = fullName[0];
                _student.User.SecondName = fullName[1];
            }
            else
            {
                _student.User.FirstName = fullName[0];
                _student.User.SecondName = fullName[1];
                _student.User.LastName = fullName[2];
            }

            return this;
        }

        public UserBuilder WithEmail(string email)
        {
            _student.User.Email = email;
            return this;
        }

        public UserBuilder WithPassword(string password)
        {
            _student.User.Password = password;
            return this;
        }

        public UserBuilder WithCode(string code)
        {
            _student.User.Code = code;
            return this;
        }
    }
}
