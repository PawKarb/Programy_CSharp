using KsiegarniaUKW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KsiegarniaUKW2.ViewModels
{
    public class KoszykViewModel
    {
        public List<PozycjaKoszyka> PozycjeKoszyka { get; set; }
        public decimal CenaCalkowita { get; set; }
    }
}