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
    }
}
