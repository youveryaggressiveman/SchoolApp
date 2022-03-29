using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolApp.Model
{
    public class Student
    {
        [JsonIgnore]
        public Guid ID { get; set; }
        public int GroupID { get; set; }
        [JsonIgnore]
        public Guid UserID { get; set; }

        [JsonIgnore]
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }

        public Student()
        {
            User = new User();
            Group = new Group();
        }
    }
}
