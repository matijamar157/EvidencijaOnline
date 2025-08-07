using Evidencija.online.Interfaces;
using Evidencija.online.Models;

namespace Evidencija.online.Services
{
    public class ValidationService : IValidationService
    {
        public ValidationResult ValidateKategorija(Kategorija kategorija)
        {
            var result = new ValidationResult { IsValid = true };

            if (kategorija == null)
            {
                result.IsValid = false;
                result.Errors.Add("Kategorija ne može biti null");
                return result;
            }

            if (string.IsNullOrWhiteSpace(kategorija.Naziv))
            {
                result.IsValid = false;
                result.Errors.Add("Naziv kategorije je obavezan");
            }
            else if (kategorija.Naziv.Length > 100)
            {
                result.IsValid = false;
                result.Errors.Add("Naziv kategorije ne može biti duži od 100 znakova");
            }

            if (!string.IsNullOrEmpty(kategorija.Opis) && kategorija.Opis.Length > 500)
            {
                result.IsValid = false;
                result.Errors.Add("Opis kategorije ne može biti duži od 500 znakova");
            }

            return result;
        }

        public ValidationResult ValidateKorisnik(Korisnik korisnik)
        {
            var result = new ValidationResult { IsValid = true };

            if (korisnik == null)
            {
                result.IsValid = false;
                result.Errors.Add("Korisnik ne može biti null");
                return result;
            }

            if (string.IsNullOrWhiteSpace(korisnik.Ime))
            {
                result.IsValid = false;
                result.Errors.Add("Ime je obavezno");
            }
            else if (korisnik.Ime.Length > 50)
            {
                result.IsValid = false;
                result.Errors.Add("Ime ne može biti duže od 50 znakova");
            }

            if (string.IsNullOrWhiteSpace(korisnik.Prezime))
            {
                result.IsValid = false;
                result.Errors.Add("Prezime je obavezno");
            }
            else if (korisnik.Prezime.Length > 50)
            {
                result.IsValid = false;
                result.Errors.Add("Prezime ne može biti duže od 50 znakova");
            }

            if (string.IsNullOrWhiteSpace(korisnik.Email))
            {
                result.IsValid = false;
                result.Errors.Add("Email je obavezan");
            }
            else if (!IsValidEmail(korisnik.Email))
            {
                result.IsValid = false;
                result.Errors.Add("Email format nije valjan");
            }

            return result;
        }

        public ValidationResult ValidateTrosak(Trosak trosak)
        {
            var result = new ValidationResult { IsValid = true };

            if (trosak == null)
            {
                result.IsValid = false;
                result.Errors.Add("Trošak ne može biti null");
                return result;
            }

            if (string.IsNullOrWhiteSpace(trosak.Opis))
            {
                result.IsValid = false;
                result.Errors.Add("Opis troška je obavezan");
            }
            else if (trosak.Opis.Length > 200)
            {
                result.IsValid = false;
                result.Errors.Add("Opis troška ne može biti duži od 200 znakova");
            }

            if (trosak.Iznos <= 0)
            {
                result.IsValid = false;
                result.Errors.Add("Iznos mora biti veći od 0");
            }

            if (trosak.KategorijaId <= 0)
            {
                result.IsValid = false;
                result.Errors.Add("Kategorija je obavezna");
            }

            if (trosak.Datum == default(DateTime))
            {
                result.IsValid = false;
                result.Errors.Add("Datum je obavezan");
            }
            else if (trosak.Datum > DateTime.Now)
            {
                result.IsValid = false;
                result.Errors.Add("Datum ne može biti u budućnosti");
            }

            if (string.IsNullOrWhiteSpace(trosak.Valuta))
            {
                result.IsValid = false;
                result.Errors.Add("Valuta je obavezna");
            }

            return result;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
