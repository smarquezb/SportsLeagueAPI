using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
    {
        // Inyección del contexto para la persistencia
        public TournamentSponsorRepository(LeagueDbContext db) : base(db) { }

        // Recupera la relación específica cargando las entidades relacionadas
        public async Task<TournamentSponsor?> GetByTournamentAndSponsorAsync(int tId, int sId)
        {
            return await _dbSet.AsNoTracking()
                .Include(x => x.Tournament)
                .Include(x => x.Sponsor)
                .FirstOrDefaultAsync(x => x.TournamentId == tId && x.SponsorId == sId);
        }

        // Obtiene el listado de patrocinadores vinculados a un torneo
        public async Task<IEnumerable<TournamentSponsor>> GetSponsorsByTournamentAsync(int tournamentId)
        {
            return await _dbSet.AsNoTracking()
                .Where(link => link.TournamentId == tournamentId)
                .Include(link => link.Sponsor)
                .Include(link => link.Tournament)
                .ToListAsync();
        }

        // Registra una nueva vinculación y retorna el objeto con sus relaciones cargadas
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




