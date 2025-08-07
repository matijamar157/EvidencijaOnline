using System.ComponentModel.DataAnnotations;

namespace Evidencija.online.Models
{
    public class Korisnik
    {
        [Key]
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public bool Free { get; set; } = true;
        public bool Premium { get; set; } = true;
        public DateTime PremiumEnds { get; set; }
    }
}
