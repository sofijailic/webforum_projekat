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
    public class PretragaController : ApiController
    {
        [HttpPost]
        [ActionName("PretraziPodforume")]
        public List<Podforum> PretraziPodforume([FromBody]Podforum podforumZaPretragu)
        {

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/podforumi.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<Podforum> listaSvihPodforuma = new List<Podforum>();

            List<Podforum> listaFiltriranih = new List<Podforum>();

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                Podforum p = new Podforum();
                string[] splitter = line.Split(';');

                p.Naziv = splitter[0];
                p.Opis = splitter[1];
                p.Ikonica = splitter[2];
                p.SpisakPravila = splitter[3];
                p.OdgovorniModerator = splitter[4];

                listaSvihPodforuma.Add(p);
            }
            sr.Close();
            stream.Close();

            if (podforumZaPretragu.Naziv != null && podforumZaPretragu.Opis == null && podforumZaPretragu.OdgovorniModerator == null)
            {
                // pretrazi samo po nazivu
                listaFiltriranih = listaSvihPodforuma.Where(p => p.Naziv.Contains(podforumZaPretragu.Naziv)).ToList();
            }


            else if (podforumZaPretragu.Naziv == null && podforumZaPretragu.Opis != null && podforumZaPretragu.OdgovorniModerator == null)
            {
                // pretrazi samo po opisu
                listaFiltriranih = listaSvihPodforuma.Where(p => p.Opis.Contains(podforumZaPretragu.Opis)).ToList();
            }


            else if (podforumZaPretragu.Naziv == null && podforumZaPretragu.Opis == null && podforumZaPretragu.OdgovorniModerator != null)
            {
                // pretrazi samo po moderatoru
                listaFiltriranih = listaSvihPodforuma.Where(p => p.OdgovorniModerator.Contains(podforumZaPretragu.OdgovorniModerator)).ToList();
            }


            else if (podforumZaPretragu.Naziv != null && podforumZaPretragu.Opis != null && podforumZaPretragu.OdgovorniModerator == null)
            {
                // pretrazi po nazivu i opisu
                listaFiltriranih = listaSvihPodforuma.Where(p => p.Naziv.Contains(podforumZaPretragu.Naziv) && p.Opis.Contains(podforumZaPretragu.Opis)).ToList();
            }


            else if (podforumZaPretragu.Naziv != null && podforumZaPretragu.Opis == null && podforumZaPretragu.OdgovorniModerator != null)
            {
                // pretrazi po nazivu i moderatoru
                listaFiltriranih = listaSvihPodforuma.Where(p => p.Naziv.Contains(podforumZaPretragu.Naziv) && p.OdgovorniModerator.Contains(podforumZaPretragu.OdgovorniModerator)).ToList();
            }


            else if (podforumZaPretragu.Naziv == null && podforumZaPretragu.Opis != null && podforumZaPretragu.OdgovorniModerator != null)
            {
                // pretrazi po opisu i moderatoru
                listaFiltriranih = listaSvihPodforuma.Where(p => p.Opis.Contains(podforumZaPretragu.Opis) && p.OdgovorniModerator.Contains(podforumZaPretragu.OdgovorniModerator)).ToList();
            }


            else if (podforumZaPretragu.Naziv != null && podforumZaPretragu.Opis != null && podforumZaPretragu.OdgovorniModerator != null)
            {
                // pretrazi po nazivu, opisu i moderatoru
                listaFiltriranih = listaSvihPodforuma.Where(p => p.Naziv.Contains(podforumZaPretragu.Naziv) && p.Opis.Contains(podforumZaPretragu.Opis) && p.OdgovorniModerator.Contains(podforumZaPretragu.OdgovorniModerator)).ToList();
            }


            return listaFiltriranih;
        }


        [HttpPost]
        [ActionName("PretraziTeme")]
        public List<Tema> PretraziTeme([FromBody]Tema temaZaPretragu)
        {
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<Tema> listaSvihTema = new List<Tema>();

            List<Tema> listaFiltriranih = new List<Tema>();

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                Tema t = new Tema();
                string[] splitter = line.Split(';');

                t.PodforumKomePripada = splitter[0];
                t.Naslov = splitter[1];
                t.Tip = splitter[2];
                t.Autor = splitter[3];
                t.Sadrzaj = splitter[4];
                t.DatumKreiranja = DateTime.Parse(splitter[5]);
                t.PozitivniGlasovi = Int32.Parse(splitter[6]);
                t.NegativniGlasovi = Int32.Parse(splitter[7]);

                listaSvihTema.Add(t);
            }
            sr.Close();
            stream.Close();

            // ------------------------------------------------- SAMO JEDAN KRITERIJUM ------------------------------------------------------
            if (temaZaPretragu.PodforumKomePripada != null && temaZaPretragu.Naslov == null && temaZaPretragu.Sadrzaj == null && temaZaPretragu.Autor == null)
            {
                // pretrazi samo po podforumu kome pripada
                listaFiltriranih = listaSvihTema.Where(t => t.PodforumKomePripada.Contains(temaZaPretragu.PodforumKomePripada)).ToList();
            }

            else if (temaZaPretragu.PodforumKomePripada == null && temaZaPretragu.Naslov != null && temaZaPretragu.Sadrzaj == null && temaZaPretragu.Autor == null)
            {
                // pretrazi samo po naslovu
                listaFiltriranih = listaSvihTema.Where(t => t.Naslov.Contains(temaZaPretragu.Naslov)).ToList();
            }

            else if (temaZaPretragu.PodforumKomePripada == null && temaZaPretragu.Naslov == null && temaZaPretragu.Sadrzaj != null && temaZaPretragu.Autor == null)
            {
                // pretrazi samo po sadrzaju
                listaFiltriranih = listaSvihTema.Where(t => t.Sadrzaj.Contains(temaZaPretragu.Sadrzaj)).ToList();
            }

            else if (temaZaPretragu.PodforumKomePripada == null && temaZaPretragu.Naslov == null && temaZaPretragu.Sadrzaj == null && temaZaPretragu.Autor != null)
            {
                // pretrazi samo po autoru
                listaFiltriranih = listaSvihTema.Where(t => t.Autor.Contains(temaZaPretragu.Autor)).ToList();
            }

            // ------------------------------------------------- DVA KRITERIJUMA ------------------------------------------------------

            else if (temaZaPretragu.PodforumKomePripada != null && temaZaPretragu.Naslov != null && temaZaPretragu.Sadrzaj == null && temaZaPretragu.Autor == null)
            {
                // pretrazi po podforumu u koji pripada i naslovu
                listaFiltriranih = listaSvihTema.Where(t => t.PodforumKomePripada.Contains(temaZaPretragu.PodforumKomePripada) && t.Naslov.Contains(temaZaPretragu.Naslov)).ToList();
            }

            else if (temaZaPretragu.PodforumKomePripada != null && temaZaPretragu.Naslov == null && temaZaPretragu.Sadrzaj != null && temaZaPretragu.Autor == null)
            {
                // pretrazi po podforumu u koji pripada i sadrzaju
                listaFiltriranih = listaSvihTema.Where(t => t.PodforumKomePripada.Contains(temaZaPretragu.PodforumKomePripada) && t.Sadrzaj.Contains(temaZaPretragu.Sadrzaj)).ToList();
            }

            else if (temaZaPretragu.PodforumKomePripada != null && temaZaPretragu.Naslov == null && temaZaPretragu.Sadrzaj == null && temaZaPretragu.Autor != null)
            {
                // pretrazi po podforumu u koji pripada i autoru
                listaFiltriranih = listaSvihTema.Where(t => t.PodforumKomePripada.Contains(temaZaPretragu.PodforumKomePripada) && t.Autor.Contains(temaZaPretragu.Autor)).ToList();
            }

            // -----------

            else if (temaZaPretragu.PodforumKomePripada == null && temaZaPretragu.Naslov != null && temaZaPretragu.Sadrzaj != null && temaZaPretragu.Autor == null)
            {
                // pretrazi po naslovu i sadrzaju
                listaFiltriranih = listaSvihTema.Where(t => t.Naslov.Contains(temaZaPretragu.Naslov) && t.Sadrzaj.Contains(temaZaPretragu.Sadrzaj)).ToList();
            }

            else if (temaZaPretragu.PodforumKomePripada == null && temaZaPretragu.Naslov != null && temaZaPretragu.Sadrzaj == null && temaZaPretragu.Autor != null)
            {
                // pretrazi po naslovu i autoru
                listaFiltriranih = listaSvihTema.Where(t => t.Naslov.Contains(temaZaPretragu.Naslov) && t.Autor.Contains(temaZaPretragu.Autor)).ToList();
            }

            // -----------

            else if (temaZaPretragu.PodforumKomePripada == null && temaZaPretragu.Naslov == null && temaZaPretragu.Sadrzaj != null && temaZaPretragu.Autor != null)
            {
                // pretrazi po sadrzaju i autoru
                listaFiltriranih = listaSvihTema.Where(t => t.Sadrzaj.Contains(temaZaPretragu.Sadrzaj) && t.Autor.Contains(temaZaPretragu.Autor)).ToList();
            }

            // -------------------------------------- TRI KRITERIJUMA -------------------------------------------

            else if (temaZaPretragu.PodforumKomePripada != null && temaZaPretragu.Naslov != null && temaZaPretragu.Sadrzaj != null && temaZaPretragu.Autor == null)
            {
                // pretrazi po podforumu, naslovu, sadrzaju
                listaFiltriranih = listaSvihTema.Where(t => t.PodforumKomePripada.Contains(temaZaPretragu.PodforumKomePripada) && t.Naslov.Contains(temaZaPretragu.Naslov) && t.Sadrzaj.Contains(temaZaPretragu.Sadrzaj)).ToList();
            }

            else if (temaZaPretragu.PodforumKomePripada != null && temaZaPretragu.Naslov != null && temaZaPretragu.Sadrzaj == null && temaZaPretragu.Autor != null)
            {
                // pretrazi po podforumu, naslovu, autoru
                listaFiltriranih = listaSvihTema.Where(t => t.PodforumKomePripada.Contains(temaZaPretragu.PodforumKomePripada) && t.Naslov.Contains(temaZaPretragu.Naslov) && t.Autor.Contains(temaZaPretragu.Autor)).ToList();
            }

            else if (temaZaPretragu.PodforumKomePripada != null && temaZaPretragu.Naslov == null && temaZaPretragu.Sadrzaj != null && temaZaPretragu.Autor != null)
            {
                // pretrazi po podforumu, sadrzaju, autoru
                listaFiltriranih = listaSvihTema.Where(t => t.PodforumKomePripada.Contains(temaZaPretragu.PodforumKomePripada) && t.Sadrzaj.Contains(temaZaPretragu.Sadrzaj) && t.Autor.Contains(temaZaPretragu.Autor)).ToList();
            }

            else if (temaZaPretragu.PodforumKomePripada == null && temaZaPretragu.Naslov != null && temaZaPretragu.Sadrzaj != null && temaZaPretragu.Autor != null)
            {
                // pretrazi po naslovu, sadrzaju, autoru
                listaFiltriranih = listaSvihTema.Where(t => t.Sadrzaj.Contains(temaZaPretragu.Sadrzaj) && t.Naslov.Contains(temaZaPretragu.Naslov) && t.Autor.Contains(temaZaPretragu.Autor)).ToList();
            }

            // ------------------------------------------------ SVA 4 KRITERIJUMA -------------------------------------------------

            else if (temaZaPretragu.PodforumKomePripada != null && temaZaPretragu.Naslov != null && temaZaPretragu.Sadrzaj != null && temaZaPretragu.Autor != null)
            {
                // pretrazi po naslovu, sadrzaju, autoru
                listaFiltriranih = listaSvihTema.Where(t => t.PodforumKomePripada.Contains(temaZaPretragu.PodforumKomePripada) && t.Naslov.Contains(temaZaPretragu.Naslov) && t.Autor.Contains(temaZaPretragu.Autor) && t.Sadrzaj.Contains(temaZaPretragu.Sadrzaj)).ToList();
            }

            return listaFiltriranih;
        }

        [HttpPost]
        [ActionName("PretraziKorisnike")]
        public List<Korisnik> PretraziKorisnike([FromBody]Korisnik korisnikZaPretragu)
        {
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<Korisnik> listaSvihKorisnika = new List<Korisnik>();

            List<Korisnik> listaFiltriranih = new List<Korisnik>();

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                Korisnik k = new Korisnik();
                string[] splitter = line.Split(';');

                k.KorisnickoIme = splitter[0];
                k.Uloga = splitter[2];
                k.Ime = splitter[3];
                k.Prezime = splitter[4];
                k.BrTelefona = splitter[5];
                k.Email = splitter[6];
                k.DatumRegistracije = DateTime.Parse(splitter[7]);

                listaSvihKorisnika.Add(k);
            }
            sr.Close();
            stream.Close();

            if (korisnikZaPretragu.KorisnickoIme != null)
            {
                listaFiltriranih = listaSvihKorisnika.Where(k => k.KorisnickoIme.Contains(korisnikZaPretragu.KorisnickoIme)).ToList();
            }

            return listaFiltriranih;
        }
    }
}
