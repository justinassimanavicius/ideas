using System.Data.Entity;
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
    public class EntryController : ApiController
    {
        private readonly IdeasDb _db = new IdeasDb();

		[HttpGet]
		[Route("api/entry")]
		public IHttpActionResult Get()
		{
		    List<Entry> entries = _db.Entries.Include("Votes").Include("Comments").ToList();

		    if (!entries.Any()) return NotFound();

		    List<EntryView> result = entries.Select(x => new EntryView
		    {
		        Id = x.Id,
		        Author = UserContext.GetUserInformationByUserName(User.Identity, UserHelper.GetUserNameFromComplexUsername(x.Author)).Name,
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
            var item = _db.Entries.Include("Votes").Include("Comments").SingleOrDefault(x => x.Id == id);

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
                Vote = EntryHelper.GetVotes(item.Votes),
                VoteResult = EntryHelper.UserVoteResult(User.Identity, item.Votes),
                Comments = item.Comments != null ? item.Comments.Count : 0,
                Status = item.Status.GetEnumDescription(),
                CreateDate = item.CreateDate,
                UpdateDate = item.UpdateDate
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

        [HttpGet]
        [Route("api/entry/{entryid}/comment")]
        [ResponseType(typeof(List<CommentView>))]
        public async Task<IHttpActionResult> GetEntryComments(int entryid)
        {
            var comments = await _db.Comments.Include("Entry").ToListAsync();

            if (comments == null) return NotFound();

            return Ok(comments.Where(x => x.Entry.Id == entryid).Select(x => new CommentView
            {
                Id = x.Id,
                Author = UserContext.GetUserInformationByUserName(User.Identity, UserHelper.GetUserNameFromComplexUsername(x.Author)).Name,
                CreateDate = x.CreateDate,
                UpdateDate = x.UpdateDate,
                Message = x.Message
            }));
        }

        [HttpGet]
        [Route("api/entry/{entryId}/comment/{commentId}")]
        [ResponseType(typeof(CommentView))]
        public async Task<IHttpActionResult> GetEntryComment(int entryId, int commentId)
        {
            var comment = await _db.Comments.Include("Entry").SingleOrDefaultAsync(x => x.Id == commentId && x.Entry.Id == entryId);

            if (comment == null) return NotFound();

            return Ok(new CommentView
            {
                Id = comment.Id,
                Author = UserContext.GetUserInformationByUserName(User.Identity, UserHelper.GetUserNameFromComplexUsername(comment.Author)).Name,
                CreateDate = comment.CreateDate,
                UpdateDate = comment.UpdateDate,
                Message = comment.Message
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
