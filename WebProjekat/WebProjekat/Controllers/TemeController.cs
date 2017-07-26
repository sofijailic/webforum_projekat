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
    public class TemeController : ApiController
    {
        [HttpPost]
        [ActionName("DodajJednuTemu")]
        public Tema DodajJednuTemu(Tema t)
        {
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[0] == t.PodforumKomePripada && splitter[1] == t.Naslov)
                {
                    sr.Close();
                    stream.Close();
                    return null;
                }
            }
            sr.Close();
            stream.Close();


            FileStream stream2 = new FileStream(dataFile, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream2);


            t.Komentari = new List<string>();
            t.DatumKreiranja = DateTime.Now;
            t.PozitivniGlasovi = 0;
            t.NegativniGlasovi = 0;

            sw.WriteLine(t.PodforumKomePripada + ";" + t.Naslov + ";" + t.Tip + ";" + t.Autor + ";" + t.Sadrzaj + ";" + t.DatumKreiranja.ToShortDateString() + ";" + t.PozitivniGlasovi.ToString() + ";" + t.NegativniGlasovi.ToString() + ";" + "nePostoje");
            sw.Close();
            stream2.Close();
            return t;
        }

        [HttpGet]
        [ActionName("UzmiSveTemeIzPodforuma")]
        public List<Tema> UzmiSveTemeIzPodforuma(string podforum)
        {
            List<Tema> listaTema = new List<Tema>();


            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);



            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                if (line != "")
                {

                    string[] splitter = line.Split(';');
                    List<string> listaKomentara = new List<string>();

                    if (splitter[0] == podforum)
                    {
                        string[] commentSplitter = splitter[8].Split('|');
                        foreach (string komentar in commentSplitter)
                        {
                            if (komentar != "nePostoje")
                            {
                                listaKomentara.Add(komentar);
                            }

                        }

                        listaTema.Add(new Tema(splitter[0], splitter[1], splitter[2], splitter[3], splitter[4], DateTime.Parse(splitter[5]), Int32.Parse(splitter[6]), Int32.Parse(splitter[7]), listaKomentara));
                    }
                }

            }
            sr.Close();
            stream.Close();
            return listaTema;
        }

        [HttpGet]
        [ActionName("UzmiTemuPoImenu")]
        public Tema UzmiTemuPoImenu(string podforum, string tema)
        {
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            List<string> listaKomentara = new List<string>();

            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[0] == podforum && splitter[1] == tema)
                {
                    string[] komentarSplitter = splitter[8].Split('|');
                    foreach (string komentar in komentarSplitter)
                    {
                        if (komentar != "nePostoje")
                        {
                            listaKomentara.Add(komentar);
                        }
                    }
                    sr.Close();
                    stream.Close();
                    return new Tema(splitter[0], splitter[1], splitter[2], splitter[3], splitter[4], DateTime.Parse(splitter[5]), Int32.Parse(splitter[6]), Int32.Parse(splitter[7]), listaKomentara);
                }
            }

            sr.Close();
            stream.Close();
            return null;
        }
    }
}
