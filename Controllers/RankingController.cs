using Prueba_1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Prueba_1.Controllers
{
    public class RankingController : Controller
    {
        //public string str_Url = "https://raw.githubusercontent.com/EvanLi/Github-Ranking/master/Data/github-ranking-2022-02-26.csv";
        // GET: Ranking
        public List<Rank> lstFinal = new List<Rank>();
        public ActionResult Index()
        {

            RankingController rK = new RankingController();
            List<Rank> r = new List<Rank>();
            r = rK.SplitCSV();
            ViewBag.Lst = r;
            ViewBag.Lst_ti = rK.Lista();

            return View();

        }

        public string GetCSV(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();

            return results;
        }

        public List<Rank> SplitCSV()
        {
            RankingController rK = new RankingController();


            List<Rank> splitted = new List<Rank>();
            string fileList = rK.GetCSV("https://raw.githubusercontent.com/EvanLi/Github-Ranking/master/Data/github-ranking-2022-02-26.csv");
            string[] tempStr;

            tempStr = fileList.Split('\n');
            string[] tempStr2 = null;

            foreach (string item in tempStr)
            {
                tempStr2 = item.Split(',');

                Rank rank = new Rank();
                if (!string.IsNullOrEmpty(tempStr2[0]) && !string.IsNullOrEmpty(tempStr2[1]))
                {
                    rank.rank = tempStr2[0];
                    rank.item = tempStr2[1];
                    rank.repo_name = tempStr2[2];
                    rank.stars = tempStr2[3];

                    splitted.Add(rank);

                }

            }
            splitted.RemoveAt(0);
            return splitted;
        }

        // metodo que filtra segun los criterios marcados
        public ActionResult Filter(resp ra)
        {
            var lstRes = new List<Rank>();
            RankingController rK = new RankingController();
            ra.Value = int.Parse(Request["Lst_ti"]);
            var lst_filter = rK.SplitCSV();


            switch (ra.Value)
            {
                case 0:
                    lstRes = lst_filter.FindAll(x => x.rank.ToLower() == ra.search.ToLower());
                    break;
                case 1:
                    lstRes = lst_filter.FindAll(x => x.item.ToLower() == ra.search.ToLower());
                    break;
                case 2:
                    lstRes = lst_filter.FindAll(x => x.repo_name.ToLower() == ra.search.ToLower());
                    break;
                case 3:
                    lstRes = lst_filter.FindAll(x => x.stars.ToLower() == ra.search.ToLower());
                    break;
            }

            if (lstRes.Count < ra.cantidad)
            {
                ra.cantidad = lstRes.Count;
            }

            if (ra.cantidad > 0)
            {
                for (int i = 0; i < ra.cantidad; i++)
                {
                    lstFinal.Add(lstRes[i]);
                }
            }
            else
            {
                lstFinal = lstRes;
            }

            //ordena de manera ascendente si marco el check
            if (ra.orden == true)
                lstFinal = lstFinal.OrderBy(x => x.repo_name).ToList();


            ViewBag.Lst_Final = lstFinal;


            return View();
        }

        public List<SelectListItem> Lista()
        {

            return new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Selecione una Opción",
                    Value = "-1"
                },
                new SelectListItem()
                {
                    Text = "Rank",
                    Value = "0"
                },
                 new SelectListItem()
                {
                    Text = "Item",
                    Value = "1"
                }, new SelectListItem()
                {
                    Text = "repo_name",
                    Value = "2"
                }, new SelectListItem()
                {
                    Text = "stars",
                    Value = "3"
                },
            };



        }



    }
}