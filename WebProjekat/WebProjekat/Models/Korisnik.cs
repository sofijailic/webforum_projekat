using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class Korisnik
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Uloga { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string BrTelefona { get; set; }
        public string Email { get; set; }
        public DateTime DatumRegistracije { get; set; }
        // lista foruma
        // lista tema
        // lista komentara

        public Korisnik(string korIme, string lozinka, string uloga, string ime, string prezime, string tel, string mail, DateTime datum)
        {
            this.KorisnickoIme = korIme;
            this.Lozinka = lozinka;
            this.Uloga = uloga;
            this.Ime = ime;
            this.Prezime = prezime;
            this.BrTelefona = tel;
            this.Email = mail;
            this.DatumRegistracije = datum;
        }
    }
}