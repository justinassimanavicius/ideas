using IdeasAPI.Helpers;
using IdeasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using IdeasAPI.DataContexts;

namespace IdeasAPI.Controllers
{
    public class EntryController : ApiController
    {
        private readonly IdeasDb _db = new IdeasDb();

		[HttpGet]
		[Route("api/entry")]
		public IHttpActionResult Get()
		{
			return Ok(_db.Entries.ToList());
		}

		[HttpGet]
		[Route("api/entry/{id}")]
		public IHttpActionResult Get(int id)
		{
            var item = _db.Entries.Find(id);

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
                entry.Author = UserHelper.GetUserNameFromIdentity(User.Identity);
                entry.Priority = EntryPriority.Minor;
                entry.SecurityLevel = new List<UserGroup> { UserGroup.None };
                entry.Source = EntrySource.Web;
                entry.Status = EntryStatus.Open;

                var item = _db.Entries.Add(entry);
                _db.SaveChanges();

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
