using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DDSPatient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        // GET: api/Authenticate
        [HttpGet(Name = "Auth")]
        public IActionResult Get([FromQuery] string  username)
        {
            return Ok($"{username}");
        }

        //// GET: api/Authenticate/5
        //[HttpGet("{id}", Name = "GetToken")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Authenticate
        [HttpPost]
        public IActionResult Post([FromBody] Credentials creds)
        {
            return Ok(new JsonResult($"{creds.username}:{creds.password}"));
        }

        // PUT: api/Authenticate/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
