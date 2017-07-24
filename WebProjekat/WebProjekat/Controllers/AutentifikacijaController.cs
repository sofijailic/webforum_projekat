using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebProjekat.Models;

namespace WebProjekat.Controllers
{
    public class AutentifikacijaController : ApiController
    {
        [HttpPost]
        [ActionName("Registracija")]
        public bool Registracija([FromBody]Korisnik k)
        {

            return true;
        }
    }
}
