using Evidencija.online.Models;

namespace Evidencija.online.Services
{
    public interface IKorisnikService
    {
        Task<IEnumerable<Korisnik>> GetAllAsync();
        Task<Korisnik> GetByIdAsync(string ime);
        Task<Korisnik> CreateAsync(Korisnik korisnik, string userEmail);
        Task<Korisnik> UpdateAsync(Korisnik korisnik);
        Task<bool> DeleteAsync(string ime);
        Task<bool> ExistsAsync(string ime);
    }
}
