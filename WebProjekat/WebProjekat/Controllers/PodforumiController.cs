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

    }
}
