using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolApp.Model
{
    public class User
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Code { get; set; }
        [JsonIgnore]
        public string FIO => SecondName + " " + FirstName[0] + "." + (LastName != null ? LastName[0] + "." : "");

        public int AddressID { get; set; }
        public int PassportID { get; set; }
        public int RoleID { get; set; }

        public Address Address { get; set; }
        public Passport Passport { get; set; }
        public Role Role { get; set; }
        
    }
}
