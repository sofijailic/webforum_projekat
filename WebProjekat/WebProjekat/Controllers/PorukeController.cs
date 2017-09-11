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
            FileStream stream = new FileStream(dataFile, FileMode.Append, FileAccess.Write);
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


        [HttpPost]
        [ActionName("OznaciKaoProcitano")]
        public bool OznaciKaoProcitano(string id)
        {

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/poruke.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<string> listaPorukaZaPonovniUpis = new List<string>();

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                bool nadjena = false;
                string[] splitter = line.Split(';');
                if (splitter[0] == id)
                {
                    nadjena = true;
                    listaPorukaZaPonovniUpis.Add(splitter[0] + ";" + splitter[1] + ";" + splitter[2] + ";" + splitter[3] + ";" + "True");
                }
                if (!nadjena)
                {
                    listaPorukaZaPonovniUpis.Add(line);
                }
            }
            sr.Close();
            stream.Close();

            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/poruke.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream1);

            foreach (string poruka in listaPorukaZaPonovniUpis)
            {
                sw.WriteLine(poruka);
            }
            sw.Close();
            stream1.Close();
            return true;
        }

    }
}
