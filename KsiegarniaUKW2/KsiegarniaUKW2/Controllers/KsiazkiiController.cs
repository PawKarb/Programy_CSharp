using KsiegarniaUKW2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KsiegarniaUKW2.Controllers
{
    public class KsiazkiiController : Controller
    {
        private Ksiazkicontext db = new Ksiazkicontext();
        // GET: Ksiazkii
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista(string nazwaKategori)
        {
            var kategoria = db.Kategorie.Include("Ksiazki").Where(k => k.NazwaKategorii.ToUpper() == nazwaKategori.ToUpper()).Single();
            var ksiazki = kategoria.Ksiazki.ToList();
            return View(ksiazki);
        }
        public ActionResult Szczegoly(int id)
        {
            var ksiazka = db.Ksiazki.Find(id);
            return View(ksiazka);
        }

        [ChildActionOnly]

        [OutputCache(Duration =5000)]

        public ActionResult KategorieMenu()
        {

            var kategorie = db.Kategorie.ToList();

            return PartialView("_KategorieMenu", kategorie);
        }
    }
}