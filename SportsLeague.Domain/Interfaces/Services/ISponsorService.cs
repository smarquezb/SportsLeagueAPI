using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface ISponsorService
    {
        // Operaciones de gestión de Patrocinadores
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task<Sponsor?> GetByIdAsync(int sponsorId);
        Task<Sponsor> CreateAsync(Sponsor entity);
        Task UpdateAsync(int sponsorId, Sponsor entity);
        Task DeleteAsync(int sponsorId);

        // Operaciones de vinculación con Torneos
        Task<TournamentSponsor> RegisterSponsorAsync(int tId, int sId, decimal amount);
        Task<IEnumerable<TournamentSponsor>> GetSponsorsByTournamentAsync(int tId);
        Task RemoveSponsorAsync(int tId, int sId);
    }
}


