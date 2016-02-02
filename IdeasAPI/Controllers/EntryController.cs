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
        private readonly UserContext _userContext;

        public EntryController(UserContext userContext)
        {
            _userContext = userContext;
        }

        [HttpGet]
		[Route("api/entry")]
		public IHttpActionResult Get()
		{
		    List<Entry> entries = _db.Entries.Include(x=>x.Votes).Include(x=>x.Comments).ToList();

		    if (!entries.Any(x => x.Status != EntryStatus.Trash && x.Visibility != EntryVisibility.Hidden)) return Ok(new List<int>());

            var user = _userContext.GetUser(UserHelper.GetUserNameFromIdentity(User.Identity));

		    if (!user.IsModerator)
		        entries = entries.Where(
		            x =>
		                x.Status != EntryStatus.AwaitingApproval || (x.Status == EntryStatus.AwaitingApproval &&
		                UserHelper.GetUserNameFromComplexUsername(x.Author) ==
		                UserHelper.GetUserNameFromIdentity(User.Identity))).ToList();

            List<EntryView> result = entries
                .Where(x => x.Status != EntryStatus.Trash && x.Visibility != EntryVisibility.Hidden)
                .Select(x =>
                {
                    var author = _userContext.GetUser(UserHelper.GetUserNameFromComplexUsername(x.Author));
                    return new EntryView
                    {
                        Id = x.Id,
                        Author = author.Name,
                        Title = x.Title,
                        Message = x.Message,
                        Vote = EntryHelper.GetVotes(x.Votes),
                        VoteResult = EntryHelper.UserVoteResult(User.Identity, x.Votes),
                        Comments = x.Comments != null ? x.Comments.Count : 0,
                        Status = x.Status.GetEnumDescription(),
                        CreateDate = x.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                        UpdateDate = x.UpdateDate.HasValue ? x.UpdateDate.Value.ToString("yyyy-MM-dd HH:mm") : ""
                    };
                }).ToList();

		    return Ok(result);
		}

		[HttpGet]
		[Route("api/entry/{id}")]
		public IHttpActionResult Get(int id)
		{
            var item = _db.Entries.Include(x => x.Votes).Include(x => x.Comments).SingleOrDefault(x => x.Id == id && x.Status != EntryStatus.Trash && x.Visibility != EntryVisibility.Hidden);

			if (item == null)
			{
				return NotFound();
			}

            var result = new EntryView
            {
                Id = item.Id,
                Author = _userContext.GetUser(UserHelper.GetUserNameFromComplexUsername(item.Author)).Name,
                Title = item.Title,
                Message = item.Message,
                Vote = EntryHelper.GetVotes(item.Votes),
                VoteResult = EntryHelper.UserVoteResult(User.Identity, item.Votes),
                Comments = item.Comments != null ? item.Comments.Count : 0,
                Status = item.Status.GetEnumDescription(),
                CreateDate = item.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                UpdateDate = item.UpdateDate.HasValue ? item.UpdateDate.Value.ToString("yyyy-MM-dd HH:mm") : ""
            };

			return Ok(result);
		}

        [HttpPost]
		[Route("api/entry")]
		public IHttpActionResult Post(Entry entry)
		{
            try
            {
                var userName = UserHelper.GetUserNameFromIdentity(User.Identity);
                var user = _userContext.GetUser(userName);
                entry.CreateDate = DateTime.Now;
                entry.Author = UserHelper.GetUserNameFromIdentity(User.Identity);
                entry.Priority = EntryPriority.Minor;
                entry.SecurityLevel = new List<UserGroup> { UserGroup.None };
                entry.Source = EntrySource.Web;
                entry.Status = _userContext.GetUser(UserHelper.GetUserNameFromComplexUsername(entry.Author)).IsModerator ? EntryStatus.Open : EntryStatus.AwaitingApproval;
                entry.Visibility = EntryVisibility.Public;
                entry.UserId = user.Id;

                var item = _db.Entries.Add(entry);
                _db.SaveChanges();

                var location = String.Format("api/entry/{0}", item.Id);
                return Created(location, new EntryView
                {
                    Id = entry.Id,
                    Author = _userContext.GetUser(UserHelper.GetUserNameFromComplexUsername(entry.Author)).Name,
                    Comments = 0,
                    CreateDate = entry.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                    Message = entry.Message,
                    Status = entry.Status.GetEnumDescription(),
                    Title = entry.Title,
                    UpdateDate = entry.UpdateDate.HasValue ? entry.UpdateDate.Value.ToString("yyyy-MM-dd HH:mm") : "",
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

                if (entry.Author != _userContext.GetUser(User.Identity.Name).DomainName)
                {
                    return BadRequest();
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
                    Author = _userContext.GetUser(UserHelper.GetUserNameFromComplexUsername(entry.Author)).Name,
                    Comments = 0,
                    CreateDate = entry.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                    Message = entry.Message,
                    Status = entry.Status.GetEnumDescription(),
                    Visibility = entry.Visibility.GetEnumDescription(),
                    Title = entry.Title,
                    UpdateDate = entry.UpdateDate.HasValue ? entry.UpdateDate.Value.ToString("yyyy-MM-dd HH:mm") : "",
                    Vote = 0,
                    VoteResult = null
                });
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("api/entry/{id}/approve")]
        public IHttpActionResult Approve(int id)
        {
            try
            {
                Entry entry = _db.Entries.Find(id);

                if (entry == null)
                {
                    return NotFound();
                }

                if (!_userContext.GetUser(User.Identity.Name).IsModerator)
                {
                    return BadRequest();
                }

                entry.Status = EntryStatus.Open;

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
                    Author = _userContext.GetUser(UserHelper.GetUserNameFromComplexUsername(entry.Author)).Name,
                    Comments = 0,
                    CreateDate = entry.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                    Message = entry.Message,
                    Status = entry.Status.GetEnumDescription(),
                    Visibility = entry.Visibility.GetEnumDescription(),
                    Title = entry.Title,
                    UpdateDate = entry.UpdateDate.HasValue ? entry.UpdateDate.Value.ToString("yyyy-MM-dd HH:mm") : "",
                    Vote = 0,
                    VoteResult = null
                });
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("api/entry/{id}/disapprove")]
        public IHttpActionResult Disapprove(int id)
        {
            try
            {
                Entry entry = _db.Entries.Find(id);

                if (entry == null)
                {
                    return NotFound();
                }

                if (!_userContext.GetUser(User.Identity.Name).IsModerator)
                {
                    return BadRequest();
                }

                entry.Status = EntryStatus.AwaitingApproval;

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
                    Author = _userContext.GetUser(UserHelper.GetUserNameFromComplexUsername(entry.Author)).Name,
                    Comments = 0,
                    CreateDate = entry.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                    Message = entry.Message,
                    Status = entry.Status.GetEnumDescription(),
                    Visibility = entry.Visibility.GetEnumDescription(),
                    Title = entry.Title,
                    UpdateDate = entry.UpdateDate.HasValue ? entry.UpdateDate.Value.ToString("yyyy-MM-dd HH:mm") : "",
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
