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
    public class PreporukeController : ApiController
    {

        [HttpGet]
        [ActionName("UzmiPreporukeZaKorisnika")]
        public List<Tema> UzmiPreporukeZaKorisnika(string username)
        {
            List<Tema> listaPreporucenihTema = new List<Tema>();

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<Tema> listaPracenihTema = new List<Tema>();
            List<string> listaPracenihPodforumaString = new List<string>();

            string korLine = "";
            while ((korLine = sr.ReadLine()) != null)
            {
                string[] splitter = korLine.Split(';');
                if (splitter[0] == username)
                {
                    // uzmi mu pracene podforume i teme
                    string[] splitterPracenihPodforuma = splitter[8].Split('|');
                    string[] splitterPracenihTema = splitter[9].Split('|');

                    listaPracenihPodforumaString.AddRange(splitterPracenihPodforuma);
                    foreach (string tema in splitterPracenihTema)
                    {
                        Tema t = new Tema();
                        if (tema == "nemaSnimljenihTema")
                        {
                            continue;
                        }
                        string[] splitterPfTema = tema.Split('-');
                        string podforumKomePripada = splitterPfTema[0];
                        string naslovTeme = splitterPfTema[1];
                        t.PodforumKomePripada = podforumKomePripada;
                        t.Naslov = naslovTeme;
                        listaPracenihTema.Add(t);
                    }
                    break;
                }
            }
            sr.Close();
            stream.Close();

            // prodji kroz sve teme, ukoliko se podforum od te teme nalazi u listi pracenihPodforuma i ukoliko se naslov te teme NE nalazi u listi pracenih tema, i ukoliko ta tema
            // ima vise od 5 pozitivnih glasova, parsiraj u temu i dodaj u listuPreporucenih

            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Open);
            StreamReader sr1 = new StreamReader(stream1);

            string temaLine = "";
            while ((temaLine = sr1.ReadLine()) != null)
            {
                string[] splitter = temaLine.Split(';');
                bool pratiPodforum = listaPracenihPodforumaString.Any(podforum => podforum == splitter[0]);
                if (pratiPodforum)
                {
                    // ako korisnik prati podforum u kom se ova tema nalazi
                    // proveri da li se OVA tema nalazi u listi njegovih pracenih

                    bool nalaziSeUListiPracenih = listaPracenihTema.Any(tema => tema.PodforumKomePripada == splitter[0] && tema.Naslov == splitter[1]);
                    // ukoliko korisnik nije vec sacuvao ovu temu, i ova tema ima 5 ili vise pozitivnih glasova, dodaj mu je u preporuke
                    if (!nalaziSeUListiPracenih && Int32.Parse(splitter[6]) >= 5)
                    {
                        Tema t = new Tema();
                        t.PodforumKomePripada = splitter[0];
                        t.Naslov = splitter[1];
                        t.Tip = splitter[2];
                        t.Autor = splitter[3];
                        t.Sadrzaj = splitter[4];
                        t.DatumKreiranja = DateTime.Parse(splitter[5]);
                        t.PozitivniGlasovi = Int32.Parse(splitter[6]);
                        t.NegativniGlasovi = Int32.Parse(splitter[7]);

                        listaPreporucenihTema.Add(t);
                    }
                }
            }

            sr1.Close();
            stream1.Close();

            return listaPreporucenihTema;
        }
    }
}
