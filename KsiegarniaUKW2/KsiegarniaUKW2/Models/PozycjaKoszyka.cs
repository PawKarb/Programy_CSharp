using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KsiegarniaUKW2.Models
{
    public class PozycjaKoszyka
    {
        public Ksiazka Ksiazka { get; set; }
        public int Ilosc { get; set; }
        public decimal Wartosc { get; set; }
    }
}