using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDSPatient.Repository
{
   public interface IPatientRepository
    {
        Task<List<Patient>> GetAllPatients();
        Task<Patient> GetPatient(int id);
        Task<Patient> UpdatePatient(Patient patient);
        Task<int> DeletePatient(Patient id);
        Task<Patient> CreatePatient(Patient value);
    }
}
