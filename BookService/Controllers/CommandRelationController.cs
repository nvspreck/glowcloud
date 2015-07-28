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
    public class CommandRelationsController : ApiController
    {
        private BookServiceContext db = new BookServiceContext();

        // GET: api/CommandRelations
        public IQueryable<CommandRelation> GetCommandRelations()
        {
            return db.CommandRelations;
        }

        // GET: api/CommandRelations/5
        [ResponseType(typeof(CommandRelation))]
        public async Task<IHttpActionResult> GetCommandRelation(int id)
        {
            CommandRelation CommandRelation = await db.CommandRelations.FindAsync(id);
            if (CommandRelation == null)
            {
                return NotFound();
            }

            if (CommandRelation.NextCommandId != 0)
            {
                CommandRelation NextCommandRelation = await db.CommandRelations.FindAsync(CommandRelation.NextCommandId);
                GlowHub gh = await db.GlowHubs.FindAsync(CommandRelation.GlowHubId);

                gh.CommandRelationId = NextCommandRelation.Id;
                db.SaveChanges();
            }

            return Ok(CommandRelation);
        }

        // PUT: api/CommandRelations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCommandRelation(int id, CommandRelation CommandRelation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != CommandRelation.Id)
            {
                return BadRequest();
            }

            db.Entry(CommandRelation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandRelationExists(id))
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

        // POST: api/CommandRelations
        [ResponseType(typeof(CommandRelation))]
        public async Task<IHttpActionResult> PostCommandRelation(CommandRelation CommandRelation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        

            //Check to see if the user has an exisiting command for the glow
            List<CommandRelation> commandList = db.CommandRelations.SqlQuery("select * from CommandRelations where UserId=" + CommandRelation.UserId + " and GlowHubId=" + CommandRelation.GlowHubId + ";").ToList();
            if (commandList.Count() == 1)
            {
                // User has a command update it
                commandList.First().Command = CommandRelation.Command;
            }
            //The user does not have a command. Insert there command into the glow q.
            else if (commandList.Count() == 0)
            {
                CommandRelation = db.CommandRelations.Add(CommandRelation);

                await db.SaveChangesAsync();

                List<CommandRelation> exitingCommands = db.CommandRelations.SqlQuery("select * from CommandRelations where GlowHubId=" + CommandRelation.GlowHubId + ";").ToList();
                // The glow does not have a q making a new one
                if (exitingCommands.Count() == 1)
                {
                    CommandRelation.NextCommandId = CommandRelation.Id;
                }
                //The glow has a q add it to it.
                else
                {
                    CommandRelation first = exitingCommands[0];

                    CommandRelation.NextCommandId = first.NextCommandId;
                    first.NextCommandId = CommandRelation.Id;
                    //db.CommandRelations.Add(first);
                    await db.SaveChangesAsync();
                }
            }


            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = CommandRelation.Id }, CommandRelation);
        }

        // DELETE: api/CommandRelations/5
        [ResponseType(typeof(CommandRelation))]
        public async Task<IHttpActionResult> DeleteCommandRelation(int id)
        {
            CommandRelation CommandRelation = await db.CommandRelations.FindAsync(id);
            if (CommandRelation == null)
            {
                return NotFound();
            }

            db.CommandRelations.Remove(CommandRelation);
            await db.SaveChangesAsync();

            return Ok(CommandRelation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommandRelationExists(int id)
        {
            return db.CommandRelations.Count(e => e.Id == id) > 0;
        }
    }
}