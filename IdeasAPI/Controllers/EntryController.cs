using System.Runtime.InteropServices;
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
	    private static readonly List<Entry> Items;

		static EntryController()
	    {
		    Items = new List<Entry>
		    {
			    new Entry()
			    {
				    Message = "Lets have a clown for bithdays",
				    Assignee = new User()
				    {
					    Username = "Jack"
				    },
				    Author = new User()
				    {
					    Username = "Jill"
				    },
				    CreateDate = new DateTime(2015, 04, 10),
				    Id = 1,
				    Status = EntryStatus.Open,
				    Visibility = EntryVisibility.Public,
				    Source = EntrySource.Web
			    },

				 new Entry()
			    {
				    Message = "Norėčiau, kad ofise stovėtų akvariumas",
				   
				    Author = new User()
				    {
					    Username = "Jonas"
				    },
				    CreateDate = new DateTime(2015, 4, 1),
				    Id = 2,
				    Status = EntryStatus.InProgress,
				    Visibility = EntryVisibility.Public,
				    Source = EntrySource.Web
			    }
		    };
	    }

        [HttpGet]
        [Route("api/entry/statuses")]
        public IHttpActionResult GetStatuses()
        {
			return Ok(Items);
        }


		[HttpGet]
		[Route("api/entry")]
		public IHttpActionResult Get()
		{
			return Ok(Items);
		}


		[HttpGet]
		[Route("api/entry/{id}")]
		public IHttpActionResult Get(int id)
		{

			var item = Items.FirstOrDefault(x => x.Id == id);

			if (item == null)
			{
				return NotFound();
			}

			return Ok(item);
		}


		[Route("api/entry")]
		public IHttpActionResult Post(Entry entry)
		{

			var id = Items.Max(x => x.Id) + 1;
			entry.Id = id;
			Items.Add(entry);

			var location = String.Format("api/entry/{0}", id);

			return Created(location, entry);
		}
    }
}
