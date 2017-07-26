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
    }
}
