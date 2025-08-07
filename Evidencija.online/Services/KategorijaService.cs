using Evidencija.online.Interfaces;
using Evidencija.online.Models;

namespace Evidencija.online.Services
{
    public class KategorijaService : IKategorijaService
    {
        private readonly IRepository<Kategorija> _repository;
        private readonly IValidationService _validationService;
        private readonly Interfaces.ILogger _logger;

        public KategorijaService(
            IRepository<Kategorija> repository,
            IValidationService validationService,
            Interfaces.ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Kategorija>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Dohvaćanje svih kategorija");
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Greška pri dohvaćanju kategorija", ex);
                throw;
            }
        }

        public async Task<Kategorija> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Dohvaćanje kategorije s ID: {id}");
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri dohvaćanju kategorije s ID: {id}", ex);
                throw;
            }
        }

        public async Task<Kategorija> CreateAsync(Kategorija kategorija)
        {
            if (kategorija == null)
                throw new ArgumentNullException(nameof(kategorija));

            var validationResult = _validationService.ValidateKategorija(kategorija);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"Validacija kategorije neuspješna: {string.Join(", ", validationResult.Errors)}");
                throw new ArgumentException($"Validacija neuspješna: {string.Join(", ", validationResult.Errors)}");
            }

            try
            {
                _logger.LogInformation($"Kreiranje kategorije: {kategorija.Naziv}");
                var created = await _repository.AddAsync(kategorija);
                _logger.LogInformation($"Kategorija uspješno kreirana s ID: {created.Id}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError("Greška pri kreiranju kategorije", ex);
                throw;
            }
        }

        public async Task<Kategorija> UpdateAsync(Kategorija kategorija)
        {
            if (kategorija == null)
                throw new ArgumentNullException(nameof(kategorija));

            var validationResult = _validationService.ValidateKategorija(kategorija);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"Validacija kategorije neuspješna: {string.Join(", ", validationResult.Errors)}");
                throw new ArgumentException($"Validacija neuspješna: {string.Join(", ", validationResult.Errors)}");
            }

            try
            {
                _logger.LogInformation($"Ažuriranje kategorije s ID: {kategorija.Id}");
                var updated = await _repository.UpdateAsync(kategorija);
                _logger.LogInformation($"Kategorija uspješno ažurirana s ID: {updated.Id}");
                return updated;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri ažuriranju kategorije s ID: {kategorija.Id}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Brisanje kategorije s ID: {id}");
                var result = await _repository.DeleteAsync(id);

                if (result)
                    _logger.LogInformation($"Kategorija s ID: {id} uspješno obrisana");
                else
                    _logger.LogWarning($"Kategorija s ID: {id} nije pronađena za brisanje");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri brisanju kategorije s ID: {id}", ex);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _repository.ExistsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri provjeri postojanja kategorije s ID: {id}", ex);
                throw;
            }
        }
    }
}
