using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOps_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OneAPIController : ControllerBase
    {

        OneMemory memory;
        public OneAPIController(IOptions<OneMemory> options)
        {
            memory = options.Value;
        }

        // GET: api/<OneAPIController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(memory);
        }

        // GET api/<OneAPIController>/5
        [HttpGet, Route("get/{id}")]
        public IActionResult Get(int id)
        {
            OneModel oneModels = memory.UsersList.Where(o => o.UserId == id).FirstOrDefault();
            if (oneModels != null)
            {
                return Ok(oneModels);
            }
            else
            {
                return BadRequest();
            }
        }

        // POST api/<OneAPIController>
        [HttpPost]
        public IActionResult Post([FromBody] OneModel value)
        {
            try
            {
                memory.UsersList.Add(value);
                return Ok(value.UserId);
            }
            catch (Exception)
            {
                return BadRequest();

            }

        }

        // PUT api/<OneAPIController>/5
        [HttpPut, Route("get/{id}")]
        public IActionResult Put(int id, [FromBody] OneModel value)
        {
            try
            {
                OneModel oneModels = memory.UsersList.Where(o => o.UserId == id).FirstOrDefault();
                if (oneModels != null)
                {
                    oneModels.UserName = value.UserName;
                    oneModels.EmailAddress = value.EmailAddress;
                    oneModels.PhoneNo = value.PhoneNo;

                    return Ok(id);
                }
                return BadRequest();
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

        // DELETE api/<OneAPIController>/5
        [HttpDelete, Route("get/{id}")]
        public IActionResult Delete(int id)
        {
            OneModel oneModels = memory.UsersList.Where(o => o.UserId == id).FirstOrDefault();
            if (oneModels != null)
            {
                memory.UsersList.Remove(oneModels);
                return Ok(id);
            }

            return BadRequest();
        }
    }
}
