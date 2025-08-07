using Evidencija.online.Models;

namespace Evidencija.online.Services
{
    public interface IKategorijaService
    {
        Task<IEnumerable<Kategorija>> GetAllAsync();
        Task<Kategorija> GetByIdAsync(int id);
        Task<Kategorija> CreateAsync(Kategorija kategorija);
        Task<Kategorija> UpdateAsync(Kategorija kategorija);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
