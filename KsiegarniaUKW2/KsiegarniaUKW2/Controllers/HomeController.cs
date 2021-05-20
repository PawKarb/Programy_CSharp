using KsiegarniaUKW2.DAL;
using KsiegarniaUKW2.Models;
using KsiegarniaUKW2.Struktura;
using KsiegarniaUKW2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KsiegarniaUKW2.Controllers
{
    public class HomeController : Controller
    {
        private Ksiazkicontext db = new Ksiazkicontext();

        public ActionResult Index()
        {
            


            ICacheProvider cache = new DefaultCacheProvider();

            List<Ksiazka> nowosci;

            if(cache.IsSet(Const.NowosciCacheKey))
            {
                nowosci = cache.Get(Const.NowosciCacheKey) as List<Ksiazka>;

            } else
            {
                nowosci = db.Ksiazki.Where(a => !a.Ukryty).OrderByDescending(a => a.DataDodania).Take(3).ToList();
                cache.Set(Const.NowosciCacheKey, nowosci, 60);

            }

            List<Ksiazka> Hit;

            if (cache.IsSet(Const.HitCacheKey))
            {
                Hit = cache.Get(Const.HitCacheKey) as List<Ksiazka>;

            }
            else
            {
                Hit = db.Ksiazki.Where(a => !a.Ukryty && a.Hit).OrderBy(a => Guid.NewGuid()).Take(3).ToList();
                cache.Set(Const.HitCacheKey, Hit, 60);

            }

            List<Kategoria> kategorie;

            if (cache.IsSet(Const.KategorieCacheKey))
            {
                kategorie = cache.Get(Const.KategorieCacheKey) as List<Kategoria>;

            }
            else
            {
                kategorie = db.Kategorie.ToList();
             
                cache.Set(Const.KategorieCacheKey, kategorie, 60);

            }

            var vm = new HomeViewModel()
            {
                Kategorie = kategorie,
                Nowosci = nowosci,
                Bestsellery = Hit,
            };
            return View(vm);
        }

        public ActionResult StronyStatyczne(string nazwa)
        {
            return View(nazwa);
        }
    }
}