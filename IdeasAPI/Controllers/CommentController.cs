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
    public class CommentController : ApiController
    {
        private readonly IdeasDb _db = new IdeasDb();

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

        // PUT api/Comment/5
        public async Task<IHttpActionResult> PutComment(int id, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.Id)
            {
                return BadRequest();
            }

            _db.Entry(comment).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Comment
        [ResponseType(typeof(Comment))]
        public async Task<IHttpActionResult> PostComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = comment.Id }, comment);
        }

        // DELETE api/Comment/5
        [ResponseType(typeof(Comment))]
        public async Task<IHttpActionResult> DeleteComment(int id)
        {
            Comment comment = await _db.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();

            return Ok(comment);
        }

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