using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using KsiegarniaUKW2.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KsiegarniaUKW2.DAL
{
    public class Ksiazkicontext : IdentityDbContext<ApplicationUser>
    {
        public Ksiazkicontext() : base("KsiazkiContext1")
        {

        }
        public static Ksiazkicontext Create()
        {
            return new Ksiazkicontext();
        }

        /*static Ksiazkicontext()
        {
            Database.SetInitializer<Ksiazkicontext>(new ksiazkiInitiaizer());
        }*/

        public DbSet<Ksiazka> Ksiazki { get; set; }
        public DbSet<Kategoria> Kategorie { get; set; }
        public DbSet<Zamowienie> Zamowienia { get; set; }
        public DbSet<PozycjaZamowienia> PozycjeZamowienia { get; set; }

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        // using System.Data.Entity.ModelConfiguration.Conventions;
        // Wyłącza konwencję, która automatycznie tworzy liczbę mnogą dla nazw tabel w bazie danych
        // Zamiast Kategorie zostałaby stworzona tabela o nazwie Kategories
        modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }*/

}
}