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
    public class PorukeController : ApiController
    {
        [HttpPost]
        [ActionName("PosaljiPoruku")]
        public bool PosaljiPoruku([FromBody]Poruka porukaZaSlanje)
        {

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/poruke.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamWriter sw = new StreamWriter(stream);

            porukaZaSlanje.Id = Guid.NewGuid().ToString();
            sw.WriteLine(porukaZaSlanje.Id + ";" + porukaZaSlanje.Posiljalac + ";" + porukaZaSlanje.Primalac + ";" + porukaZaSlanje.Sadrzaj + ";" + porukaZaSlanje.Procitana.ToString());
            sw.Close();
            stream.Close();
            return true;
        }

        [HttpGet]
        [ActionName("UzmiSvePoruke")]
        public List<Poruka> UzmiSvePoruke(string username)
        {

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/poruke.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<Poruka> listaPoruka = new List<Poruka>();

            string linija = "";
            while ((linija = sr.ReadLine()) != null)
            {
                string[] splitter = linija.Split(';');
                if (splitter[2] == username)
                {
                    Poruka p = new Poruka();
                    p.Id = splitter[0];
                    p.Posiljalac = splitter[1];
                    p.Primalac = splitter[2];
                    p.Sadrzaj = splitter[3];
                    p.Procitana = bool.Parse(splitter[4]);

                    listaPoruka.Add(p);
                }
            }
            sr.Close();
            stream.Close();
            return listaPoruka;
        }
    }
}
