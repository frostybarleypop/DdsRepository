using System;
using System.Collections.Generic;

namespace DDSPatient
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public List<Visit> Visits {get;set;}
    }

    public class Visit
    {
        public DateTime VisitDate { get; set; }
        public string Notes { get; set; }
    }
}
