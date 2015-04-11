using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IdeasAPI.Code;
using IdeasAPI.Helpers;
using IdeasAPI.Models;
using IdeasAPI.DataContexts;

namespace IdeasAPI.Controllers
{
    [Authorize]
    public class VoteController : ApiController
    {
        private readonly IdeasDb _db = new IdeasDb();

        // POST api/Vote
        [ResponseType(typeof(Vote))]
        [Route("api/entry/{id}/vote")]
		[HttpPost]
        public async Task<IHttpActionResult> PostVote(int id, VoteView voteView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entry = _db.Entries.Include("Votes").SingleOrDefault(x => x.Id == id);

            if (entry == null) return NotFound();

            var userVoteResult = EntryHelper.UserVoteResult(User.Identity, entry.Votes);

            if (userVoteResult != null) return Unauthorized();

            var vote = new Vote
            {
                Author = UserHelper.GetUserNameFromIdentity(User.Identity),
                CreateDate = DateTime.Now,
                Entry = entry,
                IsPositive = voteView.IsPositive
            };

            _db.Votes.Add(vote);
            await _db.SaveChangesAsync();

            var location = string.Format("api/entry/{0}", vote.Entry.Id);
			return Created(location, new VoteView
			{
				Id = vote.Id,
				Author = UserContext.GetUserInformationByUserName(User.Identity, UserHelper.GetUserNameFromComplexUsername(vote.Author)).Name,
				CreateDate = vote.CreateDate,
				IsPositive = voteView.IsPositive
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

        private bool VoteExists(int id)
        {
            return _db.Votes.Count(e => e.Id == id) > 0;
        }
    }
}