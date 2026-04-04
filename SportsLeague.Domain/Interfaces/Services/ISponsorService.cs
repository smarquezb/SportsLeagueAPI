using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface ISponsorService
    {

        // Sponsor Management Operations
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task<Sponsor?> GetByIdAsync(int sponsorId);
        Task<Sponsor> CreateAsync(Sponsor entity);
        Task UpdateAsync(int sponsorId, Sponsor entity);
        Task DeleteAsync(int sponsorId);

        // Tournament Linkage Operations
        Task<TournamentSponsor> RegisterSponsorAsync(int tId, int sId, decimal amount);
        Task<IEnumerable<TournamentSponsor>> GetSponsorsByTournamentAsync(int tId);
        Task RemoveSponsorAsync(int tId, int sId);
    }
}


