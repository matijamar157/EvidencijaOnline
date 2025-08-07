using Evidencija.online.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Evidencija.online.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Kategorija> Kategorija { get; set; }
        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<Trosak> Trosak { get; set; }
    }
}
