﻿using System;
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

            var entry = _db.Entries.Find(entryId);

            comment.CreateDate = DateTime.Now;
            comment.Author = UserHelper.GetUserNameFromIdentity(User.Identity);
            comment.Entry = entry;

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            var location = String.Format("api/entry/{0}/comment/{1}", comment.Entry.Id, comment.Id);

            return Created(location, new CommentView
            {
                Id = comment.Id,
                Author = UserContext.GetUserInformationByUserName(User.Identity, UserHelper.GetUserNameFromComplexUsername(comment.Author)).Name,
                CreateDate = comment.CreateDate,
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

            if (comment.Author != UserContext.GetUserInformation(User.Identity).DomainName)
            {
                return Unauthorized();
            }

            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();

            return Ok(new CommentView
            {
                Id = comment.Id,
                Author = UserContext.GetUserInformationByUserName(User.Identity, UserHelper.GetUserNameFromComplexUsername(comment.Author)).Name,
                CreateDate = comment.CreateDate,
                Message = comment.Message,
                UpdateDate = comment.UpdateDate
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