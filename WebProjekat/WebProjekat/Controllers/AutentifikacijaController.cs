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
            if (k == null || ( string.IsNullOrEmpty(k.Ime) && string.IsNullOrEmpty(k.Prezime) && string.IsNullOrEmpty(k.KorisnickoIme) && string.IsNullOrEmpty(k.Lozinka) && string.IsNullOrEmpty(k.Uloga) && string.IsNullOrEmpty(k.Email) && string.IsNullOrEmpty(k.BrTelefona)) ) {
                return false;
            }
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

            if (k == null)
            {
                return null;
            }

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

        [HttpGet]
        [ActionName("uzmiKorisnikaNaOsnovuImena")]
        public Korisnik uzmiKorisnikaNaOsnovuImena(string username)
        {
            if (username == null) {
                return null;
            }

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[0] == username)
                {
                    Korisnik kor = new Korisnik();
                    kor.KorisnickoIme = splitter[0];
                    kor.Ime = splitter[3];
                    kor.Prezime = splitter[4];
                    kor.Uloga = splitter[2];
                    kor.Email = splitter[6];
                    kor.BrTelefona = splitter[5];
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


        [HttpGet]
        [ActionName("UzmiSveKorisnikeOsimMene")]
        public List<Korisnik> UzmiSveKorisnikeOsimMene(string username)
        {
            if (username == null)
            {
                return null;
            }
            List<Korisnik> listaKorisnika = new List<Korisnik>();

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[0] != username)
                {
                    Korisnik k = new Korisnik();
                    k.KorisnickoIme = splitter[0];
                    listaKorisnika.Add(k);
                }
            }
            sr.Close();
            stream.Close();
            return listaKorisnika;
        }

        [HttpPost]
        [ActionName("PromeniTipKorisniku")]
        public bool PromeniTipKorisniku([FromBody]Korisnik korisnikZaPromenu)
        {
            if (korisnikZaPromenu == null)
            {
                return false;
            }
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);


            List<string> listaKorisnikaZaPonovniUpis = new List<string>();
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                bool nadjen = false;
                string[] splitter = line.Split(';');
                if (splitter[0] == korisnikZaPromenu.KorisnickoIme)
                {
                    nadjen = true;
                    listaKorisnikaZaPonovniUpis.Add(splitter[0] + ";" + splitter[1] + ";" + korisnikZaPromenu.Uloga + ";" + splitter[3] + ";" + splitter[4] + ";" + splitter[5] + ";" + splitter[6] + ";" + splitter[7] + ";" + splitter[8] + ";" + splitter[9] + ";" + splitter[10]);
                }
                if (!nadjen)
                {
                    listaKorisnikaZaPonovniUpis.Add(line);
                }

            }
            sr.Close();
            stream.Close();

            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Create, FileAccess.Write);
            StreamWriter sw1 = new StreamWriter(stream1);

            foreach (string korisnik in listaKorisnikaZaPonovniUpis)
            {
                sw1.WriteLine(korisnik);
            }
            sw1.Close();
            stream1.Close();
            return true;
        }

    }
}
