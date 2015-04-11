using IdeasAPI.Code;
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
		    List<Entry> entries = _db.Entries.ToList();

		    if (!entries.Any()) return NotFound();

		    List<EntryView> result = entries.Select(x => new EntryView
		    {
		        Id = x.Id,
		        Author = UserContext.GetUserInformationByUserName(User.Identity, UserHelper.GetUserNameFromComplexUsername(x.Author)).Name,
		        Title = x.Title,
		        Message = x.Message,
		        Vote = EntryHelper.GetVotes(x.Upvoters, x.Downvoters),
                Comments = x.Comments != null ? x.Comments.Count : 0,
		        Status = x.Status.GetEnumDescription()
		    }).ToList();

		    return Ok(result);
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

            var result = new EntryView
            {
                Id = item.Id,
                Author = UserContext.GetUserInformationByUserName(User.Identity, UserHelper.GetUserNameFromComplexUsername(item.Author)).Name,
                Title = item.Title,
                Message = item.Message,
                Vote = EntryHelper.GetVotes(item.Upvoters, item.Downvoters),
                Comments = item.Comments != null ? item.Comments.Count : 0,
                Status = item.Status.GetEnumDescription()
            };

			return Ok(result);
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
