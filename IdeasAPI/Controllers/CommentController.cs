using System;
using System.Collections.Generic;
using System.Data;
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
    public class CommentController : ApiController
    {
        private readonly IdeasDb _db = new IdeasDb();
        private readonly UserContext _userContext;
        private readonly IMailer _mailer;

        public CommentController(UserContext userContext, IMailer mailer)
        {
            _userContext = userContext;
            _mailer = mailer;
        }

        #region GET

        [HttpGet]
        [Route("api/comment")]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await _db.Comments.ToListAsync());
        }

        [HttpGet]
        [Route("api/comment/{id}")]
        [ResponseType(typeof(Comment))]
        public async Task<IHttpActionResult> GetComment(int id)
        {
            Comment comment = await _db.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpGet]
        [Route("api/entry/{entryid}/comment")]
        [ResponseType(typeof(List<CommentView>))]
        public async Task<IHttpActionResult> GetEntryComments(int entryid)
        {
            var comments = await _db.Comments.Include(x=>x.Entry).ToListAsync();

            if (comments == null) return NotFound();

            return Ok(comments.Where(x => x.Entry.Id == entryid).Select(x =>
            {
                var user = _userContext.GetUser(UserHelper.GetUserNameFromComplexUsername(x.Author));
                return new CommentView
                {
                    Id = x.Id,
                    Author = user.Name,
                    CreateDate = x.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                    UpdateDate = x.UpdateDate?.ToString("yyyy-MM-dd HH:mm") ?? "",
                    Message = x.Message,
                    AuthorThumbnail = user.Thumbnail
                };
            }));
        }

        [HttpGet]
        [Route("api/entry/{entryId}/comment/{commentId}")]
        [ResponseType(typeof(CommentView))]
        public async Task<IHttpActionResult> GetEntryComment(int entryId, int commentId)
        {
            var comment = await _db.Comments.Include(x=>x.Entry).SingleOrDefaultAsync(x => x.Id == commentId && x.Entry.Id == entryId);

            if (comment == null) return NotFound();

            var author = _userContext.GetUser(UserHelper.GetUserNameFromComplexUsername(comment.Author));

            return Ok(new CommentView
            {
                Id = comment.Id,
                Author = author.Name,
                CreateDate = comment.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                UpdateDate = comment.UpdateDate?.ToString("yyyy-MM-dd HH:mm") ?? "",
                Message = comment.Message,
                AuthorThumbnail = author.Thumbnail
            });
        }

        #endregion

        #region POST

        // POST api/Comment
        [HttpPost]
        [Route("api/entry/{entryId}/comment")]
        [ResponseType(typeof(CommentView))]
        public async Task<IHttpActionResult> PostComment(int entryId, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entry = _db.Entries.Include(x => x.User).First(x=>x.Id == entryId);

            var userName = UserHelper.GetUserNameFromIdentity(User.Identity);
            var user = _userContext.GetUser(userName);
            comment.CreateDate = DateTime.Now;
            comment.Author = userName;
            comment.Entry = entry;
            comment.UserId = user.Id;

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            var location = $"api/entry/{comment.Entry.Id}/comment/{comment.Id}";

            _mailer.InformAboutComment(comment);

            return Created(location, new CommentView
            {
                Id = comment.Id,
                Author = _userContext.GetUser(UserHelper.GetUserNameFromComplexUsername(comment.Author)).Name,
                CreateDate = comment.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                Message = comment.Message
            });
        }

        #endregion

        #region DELETE

        // DELETE api/Comment/5
        [ResponseType(typeof(CommentView))]
        public async Task<IHttpActionResult> DeleteComment(int id)
        {
            Comment comment = await _db.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            if (comment.Author != _userContext.GetUser(User.Identity.Name).DomainName)
            {
                return BadRequest();
            }

            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();

            return Ok(new CommentView
            {
                Id = comment.Id,
                Author = _userContext.GetUser(UserHelper.GetUserNameFromComplexUsername(comment.Author)).Name,
                CreateDate = comment.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                Message = comment.Message,
                UpdateDate = comment.UpdateDate.HasValue ? comment.UpdateDate.Value.ToString("yyyy-MM-dd HH:mm") : ""
            });
        }

        #endregion

        #region TODO

        // PUT api/Comment/5
        //public async Task<IHttpActionResult> PutComment(int id, Comment comment)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != comment.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _db.Entry(comment).State = EntityState.Modified;

        //    try
        //    {
        //        await _db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CommentExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(int id)
        {
            return _db.Comments.Count(e => e.Id == id) > 0;
        }
    }
}