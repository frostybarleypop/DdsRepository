using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDSPatient.Repository
{
   public interface IPatientRepository
    {
        List<Patient> GetAllPatients();
        Patient GetPatient(int id);
        Patient UpdatePatient(Patient patient);
        void DeletePatient(int id);
        Patient CreatePatient(Patient value);
    }
}
