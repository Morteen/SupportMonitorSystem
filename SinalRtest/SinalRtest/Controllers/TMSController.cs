using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SinalRtest.Models;

namespace SinalRtest.Controllers
{

   



    [Route("api/[controller]")]
    [ApiController]
    public class TMSController : ControllerBase
    {

        private readonly TMSDbContext db;



        public TMSController(TMSDbContext _context)
        {
            db = _context;
        }


        // GET: api/TMS
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(db.TMS.ToList());
        }

        // GET: api/TMS/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "Hei hei fra ap1";
        }

        // POST: api/TMS
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/TMS/5
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
