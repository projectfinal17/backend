using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ValuesController : Controller
    {
        private readonly IHostingEnvironment _env;
        public ValuesController(IHostingEnvironment env)
        {
            _env = env;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] {  _env.EnvironmentName , "Hello World" };
        }

        // GET api/values/5
        [Authorize (Roles ="Admin")]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "This is OK";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
