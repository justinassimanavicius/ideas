using System.Runtime.InteropServices;
using IdeasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IdeasAPI.DataContexts;

namespace IdeasAPI.Controllers
{
    public class EntryController : ApiController
    {
        private readonly IdeasDb db = new IdeasDb();

		[HttpGet]
		[Route("api/entry")]
		public IHttpActionResult Get()
		{
			return Ok(db.Entries.ToList());
		}

		[HttpGet]
		[Route("api/entry/{id}")]
		public IHttpActionResult Get(int id)
		{
            var item = db.Entries.Find(id);

			if (item == null)
			{
				return NotFound();
			}

			return Ok(item);
		}

        [Authorize]
		[Route("api/entry")]
		public IHttpActionResult Post(Entry entry)
		{
            try
            {
                entry.CreateDate = DateTime.Now;
                entry.Author = User.Identity.Name;
                entry.Priority = EntryPriority.Minor;
                entry.SecurityLevel = new List<UserGroup> { UserGroup.None };
                entry.Source = EntrySource.Web;
                entry.Status = EntryStatus.Open;

                var item = db.Entries.Add(entry);
                db.SaveChanges();

                var location = String.Format("api/entry/{0}", item.Id);
                return Created(location, entry);
            }
            catch (Exception)
            {
                return NotFound();
            }
		}
    }
}
