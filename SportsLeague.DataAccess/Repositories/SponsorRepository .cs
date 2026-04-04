using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
  
    public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
    {
        // Constructor que inyecta el contexto de base de datos a la clase base
        public SponsorRepository(LeagueDbContext dbContext) : base(dbContext) { }

       
        // Localiza un patrocinador en el sistema filtrando por su nombre comercial.
    
        public async Task<Sponsor?> GetByNameAsync(string sponsorName) =>
            await _dbSet.FirstOrDefaultAsync(s => s.Name.Equals(sponsorName, StringComparison.OrdinalIgnoreCase));
    }
}

