using System.ComponentModel.DataAnnotations;

namespace Evidencija.online.Models
{
    public class Trosak
    {
        [Key]
        public int Id { get; set; }
        public string Opis { get; set; }
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        public int KategorijaId { get; set; }
        public Kategorija Kategorija { get; set; }
        public DateTime Datum { get; set; }
        public int Iznos { get; set; }
        public string Valuta { get; set; }
    }
}
