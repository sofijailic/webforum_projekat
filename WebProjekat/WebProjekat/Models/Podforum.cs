using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class Podforum
    {
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public string Ikonica { get; set; }
        public string SpisakPravila { get; set; }
        public string Moderator { get; set; }
        public string OdgovorniModerator { get; set; }
        public List<string> Moderatori { get; set; }

        public Podforum(string naziv, string opis, string ikonica, string pravila, string odgovorniMod, List<string> moderatori)
        {
            Naziv = naziv;
            Opis = opis;
            Ikonica = ikonica;
            SpisakPravila = pravila;
            OdgovorniModerator = odgovorniMod;
            Moderatori = moderatori;

        }
        public Podforum() { }
    }
}