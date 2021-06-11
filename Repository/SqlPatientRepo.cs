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

        public async Task<Patient> CreatePatient(Patient value)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("firstname", value.FirstName);
                parameters.Add("lastname", value.LastName);
                parameters.Add("newId", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                //using a stored proc here mostly to get the data created and the patient ID out in one go.
                var affectedRows = await connection.QueryAsync<int>("CreatePatient", parameters, commandType: System.Data.CommandType.StoredProcedure);
                int newId = parameters.Get<int>("newId");
                if (affectedRows.Any() && affectedRows.Count() > 0 && newId > 0)
                {
                    if (value.Visits != null && value.Visits.Any()) //visits are foreign keyed to patients and need to be added after.
                    {
                        // add new visits - should be refactored to a method, used multiple times now.
                        value.Visits.ForEach(newvisit =>
                        {
                            const string visitsql = "insert into visits (notes, visitdate, customerid) values (@notes,@visitdate,@Id)";
                            var parms = new DynamicParameters();
                            parms.Add("@Id", newId);
                            parms.Add("@notes", newvisit.Notes);
                            parms.Add("@visitdate", newvisit.VisitDate);
                            connection.Query<Visit>(visitsql, parms);
                        });
                    }

                    return await GetPatient(newId);

                }
                throw new Exception("Unable to create new patient");
            }
        }

        public async Task<int> DeletePatient(Patient value)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", value.Id);
                
                //visits are foreign keyed to patients and need to be deleted first.
                if (value.Visits != null && value.Visits.Any())
                {
                    parameters.Add("@Ids", value.Visits.Select(v => v.Id).ToList());
                    connection.Execute("delete from visits where id in @Ids", parameters);
                }

                const string sql = "delete from patients where id = @Id";

                return await connection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<List<Patient>> GetAllPatients()
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
            {
                var visits = await connection.QueryAsync<Visit>("select id, customerid, visitdate, notes from visits");
                
                //this should probably be a stored proc for efficency.
                const string sql = "select p.id, FirstName, LastName,s.url as imageurl, s.scandate  from patients p left join scans s on p.id = s.customerid where p.id in @Ids";
                var parameters = new DynamicParameters();
                parameters.Add("@Ids", visits.Select(p => p.CustomerId).Distinct());

                var patients = (await connection.QueryAsync<Patient>(sql, parameters)).ToList();

                patients.ForEach(p => p.Visits = visits.Where(v => v.CustomerId == p.Id).ToList());
                return patients;
            }
        }

        public async Task<Patient> GetPatient(int id)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
            {
                const string sql = "select p.id, FirstName, LastName, s.url as imageurl, s.scandate  from patients p left join scans s on p.id = s.customerid where p.id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                var patients = (await connection.QueryAsync<Patient>(sql, parameters)).FirstOrDefault();
                if (patients is null)
                {
                    return null;
                }
                var visits = await connection.QueryAsync<Visit>("select id, customerid, visitdate, notes from visits where customerid = @Id", parameters);

                patients.Visits = visits.ToList();
                return patients;
            }
        }

        public async Task<Patient> UpdatePatient(Patient patient)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
            {
                const string sql = "update patients set firstname=@first, lastname=@last where id=@id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", patient.Id);
                parameters.Add("@first", patient.FirstName);
                parameters.Add("@last", patient.LastName);

                var patients = (await connection.QueryAsync<Patient>(sql, parameters)).FirstOrDefault();

                if (patient.Visits != null && patient.Visits.Any(v => v.Id > 0))
                {
                    //update visits
                    patient.Visits.Where(v => v.Id > 0).ToList().ForEach(newvisit =>
                      {
                          const string visitsql = "update visits set notes=@notes, visitdate=@visitdate where customerid=@Id";
                          var parms = new DynamicParameters();
                          parms.Add("@Id", patient.Id);
                          parms.Add("@notes", newvisit.Notes);
                          parms.Add("@visitdate", newvisit.VisitDate);
                          connection.Query<Visit>(visitsql, parms);
                      });

                }

                if (patient.Visits != null && patient.Visits.Any(v => v.Id == 0))
                {
                    // add new visits
                    patient.Visits.Where(v => v.Id == 0).ToList().ForEach(newvisit =>
                   {
                       const string visitsql = "insert into visits (notes, visitdate, customerid) values (@notes,@visitdate,@Id)";
                       var parms = new DynamicParameters();
                       parms.Add("@Id", patient.Id);
                       parms.Add("@notes", newvisit.Notes);
                       parms.Add("@visitdate", newvisit.VisitDate);
                       connection.Query<Visit>(visitsql, parms);
                   });
                }

                return await GetPatient(patient.Id);
            }
        }


    }
}
