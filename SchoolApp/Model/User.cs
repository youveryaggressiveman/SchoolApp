using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Model
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? LastName { get; set; }
        public string Uuid { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FIO => SecondName + " " + FirstName[0] + "." + (LastName != null ? LastName[0] + "." : "");

        public int AddressId { get; set; }
        public int PassportId { get; set; }
        public int RoleId { get; set; }

        public Address Address { get; set; }
        public Passport Passport { get; set; }
        public Role Role { get; set; }
        
    }
}
