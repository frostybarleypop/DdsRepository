using System;
using System.Collections.Generic;

namespace DDSPatient
{
    public class Patient
    {
        //todo: add required attribites for model validation
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public List<Visit> Visits {get;set;}
    }

    public class Visit
    {
        //todo: add required attribites for model validation
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime VisitDate { get; set; }
        public string Notes { get; set; }
    }
}
