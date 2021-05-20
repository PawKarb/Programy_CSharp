using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace KsiegarniaUKW2.Models
{
    public class Ksiazka
    {
        public int KsiazkaId { get; set; }
        public int KategoriaId { get; set; }

        [Required(ErrorMessage = "Wprowadz tutuł ksiazki")]
        [StringLength(50)]
        public string Tytulksiazki { get; set; }
       [Required(ErrorMessage = "Wprowadz Autora ksiazki")]
        [StringLength(50)]
        public string AutorKsiazki { get; set; }
        public DateTime DataDodania { get; set; }
       [StringLength(50)]
        public string NazwaPlikuOkladki { get; set; }
        public string OpisKsiazki { get; set; }
        public decimal CenaKsiazki { get; set; }
        public bool Hit { get; set; }
        public bool Ukryty { get; set; }
        public string OpisS { get; set; }

        public virtual Kategoria Kategoria { get; set; }
    }
}