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
    public class PodforumiController : ApiController
    {
        [HttpPost]
        [ActionName("DodavanjePodforuma")]
        public Podforum DodavanjePodforuma([FromBody]Podforum p)
        { 
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/podforumi.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
        
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[0] == p.Naziv)
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

            p.Moderatori = new List<string>();
            sw.WriteLine(p.Naziv + ";" + p.Opis +";"+ "ikonica"  + ";" + p.SpisakPravila + ";" + p.Moderator + ";" + p.Moderator); // za pocetak ima samo jedan moderator
            sw.Close();
            stream2.Close();

            return p;
        }

        [HttpGet]
        [ActionName("UzmiSvePodforume")]
        public List<Podforum> UzmiSvePodforume()
        {
            List<Podforum> listaSvihPodforuma = new List<Podforum>();

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/podforumi.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                if (line != "")
                {

                    string[] splitter = line.Split(';');
                    List<string> listaModeratora = new List<string>();

                    string[] moderatorSplitter = splitter[5].Split('|');
                    foreach (string moderator in moderatorSplitter)
                    {
                        listaModeratora.Add(moderator);
                    }
                    listaSvihPodforuma.Add(new Podforum(splitter[0], splitter[1], "ikonica", splitter[3], splitter[4], listaModeratora));
                }

            }
            sr.Close();
            stream.Close();
            return listaSvihPodforuma;
        }
        [HttpGet]
        [ActionName("UzmiPodforumPoImenu")]
        public Podforum UzmiPodforumPoImenu(string nazivPodforuma) {

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/podforumi.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[0] == nazivPodforuma)
                {
                    sr.Close();
                    stream.Close();
                    List<string> listaModeratora = new List<string>();

                    string[] moderatorSplitter = splitter[5].Split('|');
                    foreach (string moderator in moderatorSplitter)
                    {
                        listaModeratora.Add(moderator);
                    }
                    return new Podforum(splitter[0], splitter[1], splitter[2], splitter[3], splitter[4], listaModeratora);
                }
            }

            sr.Close();
            stream.Close();
            return null;
        }

        [HttpPost]
        [ActionName("SacuvajPodforum")]
        public bool SacuvajPodforum([FromBody]PodforumZaCuvanje pfZaCuvanje)
        {

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<string> listaSvihKorisnika = new List<string>();
            int brojac = 0;
            int indexZaIzmenu = -1;
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                listaSvihKorisnika.Add(line);
                brojac++;

                string[] splitter = line.Split(';');
                if (splitter[0] == pfZaCuvanje.KorisnikKojiCuva)
                {
                    indexZaIzmenu = brojac;
                }
            }
            sr.Close();
            stream.Close();

            // splituj tu liniju koja treba da se menja tj na koju treba da se dodaje
            string[] tokeniOdabranogKorisnika = listaSvihKorisnika[indexZaIzmenu - 1].Split(';');
            // tokeniOdabranogKorisnika[8] tu se nalazi spisak pracenih podforuma
            string[] splitterProvere = tokeniOdabranogKorisnika[8].Split('|');
            // provera ukoliko korisnik vec prati postojeci podforum
            foreach (string praceniPodforum in splitterProvere)
            {
                if (praceniPodforum == pfZaCuvanje.NazivPodforuma)
                {
                    return false;
                }
            }
            // otvori bulk writer
           
            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Create, FileAccess.Write);
            StreamWriter sw1 = new StreamWriter(stream1);


            // tokeniOdabranogKorisnika[8] tu se nalazi spisak pracenih podforuma
            tokeniOdabranogKorisnika[8] += "|" + pfZaCuvanje.NazivPodforuma;

            // linijaZaUpis se inicijalizuje na pocetku da je korisnicki username
            string linijaZaUpis = tokeniOdabranogKorisnika[0];
            // prodji kroz sve tokene odabranog korisnika i upisi ih u liniju, da ne pisem tokeni[0]+';'+tokeni[1] ...
            for (int i = 1; i < 11; i++)
            {
                linijaZaUpis += ";" + tokeniOdabranogKorisnika[i];
            }

            // ubaci tu izmenjenu liniju na to mesto u listiSvih
            listaSvihKorisnika[indexZaIzmenu - 1] = linijaZaUpis;
            // prepisi ceo fajl
            foreach (string korisnickaLinija in listaSvihKorisnika)
            {
                sw1.WriteLine(korisnickaLinija);
            }
            sw1.Close();
            stream1.Close();

            return true;
        }
        
        [HttpGet]
        [ActionName("UzmiSacuvanePodforume")]
        public List<Podforum> UzmiSacuvanePodforume(string username)
        {

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<Podforum> listaSacuvanihPodforuma = new List<Podforum>();

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[0] == username)
                {
                    string[] podforumSplitter = splitter[8].Split('|');
                    foreach (string podforumId in podforumSplitter)
                    {
                        if (podforumId != "nemaSnimljenihPodforuma")
                        {
                            // Prodji kroz sve podforume, nadji taj sa tim imenom, napravi novi Podforum i dodaj ga u listu

                           
                            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/podforumi.txt");
                            FileStream stream1 = new FileStream(dataFile1, FileMode.Open);
                            StreamReader sr1 = new StreamReader(stream1);

                            string podforumLine = "";
                            while ((podforumLine = sr1.ReadLine()) != null)
                            {
                                string[] podforumLineSplitter = podforumLine.Split(';');
                                if (podforumLineSplitter[0] == podforumId)
                                {
                                    List<string> odgovorniModeratori = new List<string>();
                                    string[] moderatoriSplitter = podforumLineSplitter[5].Split('|');
                                    foreach (string moderator in moderatoriSplitter)
                                    {
                                        odgovorniModeratori.Add(moderator);
                                    }
                                    listaSacuvanihPodforuma.Add(new Podforum(podforumLineSplitter[0], podforumLineSplitter[1], podforumLineSplitter[2], podforumLineSplitter[3], podforumLineSplitter[4], odgovorniModeratori));
                                    break;
                                }
                            }
                            sr1.Close();
                            stream1.Close();
                        }
                    }
                }
            }
            sr.Close();
            stream.Close();
            return listaSacuvanihPodforuma;
        }

        [HttpPost]
        [ActionName("ObrisiPodforum")]
        public bool ObrisiPodforum([FromBody]Podforum podforumZaBrisanje)
        {
            // 1. Prodji kroz sve podforumi.txt i kada nadjes da je splitter[0] == p.Naziv preskoci ga sa dodavanjem u listu
            // 2. Prodji kroz sve teme i svaka koja ima splitter[0] == p.Naziv obrisi, tj nemoj je prepisati
            // 3. Prodji kroz komentare i svaki obrisan dodaj u listuObrisanih
            // 4. Prodji kroz podkomentare i obrisi one ciji su roditelji u listiObrisanih
            // 5. prodji kroz lajkDislajkKomentari
            // 6. Prodji kroz lajk dislajk teme

            // -------------------------------------- 1 ----------------------------------------

            
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/podforumi.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<string> listaPodforumaZaPonovniUpis = new List<string>();

            string linijaPodforum = "";
            while ((linijaPodforum = sr.ReadLine()) != null)
            {
                bool nadjen = false;
                string[] podforumSplitter = linijaPodforum.Split(';');
                if (podforumSplitter[0] == podforumZaBrisanje.Naziv)
                {
                    nadjen = true;
                }
                if (!nadjen)
                {
                    listaPodforumaZaPonovniUpis.Add(linijaPodforum);
                }
            }

            sr.Close();
            stream.Close();

            
            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/podforumi.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream1);

            foreach (string podforumLn in listaPodforumaZaPonovniUpis)
            {
                sw.WriteLine(podforumLn);
            }
            sw.Close();
            stream1.Close();

            // -------------------------------------- 1 close ----------------------------------

            // -------------------------------------- 2 ----------------------------------------


            var dataFile2 = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream2 = new FileStream(dataFile2, FileMode.Open);
            StreamReader sr1 = new StreamReader(stream2);

            List<string> listaTemaZaPonovniUpis = new List<string>();

            string linijaTema = "";
            while ((linijaTema = sr1.ReadLine()) != null)
            {
                bool nadjena = false;
                string[] temaSplitter = linijaTema.Split(';');
                if (temaSplitter[0] == podforumZaBrisanje.Naziv)
                {
                    nadjena = true;
                }
                if (!nadjena)
                {
                    listaTemaZaPonovniUpis.Add(linijaTema);
                }
            }

            sr1.Close();
            stream2.Close();


            var dataFile3 = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream3 = new FileStream(dataFile3, FileMode.Create, FileAccess.Write);
            StreamWriter sw1 = new StreamWriter(stream3);

            foreach (string temaLn in listaTemaZaPonovniUpis)
            {
                sw1.WriteLine(temaLn);
            }
            sw1.Close();
            stream3.Close();

            // -------------------------------------- 2 close ----------------------------------

            // -------------------------------------- 3 ----------------------------------------


            var dataFile4 = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");
            FileStream stream4 = new FileStream(dataFile4, FileMode.Open);
            StreamReader sr2 = new StreamReader(stream4);

            List<string> listaKomentaraZaBrisanje = new List<string>();

            List<string> listaKomentaraZaPonovniUpis = new List<string>();

            string komentarLinija = "";

            while ((komentarLinija = sr2.ReadLine()) != null)
            {
                bool nadjen = false;

                string[] komentarSplitter = komentarLinija.Split(';');
                string[] podforumNaslovTemeSplitter = komentarSplitter[1].Split('-');
                string podforum = podforumNaslovTemeSplitter[0];
                string naslov = podforumNaslovTemeSplitter[1];

                if (podforum == podforumZaBrisanje.Naziv)
                {
                    listaKomentaraZaBrisanje.Add(komentarSplitter[0]);
                    nadjen = true;
                }
                if (!nadjen)
                {
                    listaKomentaraZaPonovniUpis.Add(komentarLinija);
                }
            }
            sr2.Close();
            stream4.Close();


            var dataFile5 = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");
            FileStream stream5 = new FileStream(dataFile5, FileMode.Create, FileAccess.Write);
            StreamWriter sw2 = new StreamWriter(stream5);

            foreach (string komentarLn in listaKomentaraZaPonovniUpis)
            {
                sw2.WriteLine(komentarLn);
            }
            sw2.Close();
            stream5.Close();

            // -------------------------------------- 3 close ----------------------------------

            // -------------------------------------- 4 ----------------------------------------

            var dataFile6 = HttpContext.Current.Server.MapPath("~/App_Data/podkomentari.txt");
            FileStream stream6 = new FileStream(dataFile6, FileMode.Open);
            StreamReader sr3 = new StreamReader(stream6);

            List<string> listaPodkomentaraZaBrisanje = new List<string>();

            List<string> listaPodkomentaraZaPonovniUpis = new List<string>();

            string podkomentarLinija = "";
            while ((podkomentarLinija = sr3.ReadLine()) != null)
            {
                bool nadjen = false;
                string[] podkomentarSplitter = podkomentarLinija.Split(';');
                foreach (string idRoditelja in listaKomentaraZaBrisanje)
                {
                    if (podkomentarSplitter[0] == idRoditelja)
                    {
                        nadjen = true;
                        listaPodkomentaraZaBrisanje.Add(podkomentarSplitter[1]);
                    }
                }
                if (!nadjen)
                {
                    listaPodkomentaraZaPonovniUpis.Add(podkomentarLinija);
                }
            }

            sr3.Close();
            stream6.Close();

            var dataFile7 = HttpContext.Current.Server.MapPath("~/App_Data/podkomentari.txt");
            FileStream stream7 = new FileStream(dataFile7, FileMode.Create, FileAccess.Write);
            StreamWriter sw3 = new StreamWriter(stream7);

            foreach (string podkomentarLn in listaPodkomentaraZaPonovniUpis)
            {
                sw3.WriteLine(podkomentarLn);
            }
            sw3.Close();
            stream7.Close();

            // -------------------------------------- 4 close ----------------------------------

            // -------------------------------------- 5 ----------------------------------------

            var dataFile8 = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkKomentari.txt");
            FileStream stream8 = new FileStream(dataFile8, FileMode.Open);
            StreamReader sr4 = new StreamReader(stream8);

            List<string> listaLajkovanihDislajkovanihKomentaraZaPonovniUpis = new List<string>();

            string likeDislikeComLine = "";
            while ((likeDislikeComLine = sr4.ReadLine()) != null)
            {
                bool nadjen = false;
                string[] likeDislikeSplitter = likeDislikeComLine.Split(';');
                foreach (string idKomentara in listaKomentaraZaBrisanje)
                {
                    if (likeDislikeSplitter[1] == idKomentara)
                    {
                        nadjen = true;
                    }
                }
                foreach (string idPodkomentara in listaPodkomentaraZaBrisanje)
                {
                    if (likeDislikeSplitter[1] == idPodkomentara)
                    {
                        nadjen = true;
                    }
                }
                if (!nadjen)
                {
                    listaLajkovanihDislajkovanihKomentaraZaPonovniUpis.Add(likeDislikeComLine);
                }
            }
            sr4.Close();
            stream8.Close();


            var dataFile9 = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkKomentari.txt");
            FileStream stream9 = new FileStream(dataFile9, FileMode.Create, FileAccess.Write);
            StreamWriter sw4 = new StreamWriter(stream9);

            foreach (string likeDislikeLn in listaLajkovanihDislajkovanihKomentaraZaPonovniUpis)
            {
                sw4.WriteLine(likeDislikeLn);
            }
            sw4.Close();
            stream9.Close();

            // -------------------------------------- 5 close ----------------------------------

            // -------------------------------------- 6 ----------------------------------------

            var dataFile10 = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkTeme.txt");
            FileStream stream10 = new FileStream(dataFile10, FileMode.Open);
            StreamReader sr5 = new StreamReader(stream10);


            List<string> listaLajkovanihDislajkovanihTemaZaPonovniUpis = new List<string>();

            string likeDislikeTemeLinija = "";
            while ((likeDislikeTemeLinija = sr5.ReadLine()) != null)
            {
                bool nadjen = false;

                string[] likeDislikeTemeLineSplitter = likeDislikeTemeLinija.Split(';');
                string[] podforumNazivSplitter = likeDislikeTemeLineSplitter[1].Split('-');
                string podforum = podforumNazivSplitter[0];
                string nazivTeme = podforumNazivSplitter[1];

                if (podforum == podforumZaBrisanje.Naziv)
                {
                    nadjen = true;
                }
                if (!nadjen)
                {
                    listaLajkovanihDislajkovanihTemaZaPonovniUpis.Add(likeDislikeTemeLinija);
                }
            }
            sr5.Close();
            stream5.Close();


            var dataFile11 = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkTeme.txt");
            FileStream stream11 = new FileStream(dataFile11, FileMode.Create, FileAccess.Write);
            StreamWriter sw5 = new StreamWriter(stream11);

            foreach (string temaLn in listaLajkovanihDislajkovanihTemaZaPonovniUpis)
            {
               sw5.WriteLine(temaLn);
            }
            sw5.Close();
            stream11.Close();

            // -------------------------------------- 6 close ----------------------------------

            return true;
        }

    }
}

