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
    public class ZalbeController : ApiController
    {
        [HttpPost]
        [ActionName("ZalbaNaPodforum")]
        public bool ZalbaNaPodforum([FromBody]Zalba zalba)
        {
            zalba.DatumZalbe = DateTime.Now;
            zalba.Id = Guid.NewGuid().ToString();
            zalba.TipEntiteta = "Podforum";
            // treba da nadjem autora zaljenog entiteta
           

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/podforumi.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string pfLine = "";
            while ((pfLine = sr.ReadLine()) != null)
            {
                string[] splitter = pfLine.Split(';');
                if (splitter[0] == zalba.Entitet)
                {
                    zalba.AutorZaljenogEntiteta = splitter[4];
                    break;
                }
            }
            sr.Close();
            stream.Close();




            
            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Open);
            StreamReader sr1 = new StreamReader(stream1);


            List<string> listaAdministratoraZaProsledjivanje = new List<string>();
            string line = "";
            while ((line = sr1.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[2] == "Administrator")
                {
                    listaAdministratoraZaProsledjivanje.Add(splitter[0]); //dodajemo smao korisnicko ime administrartora
                }
            }
            sr1.Close();
            stream1.Close();

            
            var dataFile2 = HttpContext.Current.Server.MapPath("~/App_Data/zalbe.txt");
            FileStream stream2 = new FileStream(dataFile2, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream2);

            foreach (string administrator in listaAdministratoraZaProsledjivanje)
            {
                sw.WriteLine(zalba.Id + ";" + zalba.Entitet + ";" + zalba.Tekst + ";" + zalba.DatumZalbe.ToShortDateString() + ";" + zalba.KorisnikKojiJeUlozio + ";" + administrator + ";" + zalba.AutorZaljenogEntiteta + ";" + zalba.TipEntiteta);
            }
            sw.Close();
            stream2.Close();
            return true;
        }

        [HttpPost]
        [ActionName("ZalbaNaTemu")]
        public bool ZalbaNaTemu([FromBody]Zalba zalba)
        {
            string[] podforumTema = zalba.Entitet.Split('-');

            string podforumZaljeneTeme = podforumTema[0];
            string naslovZaljeneTeme = podforumTema[1];

            zalba.Id = Guid.NewGuid().ToString();
            zalba.DatumZalbe = DateTime.Now;
            zalba.TipEntiteta = "Tema";
            //treba da nadjem autora zaljene teme

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string temaLine = "";
            while ((temaLine = sr.ReadLine()) != null)
            {
                string[] splitter = temaLine.Split(';');
                if (splitter[0] == podforumZaljeneTeme && splitter[1] == naslovZaljeneTeme)
                {
                    zalba.AutorZaljenogEntiteta = splitter[3];
                    break;
                }
            }
            sr.Close();
            stream.Close();

            // prvo nadji sve administratore da se njima prosledi

            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Open);
            StreamReader sr1 = new StreamReader(stream1);


            List<string> listaAdministratoraZaProsledjivanje = new List<string>();
            string line = "";
            while ((line = sr1.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[4] == "Administrator")
                {
                    listaAdministratoraZaProsledjivanje.Add(splitter[0]);
                }
            }
            sr1.Close();
            stream1.Close();

            // pa sada prodji kroz podforume i nadji odgovornog moderatora za podforum u kojem se ova tema nalazi

            var dataFile2 = HttpContext.Current.Server.MapPath("~/App_Data/podforumi.txt");
            FileStream stream2 = new FileStream(dataFile2, FileMode.Open);
            StreamReader sr2 = new StreamReader(stream2);

            string odgovorniModeratorPodforumaKomeTrebaDaSeProsledi = "";
            string pfLine = "";
            while ((pfLine = sr2.ReadLine()) != null)
            {
                string[] splitter = pfLine.Split(';');
                if (splitter[0] == podforumZaljeneTeme)
                {
                    odgovorniModeratorPodforumaKomeTrebaDaSeProsledi = splitter[4];
                    break;
                }
            }
            sr2.Close();
            stream2.Close();

            var dataFile3 = HttpContext.Current.Server.MapPath("~/App_Data/zalbe.txt");
            FileStream stream3 = new FileStream(dataFile3, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream3);


            foreach (string administrator in listaAdministratoraZaProsledjivanje)
            {
                sw.WriteLine(zalba.Id + ";" + zalba.Entitet + ";" + zalba.Tekst + ";" + zalba.DatumZalbe.ToShortDateString() + ";" + zalba.KorisnikKojiJeUlozio + ";" + administrator + ";" + zalba.AutorZaljenogEntiteta + ";" + zalba.TipEntiteta);
            }
            sw.WriteLine(zalba.Id + ";" + zalba.Entitet + ";" + zalba.Tekst + ";" + zalba.DatumZalbe.ToShortDateString() + ";" + zalba.KorisnikKojiJeUlozio + ";" + odgovorniModeratorPodforumaKomeTrebaDaSeProsledi + ";" + zalba.AutorZaljenogEntiteta + ";" + zalba.TipEntiteta);
            sw.Close();
            stream3.Close();
            return true;
        }

        [HttpPost]
        [ActionName("ZalbaNaKomentar")]
        public bool ZalbaNaKomentar([FromBody]Zalba zalba)
        {
            zalba.DatumZalbe = DateTime.Now;
            zalba.Id = Guid.NewGuid().ToString();
            zalba.TipEntiteta = "Komentar";

            // nadji autora zaljenog komentara
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string komentarLine = "";
            while ((komentarLine = sr.ReadLine()) != null)
            {
                string[] splitter = komentarLine.Split(';');
                if (splitter[0] == zalba.Entitet)
                {
                    zalba.AutorZaljenogEntiteta = splitter[2];
                    break;
                }
            }
            sr.Close();
            stream.Close();

            // prvo nadji sve administratore da se njima prosledi

            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Open);
            StreamReader sr1 = new StreamReader(stream1);

            List<string> listaAdministratoraZaProsledjivanje = new List<string>();
            string line = "";
            while ((line = sr1.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[2] == "Administrator")
                {
                    listaAdministratoraZaProsledjivanje.Add(splitter[0]);
                }
            }
            sr1.Close();
            stream1.Close();

            // prodji kroz komentare svih tema, ukoliko neka tema sadrzi u svojoj listi komenara zalba.Entite (idProsledjenogKomentara) sacuvaj podforum u kojem se nalazi ta tema
            // onda prodji kroz sve podforume i nadji onaj podforum koji sam malopre nasao i izvuci mu odgovornog moderatora


            var dataFile2 = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream2 = new FileStream(dataFile2, FileMode.Open);
            StreamReader sr2 = new StreamReader(stream2);

            string podforumZaPretraguModeratora = "";

            string temaLine = "";
            bool nadjen = false;
            while ((temaLine = sr2.ReadLine()) != null)
            {
                string[] splitter = temaLine.Split(';');
                // posplituj sad splitter[8] - tu su svi idEvi komentara
                string[] komentariSplitter = splitter[8].Split('|');
                foreach (string idKomentara in komentariSplitter)
                {
                    if (idKomentara == zalba.Entitet)
                    {
                        // To znaci da ova tema sadrzi taj komentar koji je poslat na zalbu i sad uzimam podforum u kom se ta tema nalazi
                        podforumZaPretraguModeratora = splitter[0];
                        nadjen = true;
                        break;
                    }
                }
                if (nadjen)
                {
                    break;
                }
            }
            sr2.Close();
            stream2.Close();

            // pa sada prodji kroz podforume i nadji odgovornog moderatora za podforum u kojem se nalazi tema u kojoj se nalazi komentar na koji se korisnik zalio


            var dataFile3 = HttpContext.Current.Server.MapPath("~/App_Data/podforumi.txt");
            FileStream stream3 = new FileStream(dataFile3, FileMode.Open);
            StreamReader sr3 = new StreamReader(stream3);

            string odgovorniModeratorPodforumaKomeTrebaDaSeProsledi = "";
            string pfLine = "";
            while ((pfLine = sr3.ReadLine()) != null)
            {
                string[] splitter = pfLine.Split(';');
                if (splitter[0] == podforumZaPretraguModeratora)
                {
                    odgovorniModeratorPodforumaKomeTrebaDaSeProsledi = splitter[4];
                    break;
                }
            }
            sr3.Close();
            stream3.Close();

            var dataFile4 = HttpContext.Current.Server.MapPath("~/App_Data/zalbe.txt");
            FileStream stream4 = new FileStream(dataFile4, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream4);

            foreach (string administrator in listaAdministratoraZaProsledjivanje)
            {
                sw.WriteLine(zalba.Id + ";" + zalba.Entitet + ";" + zalba.Tekst + ";" + zalba.DatumZalbe.ToShortDateString() + ";" + zalba.KorisnikKojiJeUlozio + ";" + administrator + ";" + zalba.AutorZaljenogEntiteta + ";" + zalba.TipEntiteta);
            }
            sw.WriteLine(zalba.Id + ";" + zalba.Entitet + ";" + zalba.Tekst + ";" + zalba.DatumZalbe.ToShortDateString() + ";" + zalba.KorisnikKojiJeUlozio + ";" + odgovorniModeratorPodforumaKomeTrebaDaSeProsledi + ";" + zalba.AutorZaljenogEntiteta + ";" + zalba.TipEntiteta);
            sw.Close();
            stream4.Close();

            return true;
        }

        [HttpPost]
        [ActionName("ZalbaNaPodkomentar")]
        public bool ZalbaNaPodkomentar([FromBody]Zalba zalba)
        {
            zalba.DatumZalbe = DateTime.Now;
            zalba.Id = Guid.NewGuid().ToString();
            zalba.TipEntiteta = "Podkomentar";

            string idRoditeljskogKomentara = "";

            // nadji autora zaljenog podkomentara

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/podkomentari.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string podkomentarLine = "";
            while ((podkomentarLine = sr.ReadLine()) != null)
            {
                string[] splitter = podkomentarLine.Split(';');
                if (splitter[1] == zalba.Entitet)
                {
                    idRoditeljskogKomentara = splitter[0];
                    zalba.AutorZaljenogEntiteta = splitter[2];
                    break;
                }
            }
            sr.Close();
            stream.Close();

            // prvo nadji sve administratore da se njima prosledi
            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Open);
            StreamReader sr1 = new StreamReader(stream1);

            List<string> listaAdministratoraZaProsledjivanje = new List<string>();
            string line = "";
            while ((line = sr1.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[2] == "Administrator")
                {
                    listaAdministratoraZaProsledjivanje.Add(splitter[0]);
                }
            }
            sr1.Close();
            stream1.Close();

            // prodji kroz komentare svih tema, ukoliko neka tema sadrzi u svojoj listi komenara idRoditeljskogKomentara sacuvaj podforum u kojem se nalazi ta tema
            // onda prodji kroz sve podforume i nadji onaj podforum koji sam malopre nasao i izvuci mu odgovornog moderatora

            var dataFile2 = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream2 = new FileStream(dataFile2, FileMode.Open);
            StreamReader sr2 = new StreamReader(stream2);

            string podforumZaPretraguModeratora = "";

            string temaLine = "";
            bool nadjen = false;
            while ((temaLine = sr2.ReadLine()) != null)
            {
                string[] splitter = temaLine.Split(';');
                // posplituj sad splitter[8] - tu su svi idEvi komentara
                string[] komentariSplitter = splitter[8].Split('|');
                foreach (string idKomentara in komentariSplitter)
                {
                    if (idKomentara == idRoditeljskogKomentara)
                    {
                        // To znaci da ova tema sadrzi taj komentar koji je poslat na zalbu i sad uzimam podforum u kom se ta tema nalazi
                        podforumZaPretraguModeratora = splitter[0];
                        nadjen = true;
                        break;
                    }
                }
                if (nadjen)
                {
                    break;
                }
            }
            sr2.Close();
            stream2.Close();

            // pa sada prodji kroz podforume i nadji odgovornog moderatora za podforum u kojem se nalazi tema u kojoj se nalazi komentar na koji se korisnik zalio

            var dataFile3 = HttpContext.Current.Server.MapPath("~/App_Data/podforumi.txt");
            FileStream stream3 = new FileStream(dataFile3, FileMode.Open);
            StreamReader sr3 = new StreamReader(stream3);

            string odgovorniModeratorPodforumaKomeTrebaDaSeProsledi = "";
            string pfLine = "";
            while ((pfLine = sr3.ReadLine()) != null)
            {
                string[] splitter = pfLine.Split(';');
                if (splitter[0] == podforumZaPretraguModeratora)
                {
                    odgovorniModeratorPodforumaKomeTrebaDaSeProsledi = splitter[4];
                    break;
                }
            }
            sr3.Close();
            stream3.Close();


            var dataFile4 = HttpContext.Current.Server.MapPath("~/App_Data/zalbe.txt");
            FileStream stream4 = new FileStream(dataFile4, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream4);

            foreach (string administrator in listaAdministratoraZaProsledjivanje)
            {
                sw.WriteLine(zalba.Id + ";" + zalba.Entitet + ";" + zalba.Tekst + ";" + zalba.DatumZalbe.ToShortDateString() + ";" + zalba.KorisnikKojiJeUlozio + ";" + administrator + ";" + zalba.AutorZaljenogEntiteta + ";" + zalba.TipEntiteta);
            }
            sw.WriteLine(zalba.Id + ";" + zalba.Entitet + ";" + zalba.Tekst + ";" + zalba.DatumZalbe.ToShortDateString() + ";" + zalba.KorisnikKojiJeUlozio + ";" + odgovorniModeratorPodforumaKomeTrebaDaSeProsledi + ";" + zalba.AutorZaljenogEntiteta + ";" + zalba.TipEntiteta);
            sw.Close();
            stream4.Close();

            return true;
        }

        [HttpGet]
        [ActionName("UzmiZalbeZaKorisnika")]
        public List<Zalba> UzmiZalbeZaKorisnika(string username)
        {
            List<Zalba> listaZalbi = new List<Zalba>();

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/zalbe.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[5] == username)
                {
                    // Ako je npr neko i moderator za podforum i administrator, da mu se ne ispise 2 puta
                    bool postojiUListi = listaZalbi.Any(zalba => zalba.Id == splitter[0]);
                    if (postojiUListi)
                    {
                        continue;
                    }
                    Zalba z = new Zalba();
                    z.Id = splitter[0];
                    z.Entitet = splitter[1];
                    z.Tekst = splitter[2];
                    z.DatumZalbe = DateTime.Parse(splitter[3]);
                    z.KorisnikKojiJeUlozio = splitter[4];
                    z.AutorZaljenogEntiteta = splitter[6];
                    z.TipEntiteta = splitter[7];

                    listaZalbi.Add(z);
                }
            }
            sr.Close();
            stream.Close();
            return listaZalbi;
        }

        [HttpPost]
        [ActionName("ObrisiZalbu")]
        public bool ObrisiZalbu([FromBody]Zalba zalbaZaBrisanje)
        {
            List<string> listaZalbiZaPonovniUpis = new List<string>();

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/zalbe.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                bool nadjena = false;
                string[] splitter = line.Split(';');

                if (splitter[0] == zalbaZaBrisanje.Id)
                {
                    nadjena = true;
                }
                if (!nadjena)
                {
                    listaZalbiZaPonovniUpis.Add(line);
                }
            }
            sr.Close();
            stream.Close();

            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/zalbe.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream1);


            foreach (string zalbaLn in listaZalbiZaPonovniUpis)
            {
                sw.WriteLine(zalbaLn);
            }
            sw.Close();
            stream1.Close();
            return true;
        }
    }
}
