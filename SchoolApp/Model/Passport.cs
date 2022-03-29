using System;

namespace SchoolApp.Model
{
    public class Passport
    {
        public int ID { get; set; }
        public string PassportSerial { get; set; }
        public string PassportNumber { get; set; }
        public System.DateTime DateBirth { get; set; }

        public Passport()
        {
            DateBirth = DateTime.Now;
        }
    }
}