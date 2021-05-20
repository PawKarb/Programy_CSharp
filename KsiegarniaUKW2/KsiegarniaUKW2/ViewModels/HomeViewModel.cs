using KsiegarniaUKW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KsiegarniaUKW2.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Kategoria> Kategorie { get; set; }
        public IEnumerable<Ksiazka> Nowosci { get; set; }
        public IEnumerable<Ksiazka> Bestsellery { get; set; }

    }
}