using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KsiegarniaUKW2.Models
{
    public class Zamowienie
    {
        public int ZamowienieId { get; set; }
        //[Required(ErrorMessage = "Wprowadz imie")]
        //[StringLength(50)]
        public string Imie { get; set; }
        //[Required(ErrorMessage = "Wprowadz nazwisko")]
        // [StringLength(50)]
        public string Nazwisko { get; set; }
        // [Required(ErrorMessage = "Wprowadz Ulice")]
        // [StringLength(50)]
        public string Ulica { get; set; }
        //[Required(ErrorMessage = "Wprowadz Miasto")]
        // [StringLength(50)]
        public string Miasto { get; set; }
        // [Required(ErrorMessage = "Wprowadz kod pocztwy")]
        // [StringLength(50)]
        public string kodPocztowy { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Komentarz { get; set; }
        public DateTime DataDodania { get; set; }
        public StanZamowienia StanZamowienia { get; set; }
        public decimal WartoscZamowienia { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }


        public List<PozycjaZamowienia> PozycjeZamowienia { get; set; }
        
    }
    public enum StanZamowienia
    {
        nowe,
        zrealizowane
    }

}