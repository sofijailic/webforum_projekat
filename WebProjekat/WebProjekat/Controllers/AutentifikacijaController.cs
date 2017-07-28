using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
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
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string linija = "";
            while ((linija = sr.ReadLine()) != null)
            {
                string[] splitovano = linija.Split(';');
                if (splitovano[0] == k.KorisnickoIme)
                {
                    stream.Close();
                    sr.Close();
                    return false;
                }
            }
            sr.Close();
            stream.Close();

            k.Uloga = "Korisnik";
            k.DatumRegistracije = DateTime.Now;

            FileStream stream2 = new FileStream(dataFile, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream2);

            sw.WriteLine(k.KorisnickoIme + ";" + k.Lozinka + ";" + k.Uloga + ";" + k.Ime + ";" + k.Prezime + ";" + k.BrTelefona + ";" + k.Email + ";" + k.DatumRegistracije.ToShortDateString() + ";" + "nemaSnimljenihPodforuma" + ";nemaSnimljenihTema" + ";nemaSnimljenihKomentara");
            sw.Close();
            stream2.Close();
            return true;
           
        }

        [HttpPost]
        [ActionName("Logovanje")]
        public Korisnik Logovanje([FromBody]Korisnik k){

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[0] == k.KorisnickoIme && splitter[1] == k.Lozinka)
                {
                    Korisnik kor = new Korisnik();
                    kor.KorisnickoIme = k.KorisnickoIme;
                    kor.Uloga = splitter[2];
                    kor.Ime = splitter[3];
                    kor.Prezime = splitter[4];
                    kor.BrTelefona = splitter[5];
                    kor.Email = splitter[6];
                    kor.DatumRegistracije = DateTime.Parse(splitter[7]);
                    kor.Lozinka = null;
                    sr.Close();
                    stream.Close();
                    return kor;
                }
            }
            sr.Close();
            stream.Close();
            return null;

        }
    }
}
