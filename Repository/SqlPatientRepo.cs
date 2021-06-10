using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace DDSPatient.Repository
{
    public class SqlPatientRepo : IPatientRepository
    {
        readonly string SqlConnectionString;
        public SqlPatientRepo()
        {
            SqlConnectionString = "Server = tcp:webproducts.database.windows.net,1433; Initial Catalog = Dds; Persist Security Info = False; User ID = websa; Password = Poi890890; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";

        }

        public Patient CreatePatient(Patient value)
        {

            throw new NotImplementedException();
        }

        public void DeletePatient(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Patient>> GetAllPatients()
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
            {                
                var visits = await connection.QueryAsync<Visit>("select id, customerid, visitdate, notes from visits");

                //const string sql = "select customerid as id, FirstName, LastName, 'https://res.cloudinary.com/mtree/image/upload/f_auto,q_auto,f_jpg,fl_attachment:ce532-fig04-number-system-adult/dentalcare/%2F-%2Fmedia%2Fdentalcareus%2Fprofessional-education%2Fce-courses%2Fcourse0501-0600%2Fce532%2Fimages%2Fce532-fig04-number-system-adult.jpg%3Fh%3D494%26la%3Den-us%26w%3D691%26v%3D1-201706011336?h=494&la=en-US&w=691' as imageUrl from SalesLT.Customer where customerid in @Ids";
                const string sql = "select c.customerid as id, FirstName, LastName, isnull(s.url,'https://juddimageblob.blob.core.windows.net/dds-records/ce532-fig04-number-system-adult.jpg') as imageurl, s.scandate  from SalesLT.Customer c left join scans s on c.CustomerID = s.customerid where c.customerid in @Ids";
                var parameters = new DynamicParameters();
                parameters.Add("@Ids", visits.Select(p => p.CustomerId).Distinct());

                var patients = (await connection.QueryAsync<Patient>(sql, parameters)).ToList();
               
               
                patients.ForEach(p => p.Visits = visits.Where(v => v.CustomerId == p.Id).ToList());
                return patients;
            }
        }

        public Patient GetPatient(int id)
        {
            throw new NotImplementedException();
        }

        public Patient UpdatePatient(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}
