using Evidencija.online.Models;

namespace Evidencija.online.Services
{
    public interface ITrosakService
    {
        Task<IEnumerable<Trosak>> GetAllByUserAsync(string userEmail);
        Task<Trosak> GetByIdAsync(int id);
        Task<Trosak> CreateAsync(Trosak trosak, string userEmail);
        Task<Trosak> UpdateAsync(Trosak trosak);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CanUserCreateExpenseAsync(string userEmail);
        Task<int> GetUserExpenseCountAsync(string userEmail);
    }

}
