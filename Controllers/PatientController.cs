using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDSPatient.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DDSPatient.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _repo;
        public PatientController(IPatientRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Patient
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {

                return Ok( await _repo.GetAllPatients());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        // GET: api/Patient/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var patient = await _repo.GetPatient(id);
                if (patient is null || string.IsNullOrEmpty(patient.FirstName))
                {
                    return NotFound(id);
                }
                return Ok(patient);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST: api/Patient
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Patient value)
        {
            try
            {
                return Ok(await _repo.CreatePatient(value));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        // PUT: api/Patient/5
        [HttpPut("{id}")]
        public async  Task<IActionResult> Put(int id, [FromBody] Patient value)
        {
            try
            {
                var patient = await _repo.GetPatient(value.Id);
                if (patient is null || string.IsNullOrEmpty(patient.FirstName))
                {
                    return NotFound(value);
                }
                return Ok(await _repo.UpdatePatient(value));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var patient = await _repo.GetPatient(id);
                if (patient is null || string.IsNullOrEmpty(patient.FirstName))
                {
                    return NotFound(id);
                }
                return Ok(await _repo.DeletePatient(patient));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
