using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http.Description;
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
    [Authorize]
    public class EntryController : ApiController
    {
        private readonly IdeasDb _db = new IdeasDb();

		[HttpGet]
		[Route("api/entry")]
		public IHttpActionResult Get()
		{
		    List<Entry> entries = _db.Entries.Include("Votes").Include("Comments").ToList();

		    if (!entries.Any(x => x.Status != EntryStatus.Trash && x.Visibility != EntryVisibility.Hidden)) return NotFound();

            List<EntryView> result = entries
                .Where(x => x.Status != EntryStatus.Trash && x.Visibility != EntryVisibility.Hidden)
                .Select(x => new EntryView
		    {
		        Id = x.Id,
                Author = UserContext.GetUserInfo(UserHelper.GetUserNameFromComplexUsername(x.Author)).Name,
		        Title = x.Title,
		        Message = x.Message,
		        Vote = EntryHelper.GetVotes(x.Votes),
                VoteResult = EntryHelper.UserVoteResult(User.Identity, x.Votes),
                Comments = x.Comments != null ? x.Comments.Count : 0,
                Status = x.Status.GetEnumDescription(),
                CreateDate = x.CreateDate,
                UpdateDate = x.UpdateDate
		    }).ToList();

		    return Ok(result);
		}

		[HttpGet]
		[Route("api/entry/{id}")]
		public IHttpActionResult Get(int id)
		{
            var item = _db.Entries.Include("Votes").Include("Comments").SingleOrDefault(x => x.Id == id && x.Status != EntryStatus.Trash && x.Visibility != EntryVisibility.Hidden);

			if (item == null)
			{
				return NotFound();
			}

            var result = new EntryView
            {
                Id = item.Id,
                Author = UserContext.GetUserInfo(UserHelper.GetUserNameFromComplexUsername(item.Author)).Name,
                Title = item.Title,
                Message = item.Message,
                Vote = EntryHelper.GetVotes(item.Votes),
                VoteResult = EntryHelper.UserVoteResult(User.Identity, item.Votes),
                Comments = item.Comments != null ? item.Comments.Count : 0,
                Status = item.Status.GetEnumDescription(),
                CreateDate = item.CreateDate,
                UpdateDate = item.UpdateDate
            };

			return Ok(result);
		}

        [HttpPost]
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
                entry.Visibility = EntryVisibility.Public;

                var item = _db.Entries.Add(entry);
                _db.SaveChanges();

                var location = String.Format("api/entry/{0}", item.Id);
                return Created(location, new EntryView
                {
                    Id = entry.Id,
                    Author = UserContext.GetUserInfo(UserHelper.GetUserNameFromComplexUsername(entry.Author)).Name,
                    Comments = 0,
                    CreateDate = entry.CreateDate,
                    Message = entry.Message,
                    Status = entry.Status.GetEnumDescription(),
                    Title = entry.Title,
                    UpdateDate = entry.UpdateDate,
                    Vote = 0,
                    VoteResult = null
                });
            }
            catch (Exception)
            {
                return NotFound();
            }
		}

        [HttpDelete]
        [Route("api/entry/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Entry entry = _db.Entries.Find(id);

                if (entry == null)
                {
                    return NotFound();
                }

                if (entry.Author != UserContext.GetUserInfo(User.Identity.Name).DomainName)
                {
                    return Unauthorized();
                }

                entry.Status = EntryStatus.Trash;
                entry.Visibility = EntryVisibility.Hidden;

                _db.Entry(entry).State = EntityState.Modified;

                try
                {
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(new EntryView
                {
                    Id = entry.Id,
                    Author = UserContext.GetUserInfo(UserHelper.GetUserNameFromComplexUsername(entry.Author)).Name,
                    Comments = 0,
                    CreateDate = entry.CreateDate,
                    Message = entry.Message,
                    Status = entry.Status.GetEnumDescription(),
                    Visibility = entry.Visibility.GetEnumDescription(),
                    Title = entry.Title,
                    UpdateDate = entry.UpdateDate,
                    Vote = 0,
                    VoteResult = null
                });
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EntryExists(int id)
        {
            return _db.Entries.Count(e => e.Id == id) > 0;
        }
    }
}
