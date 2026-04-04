using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITournamentSponsorRepository : IGenericRepository<TournamentSponsor>
    {
        // Obtiene la lista completa de sponsors vinculados a un torneo
        Task<IEnumerable<TournamentSponsor>> GetSponsorsByTournamentAsync(int tourneyId);


        Task<TournamentSponsor?> GetByTournamentAndSponsorAsync(int tId, int sId);

        
        Task<TournamentSponsor> CreateWithIncludesAsync(TournamentSponsor model);
    }
}
