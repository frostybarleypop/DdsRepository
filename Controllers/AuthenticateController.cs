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
                       
        // POST: api/Authenticate
        [HttpPost]
        public IActionResult Post([FromBody] Credentials creds)
        {
            if (creds.username.Equals(creds.password, StringComparison.InvariantCultureIgnoreCase))
            {
                return Ok(new { value = $"{creds.username}:{creds.password}" });
            }
            return BadRequest(new { Error = "Invalid Credentials" });
        }
               
    }
}
