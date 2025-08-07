using Evidencija.online.Models;
using System.ComponentModel.DataAnnotations;

namespace Evidencija.online.Interfaces
{
    public interface IValidationService
    {
        ValidationResult ValidateKategorija(Kategorija kategorija);
        ValidationResult ValidateKorisnik(Korisnik korisnik);
        ValidationResult ValidateTrosak(Trosak trosak);
    }
}
