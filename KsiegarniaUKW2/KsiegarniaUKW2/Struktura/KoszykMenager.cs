using KsiegarniaUKW2.DAL;
using KsiegarniaUKW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KsiegarniaUKW2.Struktura
{
    public class KoszykMenager
    {
        private Ksiazkicontext db;
        private ISessionMenager session;

        public KoszykMenager(ISessionMenager session, Ksiazkicontext db)
        {
            this.session = session;
            this.db = db;
        }

        public List<PozycjaKoszyka> PobierzKoszyk()
        {
            List<PozycjaKoszyka> koszyk;

            if (session.Get<List<PozycjaKoszyka>>(Const.KoszykSessionKlucz) == null)
            {
                koszyk = new List<PozycjaKoszyka>();
            }
            else
            {
                koszyk = session.Get<List<PozycjaKoszyka>>(Const.KoszykSessionKlucz) as List<PozycjaKoszyka>;
            }

            return koszyk;
        }

        public void DodajDoKoszyka(int KsiazkaId)
        {
            var koszyk = PobierzKoszyk();
            var pozycjaKoszyka = koszyk.Find(k => k.Ksiazka.KsiazkaId == KsiazkaId);

            if (pozycjaKoszyka != null)
                pozycjaKoszyka.Ilosc++;
            else
            {
                var ksiazkaDoDodania = db.Ksiazki.Where(k => k.KsiazkaId == KsiazkaId).SingleOrDefault();

                if (ksiazkaDoDodania != null)
                {
                    var nowaPozycjaKoszyka = new PozycjaKoszyka()
                    {
                        Ksiazka = ksiazkaDoDodania,
                        Ilosc = 1,
                        Wartosc = ksiazkaDoDodania.CenaKsiazki
                    };
                    koszyk.Add(nowaPozycjaKoszyka);
                }
            }

            session.Set(Const.KoszykSessionKlucz, koszyk);
        }

        public int UsunZKoszyka(int KsiazkaId)
        {
            var koszyk = PobierzKoszyk();
            var pozycjaKoszyka = koszyk.Find(k => k.Ksiazka.KsiazkaId == KsiazkaId);

            if (pozycjaKoszyka != null)
            {
                if (pozycjaKoszyka.Ilosc > 1)
                {
                    pozycjaKoszyka.Ilosc--;
                    return pozycjaKoszyka.Ilosc;
                }
                else
                {
                    koszyk.Remove(pozycjaKoszyka);
                }
            }

            return 0;
        }

        public decimal PobierzWartoscKoszyka()
        {
            var koszyk = PobierzKoszyk();
            return koszyk.Sum(k => (k.Ilosc * k.Ksiazka.CenaKsiazki));
        }
        public int PobierzIloscPozycjiKoszyka()
        {
            var koszyk = PobierzKoszyk();
            int ilosc = koszyk.Sum(k => k.Ilosc);
            return ilosc;
        }

        public Zamowienie UtworzZamowienie(Zamowienie noweZamowienie, string userId)
        {
            var koszyk = PobierzKoszyk();
            noweZamowienie.DataDodania = DateTime.Now;
            noweZamowienie.UserId = userId;

            db.Zamowienia.Add(noweZamowienie);

            if (noweZamowienie.PozycjeZamowienia == null)
                noweZamowienie.PozycjeZamowienia = new List<PozycjaZamowienia>();

            decimal koszykWartosc = 0;

            foreach (var koszykElement in koszyk)
            {
                var nowaPozycjaZamowienia = new PozycjaZamowienia()
                {
                    ksiazkaId = koszykElement.Ksiazka.KsiazkaId,
                    Ilosc = koszykElement.Ilosc,
                    CenaZakupu = koszykElement.Ksiazka.CenaKsiazki
                };

                koszykWartosc += (koszykElement.Ilosc * koszykElement.Ksiazka.CenaKsiazki);
                noweZamowienie.PozycjeZamowienia.Add(nowaPozycjaZamowienia);
            }

            noweZamowienie.WartoscZamowienia = koszykWartosc;
            db.SaveChanges();

            return noweZamowienie;
        }
        public void PustyKoszyk()
        {
            session.Set<List<PozycjaKoszyka>>(Const.KoszykSessionKlucz, null);
        }
    }
}