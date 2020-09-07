using System;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Url_Shortner.Models;

namespace Url_Shortner.Controllers
{
    public class TelemedicineUrlShortnersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public IHttpActionResult GetIndex()
        {
            return Ok("Chutiyae");
        }

        public IHttpActionResult PostURL(string url)
        {
            try
            {
                // If the url does not contain HTTP prefix it with it
                if (!url.Contains("http"))
                {
                    url = "http://" + url;
                }
                // check if the shortened URL already exists within our database
                var alreadyExists = db.TelemedicineUrlShortners.Where(x => x.longUrl == url).FirstOrDefault();
                if (alreadyExists !=null)
                {
                    return Ok(new
                    {
                        url = url,
                        status = "already shortened",
                    });
                    //return Json(new JsonResult()
                    //{
                    //    url = url,
                    //    status = "already shortened",
                    //    token = null
                    //});
                }
                // Shorten the URL and return the token as a json string
                string token=UrlShortener(url);
                return Json(token);
                // Catch and react to exceptions
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Our Redirect route
        [System.Web.Http.HttpGet]
        public IHttpActionResult NixRedirect(string token)
        {
            throw new NotImplementedException();

        }

        public string GenerateToken()
        {
            string urlsafe = string.Empty;
            Enumerable.Range(48, 75)
              .Where(i => i < 58 || i > 64 && i < 91 || i > 96)
              .OrderBy(o => new Random().Next())
              .ToList()
              .ForEach(i => urlsafe += Convert.ToChar(i)); // Store each char into urlsafe
            return urlsafe.Substring(new Random().Next(0, urlsafe.Length), new Random().Next(2, 6));
        }
        public string UrlShortener(string url)
        {
            // While the token exists in our LiteDB we generate a new one
            // It basically means that if a token already exists we simply generate a new one
            string uniqueToken="";
            bool uniqueTokenCreated = false;
            do
            {
                string newUniqueToken = GenerateToken();
                TelemedicineUrlShortner tokenAlreadyExists = db.TelemedicineUrlShortners.Where(u => u.token == newUniqueToken).FirstOrDefault();
                if(tokenAlreadyExists == null)
                {
                    uniqueToken = newUniqueToken;
                    uniqueTokenCreated = true;
                }
            } while (!uniqueTokenCreated);

            //while (db.TelemedicineUrlShortners.Where(u => u.token == GenerateToken()).FirstOrDefault()!=null) ;
            // Store the values in the NixURL model
            TelemedicineUrlShortner telemedicineShortUrl = new TelemedicineUrlShortner()
            {
                token = uniqueToken,
                longUrl = url,
                shortUrl = uniqueToken
            };
            TelemedicineUrlShortner shortUrlExists = db.TelemedicineUrlShortners.Where(u => u.shortUrl == telemedicineShortUrl.shortUrl).FirstOrDefault();
            if (shortUrlExists != null)
                throw new Exception("URL already exists");
            // Save the NixURL model to  the DB
            db.TelemedicineUrlShortners.Add(telemedicineShortUrl);
            db.SaveChanges();
            return telemedicineShortUrl.shortUrl;
        }
        //public string getNewToken()
        //{
        //    var newToken = GenerateToken();
        //    var exists = db.TelemedicineUrlShortners.Where(u => u.token == newToken).FirstOrDefault();
        //    if (exists == null)
        //        return newToken;
        //    else
        //        getNewToken();
        //    return "";
        //}

        // GET: api/TelemedicineUrlShortners
        //public IQueryable<TelemedicineUrlShortner> GetTelemedicineUrlShortners()
        //{
        //    return db.TelemedicineUrlShortners;
        //}

        //// GET: api/TelemedicineUrlShortners/5
        //[ResponseType(typeof(TelemedicineUrlShortner))]
        //public IHttpActionResult GetTelemedicineUrlShortner(long id)
        //{
        //    TelemedicineUrlShortner telemedicineUrlShortner = db.TelemedicineUrlShortners.Find(id);
        //    if (telemedicineUrlShortner == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(telemedicineUrlShortner);
        //}

        //// PUT: api/TelemedicineUrlShortners/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutTelemedicineUrlShortner(long id, TelemedicineUrlShortner telemedicineUrlShortner)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != telemedicineUrlShortner.id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(telemedicineUrlShortner).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TelemedicineUrlShortnerExists(id))
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

        //// POST: api/TelemedicineUrlShortners
        //[ResponseType(typeof(TelemedicineUrlShortner))]
        //public IHttpActionResult PostTelemedicineUrlShortner(TelemedicineUrlShortner telemedicineUrlShortner)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.TelemedicineUrlShortners.Add(telemedicineUrlShortner);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = telemedicineUrlShortner.id }, telemedicineUrlShortner);
        //}

        //// DELETE: api/TelemedicineUrlShortners/5
        //[ResponseType(typeof(TelemedicineUrlShortner))]
        //public IHttpActionResult DeleteTelemedicineUrlShortner(long id)
        //{
        //    TelemedicineUrlShortner telemedicineUrlShortner = db.TelemedicineUrlShortners.Find(id);
        //    if (telemedicineUrlShortner == null)
        //    {
        //        return NotFound();
        //    }

        //    db.TelemedicineUrlShortners.Remove(telemedicineUrlShortner);
        //    db.SaveChanges();

        //    return Ok(telemedicineUrlShortner);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //private bool TelemedicineUrlShortnerExists(long id)
        //{
        //    return db.TelemedicineUrlShortners.Count(e => e.id == id) > 0;
        //}
    }
}