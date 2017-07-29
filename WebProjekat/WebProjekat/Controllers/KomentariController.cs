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
    public class KomentariController : ApiController
    {
        [HttpPost]
        [ActionName("DodajKomentar")]
        public Komentar DodajKomentar([FromBody]Komentar k)
        {

            string[] splitovanaTema = k.TemaKojojPripada.Split('-');
            List<string> listaSvihTema = new List<string>();
            int brojac = 0;
            int indexZaIzmenu = -1;

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string linija = "";
            while ((linija = sr.ReadLine()) != null)
            {
                listaSvihTema.Add(linija);
                brojac++;

                string[] splitter = linija.Split(';');
                if (splitter[0] == splitovanaTema[0] && splitter[1] == splitovanaTema[1])
                {
                    indexZaIzmenu = brojac;
                }
            }
            sr.Close();
            stream.Close();
            // Upis u teme.txt tj dodavanje novog

            FileStream stream2 = new FileStream(dataFile, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream2);


            k.Id = Guid.NewGuid().ToString();
            k.DatumKomentara = DateTime.Now;
            k.Izmenjen = false;
            k.NegativniGlasovi = 0;
            k.PozitivniGlasovi = 0;
            k.Podkomentari = new List<Komentar>();
            k.RoditeljskiKomentar = "nemaRoditelja";
            k.Obrisan = false;

            // Prvo ako ova tema nema komentar tj ako joj je spliter listaSvihTema[indexZaIzmenu-1][8] == 'nePostoje', obrisi to nePostoje

            listaSvihTema[indexZaIzmenu-1] += "|" + k.Id;

            foreach (string tema in listaSvihTema)
            {
                sw.WriteLine(tema);

            }
            sw.Close();
            stream2.Close();

            // Upis u komentari.txt

            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");

            FileStream stream3 = new FileStream(dataFile1, FileMode.Append, FileAccess.Write);
            StreamWriter sw1 = new StreamWriter(stream3);
            sw1.WriteLine(k.Id + ";" + k.TemaKojojPripada + ";" + k.Autor + ";" + k.DatumKomentara.ToShortDateString() + ";" + k.RoditeljskiKomentar + ";" + k.Tekst + ";" + k.PozitivniGlasovi.ToString() + ";" + k.NegativniGlasovi.ToString() + ";" + k.Izmenjen.ToString() + ";" + k.Obrisan.ToString() + ";" + "nemaPodkomentara");

            sw1.Close();
            stream3.Close();
            return k;
        }


        [HttpGet]
        [ActionName("UzmiKomentare")]
        public List<Komentar> UzmiKomentare(string idTeme) {


            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            List<Komentar> listaKomentaraZaTemu = new List<Komentar>();
            List<Komentar> listaPodkomentara = new List<Komentar>();

            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                //ovaj splitter splituje komentare
                // Daj mi taj komentar samo ako nije obrisan
                if (splitter[1] == idTeme && splitter[9] == "False")
                {
                    listaPodkomentara = new List<Komentar>();
                    string[] ideviPodkomentara = splitter[10].Split('|');
                    foreach (string idPodkomentaraUKomentarima in ideviPodkomentara)
                    {
                        if (idPodkomentaraUKomentarima != "nemaPodkomentara")
                        {
                            // ideviPodkomentara predstavlja string id-eva podkomentara koji pripadaju tom komentaru
                            // prodji kroz sve podkomentare i napuni u listu samo one ciji je id == idPodkomentaraukomentarima



                            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/podkomentari.txt");
                            FileStream stream1 = new FileStream(dataFile1, FileMode.Open);
                            StreamReader sr1 = new StreamReader(stream1);

                            string podkomentarLinija = "";
                            while ((podkomentarLinija = sr1.ReadLine()) != null)
                            {

                                string[] podkomentarTokens = podkomentarLinija.Split(';');
                                // Vrati sve podkomentare koji nisu obrisani
                                if (podkomentarTokens[1] == idPodkomentaraUKomentarima && podkomentarTokens[8] == "False")
                                {
                                    Komentar podkomentar = new Komentar();
                                    podkomentar.Id = podkomentarTokens[1];
                                    podkomentar.RoditeljskiKomentar = podkomentarTokens[0];
                                    podkomentar.Autor = podkomentarTokens[2];
                                    podkomentar.DatumKomentara = DateTime.Parse(podkomentarTokens[3]);
                                    podkomentar.Tekst = podkomentarTokens[4];
                                    podkomentar.PozitivniGlasovi = Int32.Parse(podkomentarTokens[5]);
                                    podkomentar.NegativniGlasovi = Int32.Parse(podkomentarTokens[6]);
                                    podkomentar.Izmenjen = bool.Parse(podkomentarTokens[7]);
                                    podkomentar.Obrisan = bool.Parse(podkomentarTokens[8]);
                                    podkomentar.TemaKojojPripada = podkomentarTokens[9];

                                    listaPodkomentara.Add(podkomentar);
                                }
                            }
                            sr1.Close();
                            stream1.Close();
                        }

                    }
                    listaKomentaraZaTemu.Add(new Komentar(splitter[0], splitter[1], splitter[2], DateTime.Parse(splitter[3]), splitter[4], listaPodkomentara, splitter[5], Int32.Parse(splitter[6]), Int32.Parse(splitter[7]), bool.Parse(splitter[8]), bool.Parse(splitter[9])));
                }
            }

            sr.Close();
            stream.Close();
            return listaKomentaraZaTemu;

        }

        [HttpPost]
        [ActionName("DodajPodkomentar")]
        public Komentar DodajPodkomentar([FromBody]Komentar pk)
        {
            List<string> listaSvihKomentara = new List<string>();
            int brojac = 0;
            int indexZaIzmenu = -1;

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);


            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                // NE zaboravi: mora proci kroz sve da bi dodao u listuSvihKomentara, kako bi mogao celu listu ponovo da upisem
                listaSvihKomentara.Add(line);
                brojac++;

                string[] splitter = line.Split(';');
                if (splitter[0] == pk.RoditeljskiKomentar)
                {
                    indexZaIzmenu = brojac;
                }
            }
            sr.Close();
            stream.Close();
            // Upis u komentari.txt tj dodavanje novog podkomentara na kraj

            var dataFile2 = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");
            FileStream stream2 = new FileStream(dataFile2, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream2);

           

            pk.Id = Guid.NewGuid().ToString();
            pk.DatumKomentara = DateTime.Now;
            pk.Izmenjen = false;
            pk.NegativniGlasovi = 0;
            pk.PozitivniGlasovi = 0;
            pk.Obrisan = false;

            listaSvihKomentara[indexZaIzmenu - 1] += "|" + pk.Id;

            foreach (string komentar in listaSvihKomentara)
            {
                sw.WriteLine(komentar);
            }
            sw.Close();
            stream2.Close();

            // Upis u podkomentari.txt

            var dataFile3 = HttpContext.Current.Server.MapPath("~/App_Data/podkomentari.txt");
            FileStream stream3 = new FileStream(dataFile3, FileMode.Append, FileAccess.Write);
            StreamWriter sw3 = new StreamWriter(stream3);

            sw3.WriteLine(pk.RoditeljskiKomentar + ";" + pk.Id + ";" + pk.Autor + ";" + pk.DatumKomentara.ToShortDateString() + ";" + pk.Tekst + ";" + pk.PozitivniGlasovi.ToString() + ";" + pk.NegativniGlasovi.ToString() + ";" + pk.Izmenjen.ToString() + ";" + pk.Obrisan.ToString() + ";" + pk.TemaKojojPripada);

            sw3.Close();
            stream3.Close();
            return pk;
        }


        [HttpPost]
        [ActionName("LajkujKomentar")]
        public bool LajkujKomentar([FromBody]KomentarLikeDislikeRequest komentarRequest)
        {
            

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkKomentari.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr1 = new StreamReader(stream);


            List<string> listaSvih = new List<string>();
            string line = "";
            bool changed = false;
            while ((line = sr1.ReadLine()) != null)
            {
                bool isDisliked = false;

                string[] splitter = line.Split(';');
                // U slucaju da je vec lajkovao taj komentar vrati false
                if (splitter[0] == komentarRequest.KoVrsiAkciju && splitter[1] == komentarRequest.IdKomentara && splitter[2] == "like")
                {
                    sr1.Close();
                    stream.Close();
                    return false;
                }
                else if (splitter[0] == komentarRequest.KoVrsiAkciju && splitter[1] == komentarRequest.IdKomentara && splitter[2] == "dislike")
                {
                    isDisliked = true;
                    changed = true;
                    listaSvih.Add(komentarRequest.KoVrsiAkciju + ";" + komentarRequest.IdKomentara + ";like");

                }
                if (!isDisliked)
                {
                    listaSvih.Add(line);
                }

            }
            sr1.Close();
            stream.Close();

            if (!changed)
            {
               

                var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkKomentari.txt");
                FileStream stream1 = new FileStream(dataFile1, FileMode.Append, FileAccess.Write);
                StreamWriter sw1 = new StreamWriter(stream1);

                sw1.WriteLine(komentarRequest.KoVrsiAkciju + ";" + komentarRequest.IdKomentara + ";like");
                sw1.Close();
                stream1.Close();
            }
            else
            {
               

                var dataFile2 = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkKomentari.txt");
                FileStream stream2 = new FileStream(dataFile2, FileMode.Create, FileAccess.Write);
                StreamWriter sw2 = new StreamWriter(stream2);

                foreach (string lajkDislajk in listaSvih)
                {
                    sw2.WriteLine(lajkDislajk);
                }
                sw2.Close();
                stream2.Close();
            }
            // Nakon sto sam dodao u .txt fajl ko je lajkovao , sada nadji taj komentar i povecaj mu brojlajkovanih
         

            var dataFile3 = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");
            FileStream stream3 = new FileStream(dataFile3, FileMode.Open);
            StreamReader sr2 = new StreamReader(stream3);

            List<string> sviKomentari = new List<string>();

            string komentar = "";
            while ((komentar = sr2.ReadLine()) != null)
            {
                bool nadjena = false;

                string[] komentarTokens = komentar.Split(';');
                if (komentarTokens[0] == komentarRequest.IdKomentara)
                {
                    // nasli smo komentar kome treba povecati pozitivne glasove
                    nadjena = true;
                    int brojTrenutnoPozitivnih = Int32.Parse(komentarTokens[6]);
                    int brojTrenutnoNegativnih = Int32.Parse(komentarTokens[7]);
                    brojTrenutnoPozitivnih++;
                    if (changed)
                    {
                        brojTrenutnoNegativnih--;
                    }
                    sviKomentari.Add(komentarTokens[0] + ";" + komentarTokens[1] + ";" + komentarTokens[2] + ";" + komentarTokens[3] + ";" + komentarTokens[4] + ";" + komentarTokens[5] + ";" + brojTrenutnoPozitivnih.ToString() + ";" + brojTrenutnoNegativnih.ToString() + ";" + komentarTokens[8] + ";" + komentarTokens[9] + ";" + komentarTokens[10]);

                }
                if (!nadjena)
                {
                    sviKomentari.Add(komentar);
                }
            }
            sr2.Close();
            stream3.Close();

            var dataFile4 = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");
            FileStream stream4 = new FileStream(dataFile4, FileMode.Create, FileAccess.Write);
            StreamWriter sw3 = new StreamWriter(stream4);

           


            foreach (string linijaKomentara in sviKomentari)
            {
                sw3.WriteLine(linijaKomentara);
            }
            sw3.Close();
            stream4.Close();

            // Sada sve ovo isto za podkomentare
            // Nakon sto sam dodao u .txt fajl ko je lajkovao , sada nadji taj PODKOMENTAR i povecaj mu brojlajkovanih

            var dataFile5 = HttpContext.Current.Server.MapPath("~/App_Data/podkomentari.txt");
            FileStream stream5 = new FileStream(dataFile5, FileMode.Open);
            StreamReader sr3 = new StreamReader(stream5);

            List<string> sviPodkomentari = new List<string>();

            string podkomentar = "";
            while ((podkomentar = sr3.ReadLine()) != null)
            {
                bool nadjena = false;

                string[] podkomentarTokens = podkomentar.Split(';');
                if (podkomentarTokens[1] == komentarRequest.IdKomentara)
                {
                    // nasli smo komentar kome treba povecati pozitivne glasove
                    nadjena = true;
                    int brojTrenutnoPozitivnih = Int32.Parse(podkomentarTokens[5]);
                    int brojTrenutnoNegativnih = Int32.Parse(podkomentarTokens[6]);
                    brojTrenutnoPozitivnih++;
                    if (changed)
                    {
                        brojTrenutnoNegativnih--;
                    }
                    sviPodkomentari.Add(podkomentarTokens[0] + ";" + podkomentarTokens[1] + ";" + podkomentarTokens[2] + ";" + podkomentarTokens[3] + ";" + podkomentarTokens[4] + ";" + brojTrenutnoPozitivnih.ToString() + ";" + brojTrenutnoNegativnih.ToString() + ";" + podkomentarTokens[7] + ";" + podkomentarTokens[8] + ";" + podkomentarTokens[9]);

                }
                if (!nadjena)
                {
                    sviPodkomentari.Add(podkomentar);
                }
            }
            sr3.Close();
            stream5.Close();

            var dataFile6 = HttpContext.Current.Server.MapPath("~/App_Data/podkomentari.txt");
            FileStream stream6 = new FileStream(dataFile6, FileMode.Create, FileAccess.Write);
            StreamWriter sw4 = new StreamWriter(stream6);

            foreach (string linijaKomentara in sviPodkomentari)
            {
                sw4.WriteLine(linijaKomentara);
            }
           sw4.Close();
           stream6.Close();

            return true;
        }


        [HttpPost]
        [ActionName("DislajkujKomentar")]
        public bool DislajkujKomentar([FromBody]KomentarLikeDislikeRequest komentarRequest)
        {
         

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkKomentari.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr1 = new StreamReader(stream);

            List<string> listaSvih = new List<string>();
            string line = "";
            bool changed = false;
            while ((line = sr1.ReadLine()) != null)
            {
                bool isDisliked = false;

                string[] splitter = line.Split(';');
                // U slucaju da je vec dislajkovao taj komentar vrati false
                if (splitter[0] == komentarRequest.KoVrsiAkciju && splitter[1] == komentarRequest.IdKomentara && splitter[2] == "dislike")
                {
                    sr1.Close();
                    stream.Close();
                    return false;
                }
                else if (splitter[0] == komentarRequest.KoVrsiAkciju && splitter[1] == komentarRequest.IdKomentara && splitter[2] == "like")
                {
                    isDisliked = true;
                    changed = true;
                    listaSvih.Add(komentarRequest.KoVrsiAkciju + ";" + komentarRequest.IdKomentara + ";dislike");

                }
                if (!isDisliked)
                {
                    listaSvih.Add(line);
                }

            }
            sr1.Close();
            stream.Close();

            if (!changed)
            {
               

                var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkKomentari.txt");
                FileStream stream1 = new FileStream(dataFile1, FileMode.Append, FileAccess.Write);
                StreamWriter sw1 = new StreamWriter(stream1);

                sw1.WriteLine(komentarRequest.KoVrsiAkciju + ";" + komentarRequest.IdKomentara + ";dislike");
                sw1.Close();
                stream1.Close();
            }
            else
            {
               

                var dataFile2 = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkKomentari.txt");
                FileStream stream2 = new FileStream(dataFile2, FileMode.Create, FileAccess.Write);
                StreamWriter sw2 = new StreamWriter(stream2);

                foreach (string lajkDislajk in listaSvih)
                {
                    sw2.WriteLine(lajkDislajk);
                }
                sw2.Close();
                stream2.Close();
            }
            // Nakon sto sam dodao u .txt fajl ko je dislajkovao , sada nadji taj komentar i povecaj mu brojlajkovanih


           

            var dataFile3 = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");
            FileStream stream3 = new FileStream(dataFile3, FileMode.Open);
            StreamReader sr2 = new StreamReader(stream3);

            List<string> sviKomentari = new List<string>();

            string komentar = "";
            while ((komentar = sr2.ReadLine()) != null)
            {
                bool nadjena = false;

                string[] komentarTokens = komentar.Split(';');
                if (komentarTokens[0] == komentarRequest.IdKomentara)
                {
                    // nasli smo komentar kome treba povecati pozitivne glasove
                    nadjena = true;
                    int brojTrenutnoPozitivnih = Int32.Parse(komentarTokens[6]);
                    int brojTrenutnoNegativnih = Int32.Parse(komentarTokens[7]);
                    brojTrenutnoNegativnih++;
                    if (changed)
                    {
                        brojTrenutnoPozitivnih--;
                    }
                    sviKomentari.Add(komentarTokens[0] + ";" + komentarTokens[1] + ";" + komentarTokens[2] + ";" + komentarTokens[3] + ";" + komentarTokens[4] + ";" + komentarTokens[5] + ";" + brojTrenutnoPozitivnih.ToString() + ";" + brojTrenutnoNegativnih.ToString() + ";" + komentarTokens[8] + ";" + komentarTokens[9] + ";" + komentarTokens[10]);

                }
                if (!nadjena)
                {
                    sviKomentari.Add(komentar);
                }
            }
            sr2.Close();
            stream3.Close();

            var dataFile4 = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");
            FileStream stream4 = new FileStream(dataFile4, FileMode.Create, FileAccess.Write);
            StreamWriter sw3 = new StreamWriter(stream4);


            
            foreach (string linijaKomentara in sviKomentari)
            {
                sw3.WriteLine(linijaKomentara);
            }
            sw3.Close();
            stream4.Close();

            // Sada sve ovo isto za podkomentare
            // Nakon sto sam dodao u .txt fajl ko je dislajkovao , sada nadji taj PODKOMENTAR i povecaj mu brojlajkovanih
           
            var dataFile5 = HttpContext.Current.Server.MapPath("~/App_Data/podkomentari.txt");
            FileStream stream5 = new FileStream(dataFile5, FileMode.Open);
            StreamReader sr3 = new StreamReader(stream5);

            List<string> sviPodkomentari = new List<string>();

            string podkomentar = "";
            while ((podkomentar = sr3.ReadLine()) != null)
            {
                bool nadjena = false;

                string[] podkomentarTokens = podkomentar.Split(';');
                if (podkomentarTokens[1] == komentarRequest.IdKomentara)
                {
                    // nasli smo komentar kome treba povecati pozitivne glasove
                    nadjena = true;
                    int brojTrenutnoPozitivnih = Int32.Parse(podkomentarTokens[5]);
                    int brojTrenutnoNegativnih = Int32.Parse(podkomentarTokens[6]);
                    brojTrenutnoNegativnih++;
                    if (changed)
                    {
                        brojTrenutnoPozitivnih--;
                    }
                    sviPodkomentari.Add(podkomentarTokens[0] + ";" + podkomentarTokens[1] + ";" + podkomentarTokens[2] + ";" + podkomentarTokens[3] + ";" + podkomentarTokens[4] + ";" + brojTrenutnoPozitivnih.ToString() + ";" + brojTrenutnoNegativnih.ToString() + ";" + podkomentarTokens[7] + ";" + podkomentarTokens[8] + ";" + podkomentarTokens[9]);

                }
                if (!nadjena)
                {
                    sviPodkomentari.Add(podkomentar);
                }
            }
            sr3.Close();
            stream5.Close();

            var dataFile6 = HttpContext.Current.Server.MapPath("~/App_Data/podkomentari.txt");
            FileStream stream6 = new FileStream(dataFile6, FileMode.Create, FileAccess.Write);
            StreamWriter sw4 = new StreamWriter(stream6);

            

            foreach (string linijaKomentara in sviPodkomentari)
            {
                sw4.WriteLine(linijaKomentara);
            }
            sw4.Close();
            stream6.Close();

            return true;
        }

        [HttpPost]
        [ActionName("SacuvajKomentar")]
        public bool SacuvajKomentar([FromBody]KomentarZaCuvanje komentarZaCuvanje)
        {

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr1 = new StreamReader(stream);

            List<string> listaSvihKorisnika = new List<string>();
            int brojac = 0;
            int indexZaIzmenu = -1;
            string line = "";
            while ((line = sr1.ReadLine()) != null)
            {
                listaSvihKorisnika.Add(line);
                brojac++;

                string[] splitter = line.Split(';');
                if (splitter[0] == komentarZaCuvanje.KoCuva)
                {
                    indexZaIzmenu = brojac;
                }
            }
            sr1.Close();
            stream.Close();

            // splituj tu liniju koja treba da se menja tj na koju treba da se dodaje
            string[] tokeniOdabranogKorisnika = listaSvihKorisnika[indexZaIzmenu - 1].Split(';');
            // tokeniOdabranogKorisnika[10] tu se nalazi spisak pracenih komentara
            string[] splitterProvere = tokeniOdabranogKorisnika[10].Split('|');
            // provera ukoliko korisnik vec prati postojeci podforum
            foreach (string idKomentara in splitterProvere)
            {
                if (idKomentara == komentarZaCuvanje.IdKomentara)
                {
                    return false;
                }
            }
            // otvori bulk writer
           

            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Create, FileAccess.Write);
            StreamWriter sw1 = new StreamWriter(stream1);


            // tokeniOdabranogKorisnika[10] tu se nalazi spisak pracenih komentara
            tokeniOdabranogKorisnika[10] += "|" + komentarZaCuvanje.IdKomentara;

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
        [ActionName("UzmiSacuvaneKomentare")]
        public List<Komentar> UzmiSacuvaneKomentare(string username)
        {
            
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<Komentar> listaSacuvanihKomentara = new List<Komentar>();

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[0] == username)
                {
                    string[] komentariSplitter = splitter[10].Split('|');
                    foreach (string komentarId in komentariSplitter)
                    {
                        if (komentarId != "nemaSnimljenihKomentara")
                        {
                            // Prodji kroz sve komentare, nadji taj sa tim id-em, napravi novi komentar, od podataka i dodaj ga u listu
                            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");
                            FileStream stream1 = new FileStream(dataFile1, FileMode.Open);
                            StreamReader sr1 = new StreamReader(stream1);

                            string komentarLine = "";
                            while ((komentarLine = sr1.ReadLine()) != null)
                            {
                                string[] komentarLineSplitter = komentarLine.Split(';');
                                if (komentarLineSplitter[0] == komentarId && komentarLineSplitter[9] == "False")
                                {
                                    // NOTE: kada dodajem ovde komentar, necu dodavati njegove podkomentare, posto to nije bitno za tu stranicu, korisnik treba samo da ima uvid u jedan komentar
                                    listaSacuvanihKomentara.Add(new Komentar(komentarLineSplitter[0], komentarLineSplitter[1], komentarLineSplitter[2], DateTime.Parse(komentarLineSplitter[3]), komentarLineSplitter[4], new List<Komentar>(), komentarLineSplitter[5], Int32.Parse(komentarLineSplitter[6]), Int32.Parse(komentarLineSplitter[7]), bool.Parse(komentarLineSplitter[8]), bool.Parse(komentarLineSplitter[9])));
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
            return listaSacuvanihKomentara;
        }

        [HttpGet]
        [ActionName("UzmiSacuvanePodkomentare")]
        public List<Komentar> UzmiSacuvanePodkomentare(string username)
        {
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<Komentar> listaSacuvanihKomentara = new List<Komentar>();

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[0] == username)
                {
                    string[] komentariSplitter = splitter[10].Split('|');
                    foreach (string komentarId in komentariSplitter)
                    {
                        if (komentarId != "nemaSnimljenihKomentara")
                        {
                            // Prodji kroz sve podkomentare, nadji taj sa tim id-em, napravi novi komentar, od podataka i dodaj ga u listu
      
                            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/podkomentari.txt");
                            FileStream stream1 = new FileStream(dataFile1, FileMode.Open);
                            StreamReader sr1 = new StreamReader(stream1);

                            string komentarLine = "";
                            while ((komentarLine = sr1.ReadLine()) != null)
                            {
                                string[] komentarLineSplitter = komentarLine.Split(';');
                                if (komentarLineSplitter[1] == komentarId && komentarLineSplitter[8] == "False")
                                {
                                    // evenutalno: prodji kroz sve teme i pogledaj da li komentarLineSplitter[9] odgovara nekoj
                                    // ako ne odgovara nijednoj, to znaci da ta tema ne postoji tj da je obrisana i nemoj dodati ovaj
                                    // podkomentar u listu

                                    Komentar podkomentar = new Komentar();

                                    podkomentar.RoditeljskiKomentar = komentarLineSplitter[0];
                                    podkomentar.Id = komentarLineSplitter[1];

                                    podkomentar.Autor = komentarLineSplitter[2];
                                    podkomentar.DatumKomentara = DateTime.Parse(komentarLineSplitter[3]);
                                    podkomentar.Tekst = komentarLineSplitter[4];
                                    podkomentar.PozitivniGlasovi = Int32.Parse(komentarLineSplitter[5]);
                                    podkomentar.NegativniGlasovi = Int32.Parse(komentarLineSplitter[6]);
                                    podkomentar.Izmenjen = bool.Parse(komentarLineSplitter[7]);
                                    podkomentar.Obrisan = bool.Parse(komentarLineSplitter[8]);
                                    podkomentar.TemaKojojPripada = komentarLineSplitter[9];

                                    listaSacuvanihKomentara.Add(podkomentar);
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
            return listaSacuvanihKomentara;
        }

        [HttpGet]
        [ActionName("UzmiLajkovaniKomentari")]
        public List<Komentar> UzmiLajkovaniKomentari(string username)
        {
            List<Komentar> listaLajkovanihKomentara = new List<Komentar>();

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkKomentari.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<string> ideviLajkovanih = new List<string>();
            string komentarLine = "";
            while ((komentarLine = sr.ReadLine()) != null)
            {
                string[] splitter = komentarLine.Split(';');
                if (splitter[2] == "like" && splitter[0] == username)
                {
                    ideviLajkovanih.Add(splitter[1]);
                }

            }
            sr.Close();
            stream.Close();

            if (ideviLajkovanih.Count == 0) return listaLajkovanihKomentara;
            // prodji kroz komentari.txt i uzmi te sa tim idem
            
            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Open);
            StreamReader sr1 = new StreamReader(stream1);


            string komLine = "";
            while ((komLine = sr1.ReadLine()) != null)
            {
                string[] splitter = komLine.Split(';');
                bool postojiULajkovanim = ideviLajkovanih.Any(idKomentara => idKomentara == splitter[0]);
                // ako postoji u lajkovanim i nije obrisan dodaj ga u listu lajkovanih
                if (postojiULajkovanim && splitter[9] == "False")
                {
                    Komentar k = new Komentar();
                    k.Id = splitter[0];
                    k.TemaKojojPripada = splitter[1];
                    k.Autor = splitter[2];
                    k.DatumKomentara = DateTime.Parse(splitter[3]);
                    k.RoditeljskiKomentar = splitter[4];
                    k.Tekst = splitter[5];
                    k.PozitivniGlasovi = Int32.Parse(splitter[6]);
                    k.NegativniGlasovi = Int32.Parse(splitter[7]);
                    k.Izmenjen = bool.Parse(splitter[8]);
                    k.Obrisan = bool.Parse(splitter[9]);

                    listaLajkovanihKomentara.Add(k);
                }
            }
            sr1.Close();
            stream1.Close();

            return listaLajkovanihKomentara;
        }

        [HttpGet]
        [ActionName("UzmiDislajkovaniKomentari")]
        public List<Komentar> UzmiDislajkovaniKomentari(string username)
        {

            List<Komentar> listaDislajkovanihKomentara = new List<Komentar>();

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkKomentari.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<string> ideviDislajkovanih = new List<string>();
            string komentarLine = "";
            while ((komentarLine = sr.ReadLine()) != null)
            {
                string[] splitter = komentarLine.Split(';');
                if (splitter[2] == "dislike" && splitter[0] == username)
                {
                    ideviDislajkovanih.Add(splitter[1]);
                }

            }
            sr.Close();
            stream.Close();

            if (ideviDislajkovanih.Count == 0) return listaDislajkovanihKomentara;
            // prodji kroz komentari.txt i uzmi te sa tim idem
            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/komentari.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Open);
            StreamReader sr1 = new StreamReader(stream1);

            string komLine = "";
            while ((komLine = sr1.ReadLine()) != null)
            {
                string[] splitter = komLine.Split(';');
                bool postojiUDislajkovanim = ideviDislajkovanih.Any(idKomentara => idKomentara == splitter[0]);
                // ako postoji u dislajkovanima i nije obrisan dodaj ga u listu dislajkovanih
                if (postojiUDislajkovanim && splitter[9] == "False")
                {
                    Komentar k = new Komentar();
                    k.Id = splitter[0];
                    k.TemaKojojPripada = splitter[1];
                    k.Autor = splitter[2];
                    k.DatumKomentara = DateTime.Parse(splitter[3]);
                    k.RoditeljskiKomentar = splitter[4];
                    k.Tekst = splitter[5];
                    k.PozitivniGlasovi = Int32.Parse(splitter[6]);
                    k.NegativniGlasovi = Int32.Parse(splitter[7]);
                    k.Izmenjen = bool.Parse(splitter[8]);
                    k.Obrisan = bool.Parse(splitter[9]);

                    listaDislajkovanihKomentara.Add(k);
                }
            }
            sr1.Close();
            stream1.Close();

            return listaDislajkovanihKomentara;
        }
    }
}
