using Evidencija.online.Data;
using Evidencija.online.Interfaces;
using Evidencija.online.Models;
using Microsoft.EntityFrameworkCore;

namespace Evidencija.online.Services
{
    public class TrosakService : ITrosakService
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidationService _validationService;
        private readonly Interfaces.ILogger _logger;
        private const int FREE_PLAN_EXPENSE_LIMIT = 5;

        public TrosakService(
            ApplicationDbContext context,
            IValidationService validationService,
            Interfaces.ILogger logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Trosak>> GetAllByUserAsync(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
                throw new ArgumentException("Email korisnika je obavezan", nameof(userEmail));

            try
            {
                _logger.LogInformation($"Dohvaćanje troškova za korisnika: {userEmail}");
                return await _context.Trosak
                    .Include(t => t.Kategorija)
                    .Include(t => t.Korisnik)
                    .Where(x => x.Korisnik.Email == userEmail)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri dohvaćanju troškova za korisnika: {userEmail}", ex);
                throw;
            }
        }

        public async Task<Trosak> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Dohvaćanje troška s ID: {id}");
                return await _context.Trosak
                    .Include(t => t.Kategorija)
                    .Include(t => t.Korisnik)
                    .FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri dohvaćanju troška s ID: {id}", ex);
                throw;
            }
        }

        public async Task<Trosak> CreateAsync(Trosak trosak, string userEmail)
        {
            if (trosak == null)
                throw new ArgumentNullException(nameof(trosak));

            if (string.IsNullOrEmpty(userEmail))
                throw new ArgumentException("Email korisnika je obavezan", nameof(userEmail));

            // Check expense limit
            if (!await CanUserCreateExpenseAsync(userEmail))
            {
                _logger.LogWarning($"Korisnik {userEmail} je dosegao limit od {FREE_PLAN_EXPENSE_LIMIT} troškova");
                throw new InvalidOperationException($"Dosegnuli ste limit od {FREE_PLAN_EXPENSE_LIMIT} troškova za besplatni plan");
            }

            // Get user
            var korisnik = await _context.Korisnik
                .FirstOrDefaultAsync(x => x.Email == userEmail);

            if (korisnik == null)
            {
                _logger.LogError($"Korisnik s emailom {userEmail} nije pronađen");
                throw new InvalidOperationException("Korisnik nije pronađen");
            }

            trosak.KorisnikId = korisnik.Id;

            var validationResult = _validationService.ValidateTrosak(trosak);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"Validacija troška neuspješna: {string.Join(", ", validationResult.Errors)}");
                throw new ArgumentException($"Validacija neuspješna: {string.Join(", ", validationResult.Errors)}");
            }

            try
            {
                _logger.LogInformation($"Kreiranje troška: {trosak.Opis} za korisnika: {userEmail}");
                _context.Add(trosak);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Trošak uspješno kreiran s ID: {trosak.Id}");
                return trosak;
            }
            catch (Exception ex)
            {
                _logger.LogError("Greška pri kreiranju troška", ex);
                throw;
            }
        }

        public async Task<Trosak> UpdateAsync(Trosak trosak)
        {
            if (trosak == null)
                throw new ArgumentNullException(nameof(trosak));

            var validationResult = _validationService.ValidateTrosak(trosak);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"Validacija troška neuspješna: {string.Join(", ", validationResult.Errors)}");
                throw new ArgumentException($"Validacija neuspješna: {string.Join(", ", validationResult.Errors)}");
            }

            try
            {
                _logger.LogInformation($"Ažuriranje troška s ID: {trosak.Id}");
                _context.Update(trosak);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Trošak uspješno ažuriran s ID: {trosak.Id}");
                return trosak;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await ExistsAsync(trosak.Id))
                {
                    _logger.LogError($"Trošak s ID: {trosak.Id} nije pronađen");
                    throw new InvalidOperationException("Trošak nije pronađen");
                }

                _logger.LogError($"Greška pri ažuriranju troška s ID: {trosak.Id}", ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri ažuriranju troška s ID: {trosak.Id}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Brisanje troška s ID: {id}");
                var trosak = await _context.Trosak.FindAsync(id);

                if (trosak == null)
                {
                    _logger.LogWarning($"Trošak s ID: {id} nije pronađen za brisanje");
                    return false;
                }

                _context.Trosak.Remove(trosak);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Trošak s ID: {id} uspješno obrisan");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri brisanju troška s ID: {id}", ex);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.Trosak.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri provjeri postojanja troška s ID: {id}", ex);
                throw;
            }
        }

        public async Task<bool> CanUserCreateExpenseAsync(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
                throw new ArgumentException("Email korisnika je obavezan", nameof(userEmail));

            try
            {
                var count = await GetUserExpenseCountAsync(userEmail);
                return count < FREE_PLAN_EXPENSE_LIMIT;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri provjeri limita troškova za korisnika: {userEmail}", ex);
                throw;
            }
        }

        public async Task<int> GetUserExpenseCountAsync(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
                throw new ArgumentException("Email korisnika je obavezan", nameof(userEmail));

            try
            {
                return await _context.Trosak
                    .Include(t => t.Korisnik)
                    .CountAsync(x => x.Korisnik.Email == userEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška pri dohvaćanju broja troškova za korisnika: {userEmail}", ex);
                throw;
            }
        }
    }
}
