﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IdeasAPI.Code;
using IdeasAPI.Helpers;
using IdeasAPI.Models;
using IdeasAPI.DataContexts;
using System.Data.Entity;

namespace IdeasAPI.Controllers
{
    [Authorize]
    public class VoteController : ApiController
    {
        private readonly IdeasDb _db = new IdeasDb();
        private readonly UserContext _userContext;
        private readonly IMailer _mailer;

        public VoteController(UserContext userContext, IMailer mailer)
        {
            _userContext = userContext;
            _mailer = mailer;
        }

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
			 
            var entry = _db.Entries.Include(x => x.Votes).Include(x => x.User).SingleOrDefault(x => x.Id == id);

            if (entry == null) return NotFound();

            var userVoteResult = EntryHelper.UserVoteResult(User.Identity, entry.Votes);

            if (userVoteResult != null) return BadRequest();

            var userName = UserHelper.GetUserNameFromIdentity(User.Identity);
            var user = _userContext.GetUser(userName);

            var vote = new Vote
            {
                Author = UserHelper.GetUserNameFromIdentity(User.Identity),
                CreateDate = DateTime.Now,
                Entry = entry,
                IsPositive = voteView.IsPositive,
                UserId = user.Id
            };

            _db.Votes.Add(vote);
            await _db.SaveChangesAsync();

            _mailer.InformAboutVote(entry);

            var location = $"api/entry/{vote.Entry.Id}";
			return Created(location, new VoteView
			{
				Id = vote.Id,
                Author = _userContext.GetUser(UserHelper.GetUserNameFromComplexUsername(vote.Author)).Name,
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