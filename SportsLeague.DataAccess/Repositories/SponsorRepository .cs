using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
    {
        // Constructor that injects the database context into the base class
        public SponsorRepository(LeagueDbContext dbContext) : base(dbContext) { }

        // Locate a sponsor in the system by filtering by its trade name.
        public async Task<Sponsor?> GetByNameAsync(string sponsorName)
        {
            
            return await _dbSet.FirstOrDefaultAsync(s => s.Name.ToUpper() == sponsorName.ToUpper());
        }
    }
}


