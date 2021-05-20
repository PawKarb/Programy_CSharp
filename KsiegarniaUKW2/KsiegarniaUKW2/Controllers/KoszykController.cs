using KsiegarniaUKW2.DAL;
using KsiegarniaUKW2.Struktura;
using KsiegarniaUKW2.ViewModels;
using KsiegarniaUKW2.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using KsiegarniaUKW2.App_Start;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace KsiegarniaUKW2.Controllers
{
    public class KoszykController : Controller
    {
        private KoszykMenager koszykMenager;
        private Ksiazkicontext db;

        private ISessionMenager SessionMenager { get; set; }
        // GET: Koszyk

        public KoszykController()
        {
            db = new Ksiazkicontext();

            SessionMenager = new SessionMenager();
            koszykMenager = new KoszykMenager(SessionMenager, db);
        }
        public ActionResult Index()
        {

            var pozycjeKoszyka = koszykMenager.PobierzKoszyk();
            var cenaCalkowita = koszykMenager.PobierzWartoscKoszyka();
            KoszykViewModel koszykVM = new KoszykViewModel()
            {
                PozycjeKoszyka = pozycjeKoszyka,
                CenaCalkowita = cenaCalkowita
            };

            return View(koszykVM);
        }

        public ActionResult DodajDoKoszyka(int id)
        {
            koszykMenager.DodajDoKoszyka(id);


            return RedirectToAction("Index");
        }

        public int PobierzIloscElementowKoszyka()
        {
            return koszykMenager.PobierzIloscPozycjiKoszyka();
        }

        public ActionResult UsunZKoszyka(int KsiazkaId2)
        {
            int iloscPozycji = koszykMenager.UsunZKoszyka(KsiazkaId2);
            int iloscPozycjiKoszyka = koszykMenager.PobierzIloscPozycjiKoszyka();
            decimal wartoscKoszyka = koszykMenager.PobierzWartoscKoszyka();

            var wynik = new KoszykUsuwanieViewModel
            {
                IdPozycjiUsuwanej = KsiazkaId2,
                IloscPozycjiUsuwanej = iloscPozycji,
                KoszykCenaCalkowita = wartoscKoszyka,
                KoszykIloscPozycji = iloscPozycjiKoszyka,
            };
            return Json(wynik);
        }

        public async Task<ActionResult> Zaplac()
        {
            //var name = User.Identity.Name;
            // logger.Info("Strona koszyk | zaplac | " + name);

            if (Request.IsAuthenticated)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                var zamowienie = new Zamowienie
                {
                    Imie = user.DaneUzytkownika.Imie,
                    Nazwisko = user.DaneUzytkownika.Nazwisko,
                    Ulica = user.DaneUzytkownika.Adres,
                    Miasto = user.DaneUzytkownika.Miasto,
                    kodPocztowy = user.DaneUzytkownika.KodPocztowy,
                    Email = user.DaneUzytkownika.Email,
                    Telefon = user.DaneUzytkownika.Telefon
                };
                return View(zamowienie);
            }
            else
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Zaplac", "Koszyk") });
        }
        [HttpPost]
        public async Task<ActionResult> Zaplac(Zamowienie zamowienieSzczegoly)
        {
            if (ModelState.IsValid)
            {
                // pobieramy id uzytkownika aktualnie zalogowanego
                var userId = User.Identity.GetUserId();

                // utworzenie obiektu zamowienia na podstawie tego co mamy w koszyku
                var newOrder = koszykMenager.UtworzZamowienie(zamowienieSzczegoly, userId);

                // szczegóły użytkownika - aktualizacja danych 
                var user = await UserManager.FindByIdAsync(userId);
                TryUpdateModel(user.DaneUzytkownika);
                await UserManager.UpdateAsync(user);

                // opróżnimy nasz koszyk zakupów
                koszykMenager.PustyKoszyk();



                return RedirectToAction("PotwierdzenieZamowienia");
            }
            else
                return View(zamowienieSzczegoly);
        }

        public ActionResult PotwierdzenieZamowienia()
        {
            var name = User.Identity.Name;
            //logger.Info("Strona koszyk | potwierdzenie | " + name);
            return View();
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}