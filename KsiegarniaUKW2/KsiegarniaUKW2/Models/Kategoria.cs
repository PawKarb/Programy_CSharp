using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KsiegarniaUKW2.Models
{
    public class Kategoria
    {
        public int KategoriaId { get; set; }
        [Required(ErrorMessage = "Wprowadz nazwe Kategori")]
        [StringLength(50)]
        public string NazwaKategorii { get; set; }
       [Required(ErrorMessage = "Wprowadz opis kategori")]
       [StringLength(50)]
        public string OpisKategorii { get; set; }
        public string NazwaPlikuIkony { get; set; }

        public virtual ICollection<Ksiazka> Ksiazki { get; set; }

    }
}