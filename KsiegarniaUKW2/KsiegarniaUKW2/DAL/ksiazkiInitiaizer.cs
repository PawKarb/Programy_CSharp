using KsiegarniaUKW2.Migrations;
using KsiegarniaUKW2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;

namespace KsiegarniaUKW2.DAL
{
    public class ksiazkiInitiaizer : MigrateDatabaseToLatestVersion<Ksiazkicontext, Configuration>
    {
        

        public static void SeedksiazkiData(Ksiazkicontext context)
        {

            var kategorie = new List<Kategoria>
        {
        new Kategoria(){ KategoriaId = 1, NazwaKategorii="Naukwe", NazwaPlikuIkony="naukowe.jpg", OpisKategorii = "Ksiazki naukowe"},
        new Kategoria(){KategoriaId = 2, NazwaKategorii="Historyczne", NazwaPlikuIkony="Historyczne.jpg", OpisKategorii = "Ksiazki Historyczne" },
        new Kategoria(){KategoriaId = 3, NazwaKategorii="Przygodowe", NazwaPlikuIkony="Przygodowe.jpg", OpisKategorii = "Ksiazki Przygodowe" },
        new Kategoria(){KategoriaId = 4, NazwaKategorii="Podreczniki", NazwaPlikuIkony="Podreczniki.jpg", OpisKategorii = "Podreczniki" },
        };
            kategorie.ForEach(k => context.Kategorie.AddOrUpdate(k));
            context.SaveChanges();

            var ksiazkii = new List<Ksiazka>
        {
            new Ksiazka(){KsiazkaId= 1, AutorKsiazki = "Jan Kowalski", Tytulksiazki = "Wprowadzenie do C++", KategoriaId =1,
            CenaKsiazki = 99, Hit = true, NazwaPlikuOkladki="wprowadzenie.jpg", DataDodania=DateTime.Now, OpisKsiazki = "Wprowadzenie do C++"},
            new Ksiazka(){KsiazkaId= 2, AutorKsiazki = "Kmil Kowalski", Tytulksiazki = "Historia swiata", KategoriaId =1,
            CenaKsiazki = 49, Hit = true, NazwaPlikuOkladki="historiaSe.jpg", DataDodania=DateTime.Now, OpisKsiazki = "Historia swiata"},
            new Ksiazka(){KsiazkaId= 3, AutorKsiazki = "Jan Tetarnik", Tytulksiazki = "W gorach", KategoriaId =3,
            CenaKsiazki = 9, Hit = false, NazwaPlikuOkladki="wgorach.jpg", DataDodania=DateTime.Now, OpisKsiazki = "Przygodowa"},
            new Ksiazka(){KsiazkaId= 4, AutorKsiazki = "Marian Kawka", Tytulksiazki = "Jezyk Polski", KategoriaId =4,
            CenaKsiazki = 12, Hit = true, NazwaPlikuOkladki="jpolski.jpg", DataDodania=DateTime.Now, OpisKsiazki = "Podrecznik do jezyka polskiego"}
        };

            ksiazkii.ForEach(k => context.Ksiazki.AddOrUpdate(k));
            context.SaveChanges();
        }

        
    }

    
}