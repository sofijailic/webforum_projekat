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

        [HttpPost]
        [ActionName("LajkujTemu")]
        public bool LajkujTemu([FromBody]TemaLikeDislikeRequest temaRequest)
        {
            
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkTeme.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<string> listaSvih = new List<string>();

            string[] temaRequestSplit = temaRequest.PunNazivTeme.Split('-');
            string podforumKomePripada = temaRequestSplit[0];
            string naslovTeme = temaRequestSplit[1];

            string line = "";
            bool changed = false;
            while ((line = sr.ReadLine()) != null)
            {
                bool isDisliked = false;

                string[] splitter = line.Split(';');
                // U slucaju da je vec lajkovao tu temu vrati false
                if (splitter[0] == temaRequest.KoVrsiAkciju && splitter[1] == temaRequest.PunNazivTeme && splitter[2] == "like")
                {
                    sr.Close();
                    stream.Close();
                    return false;
                }
                else if (splitter[0] == temaRequest.KoVrsiAkciju && splitter[1] == temaRequest.PunNazivTeme && splitter[2] == "dislike")
                {
                    isDisliked = true;
                    changed = true;
                    listaSvih.Add(temaRequest.KoVrsiAkciju + ";" + temaRequest.PunNazivTeme + ";like");

                }
                if (!isDisliked)
                {
                    listaSvih.Add(line);
                }

            }
            sr.Close();
            stream.Close();

            if (!changed)
            {
                

                var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkTeme.txt");
                FileStream stream2 = new FileStream(dataFile1, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(stream2);


                sw.WriteLine(temaRequest.KoVrsiAkciju + ";" + temaRequest.PunNazivTeme + ";like");
                sw.Close();
                stream2.Close();
            }
            else
            {
                
                var dataFile2 = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkTeme.txt");
                FileStream stream3 = new FileStream(dataFile2, FileMode.Create, FileAccess.Write);
                StreamWriter sw2 = new StreamWriter(stream3);

                foreach (string lajkDislajk in listaSvih)
                {
                    sw2.WriteLine(lajkDislajk);
                }
                sw2.Close();
                stream3.Close();
            }
            // Nakon sto sam dodao u .txt fajl ko je lajkovao , sada nadji tu temu i povecaj joj brojlajkovanih
            

            var dataFile3 = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream4 = new FileStream(dataFile3, FileMode.Open);
            StreamReader sr2 = new StreamReader(stream4);

            List<string> sveTeme = new List<string>();

            string tema = "";
            while ((tema = sr2.ReadLine()) != null)
            {
                bool nadjena = false;

                string[] temaTokens = tema.Split(';');
                if (temaTokens[0] == podforumKomePripada && temaTokens[1] == naslovTeme)
                {
                    // nasli smo temu kojoj treba povecati pozitivne glasove
                    nadjena = true;
                    int brojTrenutnoPozitivnih = Int32.Parse(temaTokens[6]);
                    int brojTrenutnoNegativnih = Int32.Parse(temaTokens[7]);
                    brojTrenutnoPozitivnih++;
                    if (changed)
                    {
                        brojTrenutnoNegativnih--;
                    }
                    sveTeme.Add(temaTokens[0] + ";" + temaTokens[1] + ";" + temaTokens[2] + ";" + temaTokens[3] + ";" + temaTokens[4] + ";" + temaTokens[5] + ";" + brojTrenutnoPozitivnih.ToString() + ";" + brojTrenutnoNegativnih.ToString() + ";" + temaTokens[8]);

                }
                if (!nadjena)
                {
                    sveTeme.Add(tema);
                }
            }
            sr2.Close();
            stream4.Close();


            var dataFile4 = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream5 = new FileStream(dataFile4, FileMode.Create, FileAccess.Write);
            StreamWriter sw3 = new StreamWriter(stream5);

            foreach (string linijaTeme in sveTeme)
            {
                sw3.WriteLine(linijaTeme);
            }
            sw3.Close();
            stream5.Close();

            return true;

        }

        [HttpPost]
        [ActionName("DislajkujTemu")]
        public bool DislajkujTemu([FromBody]TemaLikeDislikeRequest temaRequest) {


            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkTeme.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Open);
            StreamReader sr1 = new StreamReader(stream1);

            List<string> listaSvih = new List<string>();

            string[] temaRequestSplit = temaRequest.PunNazivTeme.Split('-');
            string podforumKomePripada = temaRequestSplit[0];
            string naslovTeme = temaRequestSplit[1];

            string line = "";
            bool changed = false;
            while ((line = sr1.ReadLine()) != null)
            {
                bool isLiked = false;

                string[] splitter = line.Split(';');
                // U slucaju da je vec dislajkovao tu temu vrati false
                if (splitter[0] == temaRequest.KoVrsiAkciju && splitter[1] == temaRequest.PunNazivTeme && splitter[2] == "dislike")
                {
                    sr1.Close();
                    stream1.Close();
                    return false;
                }
                else if (splitter[0] == temaRequest.KoVrsiAkciju && splitter[1] == temaRequest.PunNazivTeme && splitter[2] == "like")
                {
                    isLiked = true;
                    changed = true;
                    listaSvih.Add(temaRequest.KoVrsiAkciju + ";" + temaRequest.PunNazivTeme + ";dislike");

                }
                if (!isLiked)
                {
                    listaSvih.Add(line);
                }

            }
            sr1.Close();
            stream1.Close();

            if (!changed)
            {
               

                var dataFile2 = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkTeme.txt");
                FileStream stream2 = new FileStream(dataFile2, FileMode.Append, FileAccess.Write);
                StreamWriter sw1 = new StreamWriter(stream2);

                sw1.WriteLine(temaRequest.KoVrsiAkciju + ";" + temaRequest.PunNazivTeme + ";dislike");
                sw1.Close();
                stream2.Close();
            }
            else
            {
                

                var dataFile3 = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkTeme.txt");
                FileStream stream3 = new FileStream(dataFile3, FileMode.Create, FileAccess.Write);
                StreamWriter sw2 = new StreamWriter(stream3);


                foreach (string lajkDislajk in listaSvih)
                {
                    sw2.WriteLine(lajkDislajk);
                }
                sw2.Close();
                stream3.Close();
            }
            // Nakon sto sam dodao u .txt fajl ko je dislajkovao , sada nadji tu temu i povecaj joj brojDislajkovanih


            

            var dataFile4 = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream4 = new FileStream(dataFile4, FileMode.Open);
            StreamReader sr2 = new StreamReader(stream4);

            List<string> sveTeme = new List<string>();

            string tema = "";
            while ((tema = sr2.ReadLine()) != null)
            {
                bool nadjena = false;

                string[] temaTokens = tema.Split(';');
                if (temaTokens[0] == podforumKomePripada && temaTokens[1] == naslovTeme)
                {
                    // nasli smo temu kojoj treba povecati negativne glasove
                    nadjena = true;
                    int brojTrenutnoPozitivnih = Int32.Parse(temaTokens[6]);
                    int brojTrenutnoNegativnih = Int32.Parse(temaTokens[7]);
                    brojTrenutnoNegativnih++;
                    if (changed)
                    {
                        brojTrenutnoPozitivnih--;
                    }
                    sveTeme.Add(temaTokens[0] + ";" + temaTokens[1] + ";" + temaTokens[2] + ";" + temaTokens[3] + ";" + temaTokens[4] + ";" + temaTokens[5] + ";" + brojTrenutnoPozitivnih.ToString() + ";" + brojTrenutnoNegativnih.ToString() + ";" + temaTokens[8]);

                }
                if (!nadjena)
                {
                    sveTeme.Add(tema);
                }
            }
            sr2.Close();
            stream4.Close();


            var dataFile5 = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
            FileStream stream5 = new FileStream(dataFile5, FileMode.Create, FileAccess.Write);
            StreamWriter sw3 = new StreamWriter(stream5);


            foreach (string linijaTeme in sveTeme)
            {
                sw3.WriteLine(linijaTeme);
            }
            sw3.Close();
            stream5.Close();

            return true;
        }


        [HttpPost]
        [ActionName("SacuvajTemu")]
        public bool SacuvajTemu([FromBody]TemaZaCuvanje temaZaCuvanje)
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
                if (splitter[0] == temaZaCuvanje.KorisnikKojiPrati)
                {
                    indexZaIzmenu = brojac;
                }
            }
            sr.Close();
            stream.Close();

            // splituj tu liniju koja treba da se menja tj na koju treba da se dodaje
            string[] tokeniOdabranogKorisnika = listaSvihKorisnika[indexZaIzmenu - 1].Split(';');
            // tokeniOdabranogKorisnika[9] tu se nalazi spisak pracenih tema
            string[] splitterProvere = tokeniOdabranogKorisnika[9].Split('|');
            // provera ukoliko korisnik vec prati postojeci podforum
            foreach (string pracenaTema in splitterProvere)
            {
                if (pracenaTema == temaZaCuvanje.NaslovTeme)
                {
                    return false;
                }
            }
            // otvori bulk writer
           

            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream1 = new FileStream(dataFile1, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream1);

            // tokeniOdabranogKorisnika[9] tu se nalazi spisak pracenih tema
            tokeniOdabranogKorisnika[9] += "|" + temaZaCuvanje.NaslovTeme;

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
                sw.WriteLine(korisnickaLinija);
            }
            sw.Close();
            stream1.Close();

            return true;
        }

        [HttpGet]
        [ActionName("UzmiSacuvaneTeme")]
        public List<Tema> UzmiSacuvaneTeme(string username)
        {
           
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            List<Tema> listaSacuvanihTema = new List<Tema>();

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[0] == username)
                {
                    string[] temeSplitter = splitter[9].Split('|');
                    foreach (string temaId in temeSplitter)
                    {
                        if (temaId != "nemaSnimljenihTema")
                        {
                            // Prodji kroz sve teme, nadji tu sa tim imenom, napravi novu temu, od podataka i dodaj je u listu

                            var dataFile1 = HttpContext.Current.Server.MapPath("~/App_Data/teme.txt");
                            FileStream stream1 = new FileStream(dataFile1, FileMode.Open);
                            StreamReader sr1 = new StreamReader(stream1);

                            string temaLine = "";
                            while ((temaLine = sr1.ReadLine()) != null)
                            {
                                string[] temaLineSplitter = temaLine.Split(';');
                                string[] podforumTema = temaId.Split('-');

                                if (temaLineSplitter[0] == podforumTema[0] && temaLineSplitter[1] == podforumTema[1])
                                {
                                    // NOTE: kada dodajem temu u listu pracenih tema , stavim da nema ni jedan komentar, posto mi ne trebaju komentari kada budem ispisivao samo teme
                                    // kada se klikne na tu temu on ga vodi i sve fino
                                    listaSacuvanihTema.Add(new Tema(temaLineSplitter[0], temaLineSplitter[1], temaLineSplitter[2], temaLineSplitter[3], temaLineSplitter[4], DateTime.Parse(temaLineSplitter[5]), Int32.Parse(temaLineSplitter[6]), Int32.Parse(temaLineSplitter[7]), new List<string>()));
                                    break; // tema nadjena pici sledeci foreach
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
            return listaSacuvanihTema;
        }

        [HttpGet]
        [ActionName("UzmiLajkovaneTeme")]
        public List<string> UzmiLajkovaneTeme(string username)
        {
            List<string> listaLajkovanihTema = new List<string>();

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkTeme.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[0] == username && splitter[2] == "like")
                {
                    listaLajkovanihTema.Add(splitter[1]);
                }
            }
            sr.Close();
            stream.Close();

            return listaLajkovanihTema;
        }

        [HttpGet]
        [ActionName("UzmiDislajkovaneTeme")]
        public List<string> UzmiDislajkovaneTeme(string username)
        {
            List<string> listaDislajkovanihTema = new List<string>();

            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/lajkDislajkTeme.txt");
            FileStream stream = new FileStream(dataFile, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitter = line.Split(';');
                if (splitter[0] == username && splitter[2] == "dislike")
                {
                    listaDislajkovanihTema.Add(splitter[1]);
                }
            }
            sr.Close();
            stream.Close();

            return listaDislajkovanihTema;
        }
    }
}
