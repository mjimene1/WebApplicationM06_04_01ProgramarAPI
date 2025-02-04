using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationM06_04_01ProgramarAPI.Models;

namespace WebApplicationM06_04_01ProgramarAPI.Controllers
{
    public class ciclesController : ApiController
    {
        private abpEntities2 db = new abpEntities2();

        // GET: api/cicles
        public IQueryable<cicle> Getcicles()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.cicles;
        }

        // GET: api/cicles/5
        [ResponseType(typeof(cicle))]
        public async Task<IHttpActionResult> Getcicle(int id)
        {
            IHttpActionResult result;
            db.Configuration.LazyLoadingEnabled = false;
            // ParCicle ParCicle = await db.cicles.FindAsync(id);
            cicle cicle = await db.cicles.Include("cursos").Where(c => c.id == id).FirstOrDefaultAsync();
                    
            if (cicle == null)
            {
                result =  NotFound();
            } else
            {
                result = Ok(cicle);
            }


            return result;
        }

        [HttpGet]
        [Route("api/cicles/nom/{ParNom}")]
        public async Task<IHttpActionResult> FindByNom (String ParNom)
        {
            IHttpActionResult result;
            db.Configuration.LazyLoadingEnabled = false;
            List<cicle> cicles = db.cicles.Where(c => c.nom.Contains(ParNom)).ToList();
            return Ok(cicles);

        }


        // PUT: api/cicles/put/5
        [HttpPut]
        [Route("api/cicles/put/{id}")]
        [ResponseType(typeof(cicle))]
        public async Task<IHttpActionResult> put(int id, cicle ParCicle)
        {
            IHttpActionResult result;
            String missatge = "";
            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            }
            else if (id != ParCicle.id)
            {
                result = BadRequest();

            }
            else
            {
                db.Entry(ParCicle).State = EntityState.Modified;
                result = StatusCode(HttpStatusCode.NoContent);
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cicleExists(id))
                    {
                        result = NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                    missatge = Clases.Utils.MissatgeError(sqlException);
                    result = BadRequest(missatge);
                }

            }

            return result;
        }

        // POST: api/cicles/5
        // Prova de que ara es reconeix un Post tot i que el nom del mètode són diferents
        // És a dir estem intentant fer un PUT amb un POST
        // Estaria guai evitar això per tal d'accedir directament amb un PUT i DELETE.
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("api/cicles/putInAPost/{id}")]
        public async Task<IHttpActionResult> UpdateCicle(int id, cicle ParCicle)
        {
            IHttpActionResult result;
            String missatge = "";
            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            }
            else if (id != ParCicle.id)
            {
                result = BadRequest();

            }
            else
            {
                db.Entry(ParCicle).State = EntityState.Modified;
                result = StatusCode(HttpStatusCode.NoContent);
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cicleExists(id))
                    {
                        result = NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                    missatge = Clases.Utils.MissatgeError(sqlException);
                    result = BadRequest(missatge);
                }

            }

            return result;
        }

        // POST: api/cicles
        [ResponseType(typeof(cicle))]
        public async Task<IHttpActionResult> Postcicle(cicle ParCicle)
        {
            IHttpActionResult result;
            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            } else
            {
                String missatge = "";
                try
                {
                    db.cicles.Add(ParCicle);
                    await db.SaveChangesAsync();
                    result = CreatedAtRoute("DefaultApi", new { id = ParCicle.id }, ParCicle);

                } catch (DbUpdateException ex)
                {   
                    SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                    missatge = Clases.Utils.MissatgeError(sqlException);
                    result = BadRequest(missatge);
                }

            }
            return result;
        }

        // DELETE: api/cicles/5
        /* MJC
        * He añadido lo siguiente, dado que el PUT había dejado de funcionar:
        * [HttpDelete]
        * [Route("api/cicles/deleteCicle/{id}")]
        */

        [ResponseType(typeof(cicle))]
        public async Task<IHttpActionResult> Deletecicle(int id)
        {
            IHttpActionResult result;
            cicle cicle = await db.cicles.FindAsync(id);
            if (cicle == null)
            {
                result = NotFound();
            }
            else
            {
                String missatge = "";
                try
                {
                    db.cicles.Remove(cicle);
                    await db.SaveChangesAsync();
                    result = Ok(cicle);
                } catch (DbUpdateException ex)
                {
                    SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                    missatge = Clases.Utils.MissatgeError(sqlException);
                    result = BadRequest(missatge);
                }

            }

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool cicleExists(int id)
        {
            return db.cicles.Count(e => e.id == id) > 0;
        }
    }
}