using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDSPatient.Repository
{
    public class MockPatientRepo : IPatientRepository
    {
        public Task<Patient> CreatePatient(Patient value)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeletePatient(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Patient>> GetAllPatients()
        {
            List<Patient> patients = new List<Patient>();
            for (int i = 0; i < 10; i++)
            {
                Patient patient = new Patient();
                patient.FirstName = $"{i} FirstName";
                patient.LastName = $"LastName {i}";
                patient.Id = i;
                patient.ImageUrl = "https://res.cloudinary.com/mtree/image/upload/f_auto,q_auto,f_jpg,fl_attachment:ce532-fig04-number-system-adult/dentalcare/%2F-%2Fmedia%2Fdentalcareus%2Fprofessional-education%2Fce-courses%2Fcourse0501-0600%2Fce532%2Fimages%2Fce532-fig04-number-system-adult.jpg%3Fh%3D494%26la%3Den-us%26w%3D691%26v%3D1-201706011336?h=494&la=en-US&w=691";
                patient.Visits = new List<Visit> { new Visit { VisitDate = DateTime.Now.AddDays(i), Notes = $"Note #:{i}" } };
                patients.Add(patient);
            }
            return  patients;
        }

        public Task<Patient> GetPatient(int id)
        {

            List<Patient> patients = new List<Patient>();
            for (int i = 0; i < 10; i++)
            {
                Patient patient = new Patient();
                patient.FirstName = $"{i} FirstName";
                patient.LastName = $"LastName {i}";
                patient.Id = i;
                patient.ImageUrl = "https://res.cloudinary.com/mtree/image/upload/f_auto,q_auto,f_jpg,fl_attachment:ce532-fig04-number-system-adult/dentalcare/%2F-%2Fmedia%2Fdentalcareus%2Fprofessional-education%2Fce-courses%2Fcourse0501-0600%2Fce532%2Fimages%2Fce532-fig04-number-system-adult.jpg%3Fh%3D494%26la%3Den-us%26w%3D691%26v%3D1-201706011336?h=494&la=en-US&w=691";
                patient.Visits = new List<Visit> { new Visit { VisitDate = DateTime.Now.AddDays(i), Notes = $"Note #:{i}" } };
                patients.Add(patient);
            }
            return new Task<Patient>(()=>patients.FirstOrDefault(x => x.Id == id));
        }

        public Task<Patient> UpdatePatient(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}
