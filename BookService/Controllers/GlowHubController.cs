using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BookService.Models;

namespace BookService.Controllers
{
    public class GlowHubController : ApiController
    {
        private BookServiceContext db = new BookServiceContext();

        // GET: api/GlowHub
        public IQueryable<GlowHub> GetGlowHubs()
        {
            return db.GlowHubs;
        }

        // GET: api/GlowHubs/5
        [ResponseType(typeof(GlowHub))]
        public async Task<IHttpActionResult> GetGlowHub(int id)
        {
            GlowHub glowhub = await db.GlowHubs.FindAsync(id);
            if (glowhub == null)
            {
                return NotFound();
            }

            return Ok(glowhub);
        }

        // PUT: api/GlowHubs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGlowHub(int id, GlowHub glowhub)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != glowhub.Id)
            {
                return BadRequest();
            }

            db.Entry(glowhub).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GlowHubExists(id))
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

        // POST: api/GlowHub
        [ResponseType(typeof(GlowHub))]
        public async Task<IHttpActionResult> PostGlowHub(GlowHub glowhub)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GlowHubs.Add(glowhub);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = glowhub.Id }, glowhub);
        }

        // DELETE: api/GlowHubs/5
        [ResponseType(typeof(GlowHub))]
        public async Task<IHttpActionResult> DeleteGlowHub(int id)
        {
            GlowHub glowhub = await db.GlowHubs.FindAsync(id);
            if (glowhub == null)
            {
                return NotFound();
            }

            db.GlowHubs.Remove(glowhub);
            await db.SaveChangesAsync();

            return Ok(glowhub);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GlowHubExists(int id)
        {
            return db.GlowHubs.Count(e => e.Id == id) > 0;
        }
    }
}