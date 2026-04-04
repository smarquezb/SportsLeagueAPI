using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
    {
        // Context injection for persistence
        public TournamentSponsorRepository(LeagueDbContext db) : base(db) { }


        // Retrieves the specific relationship by loading the related entities
        public async Task<TournamentSponsor?> GetByTournamentAndSponsorAsync(int tId, int sId)
        {
            return await _dbSet.AsNoTracking()
                .Include(x => x.Tournament)
                .Include(x => x.Sponsor)
                .FirstOrDefaultAsync(x => x.TournamentId == tId && x.SponsorId == sId);
        }


        // Gets the list of sponsors linked to a tournament
        public async Task<IEnumerable<TournamentSponsor>> GetSponsorsByTournamentAsync(int tournamentId)
        {
            return await _dbSet.AsNoTracking()
                .Where(link => link.TournamentId == tournamentId)
                .Include(link => link.Sponsor)
                .Include(link => link.Tournament)
                .ToListAsync();
        }

        // Registers a new link and returns the object with its loaded relationships
        public async Task<TournamentSponsor> CreateWithIncludesAsync(TournamentSponsor model)
        {
            model.CreatedAt = DateTime.UtcNow;

            await _dbSet.AddAsync(model);
            await _context.SaveChangesAsync();

            return await _dbSet.AsNoTracking()
                .Include(x => x.Sponsor)
                .Include(x => x.Tournament)
                .FirstAsync(x => x.TournamentId == model.TournamentId && x.SponsorId == model.SponsorId);
        }
    }
}





