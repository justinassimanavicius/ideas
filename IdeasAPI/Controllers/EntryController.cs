using IdeasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IdeasAPI.Controllers
{
    public class EntryController : ApiController
    {
        [HttpGet]
        [Route("api/entry/statuses")]
        public IHttpActionResult GetStatuses()
        {
            return Ok();
        }
    }
}
