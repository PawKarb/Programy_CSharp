namespace KsiegarniaUKW2.Models
{
    public class PozycjaZamowienia
    {
        public int PozycjaZamowieniaId { get; set; }
        public int ZamowienieId{ get; set; }
        public int ksiazkaId { get; set; }
        public int Ilosc { get; set; }
        public decimal CenaZakupu { get; set; }

        public virtual Ksiazka ksiazka { get; set; }
        public virtual Zamowienie zamowienie { get; set; }
}
}