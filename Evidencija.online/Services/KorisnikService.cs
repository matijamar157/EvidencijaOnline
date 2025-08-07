using Evidencija.online.Interfaces;
using Evidencija.online.Models;

namespace Evidencija.online.Services
{
    public class KorisnikService : IKorisnikService
    {
        private readonly IRepository<Korisnik> _repository;
        private readonly IValidationService _validationService;
        private readonly Interfaces.ILogger _logger;

        public KorisnikService(
            IRepository<Korisnik> repository,
            IValidationService validationService,
            Interfaces.ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Korisnik>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Dohvaćanje svih korisnika");
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Greška pri dohvaćanju korisnika", ex);
                throw;
            }
        }

        public async Task<Korisnik> GetByIdAsync(string ime)
        {
            try
            {
                _logger.LogInformation($"Dohvaćanje korisnika s imenom: {ime}");
                return await _repository.GetByIdAsync(ime);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri dohvaćanju korisnika s imenom: {ime}", ex);
                throw;
            }
        }

        public async Task<Korisnik> CreateAsync(Korisnik korisnik, string userEmail)
        {
            if (korisnik == null)
                throw new ArgumentNullException(nameof(korisnik));

            if (string.IsNullOrEmpty(userEmail))
                throw new ArgumentException("Email korisnika je obavezan", nameof(userEmail));

            korisnik.Email = userEmail;

            var validationResult = _validationService.ValidateKorisnik(korisnik);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"Validacija korisnika neuspješna: {string.Join(", ", validationResult.Errors)}");
                throw new ArgumentException($"Validacija neuspješna: {string.Join(", ", validationResult.Errors)}");
            }

            try
            {
                _logger.LogInformation($"Kreiranje korisnika: {korisnik.Ime} {korisnik.Prezime}");
                var created = await _repository.AddAsync(korisnik);
                _logger.LogInformation($"Korisnik uspješno kreiran s ID: {created.Id}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError("Greška pri kreiranju korisnika", ex);
                throw;
            }
        }

        public async Task<Korisnik> UpdateAsync(Korisnik korisnik)
        {
            if (korisnik == null)
                throw new ArgumentNullException(nameof(korisnik));

            var validationResult = _validationService.ValidateKorisnik(korisnik);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"Validacija korisnika neuspješna: {string.Join(", ", validationResult.Errors)}");
                throw new ArgumentException($"Validacija neuspješna: {string.Join(", ", validationResult.Errors)}");
            }

            try
            {
                _logger.LogInformation($"Ažuriranje korisnika s ID: {korisnik.Id}");
                var updated = await _repository.UpdateAsync(korisnik);
                _logger.LogInformation($"Korisnik uspješno ažuriran s ID: {updated.Id}");
                return updated;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri ažuriranju korisnika s ID: {korisnik.Id}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string ime)
        {
            try
            {
                _logger.LogInformation($"Brisanje korisnika s imenom: {ime}");
                var result = await _repository.DeleteAsync(ime);

                if (result)
                    _logger.LogInformation($"Korisnik s imenom: {ime} uspješno obrisan");
                else
                    _logger.LogWarning($"Korisnik s imenom: {ime} nije pronađen za brisanje");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri brisanju korisnika s imenom: {ime}", ex);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(string ime)
        {
            try
            {
                return await _repository.ExistsAsync(ime);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri provjeri postojanja korisnika s imenom: {ime}", ex);
                throw;
            }
        }
    }
}
