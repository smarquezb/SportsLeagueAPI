using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsors;
        private readonly ITournamentRepository _tournaments;
        private readonly ITournamentSponsorRepository _relations;

        public SponsorService(ISponsorRepository sponsors, ITournamentRepository tournaments, ITournamentSponsorRepository relations)
        {
            _sponsors = sponsors;
            _tournaments = tournaments;
            _relations = relations;
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync() => await _sponsors.GetAllAsync();

        public async Task<Sponsor> GetByIdAsync(int id) => await _sponsors.GetByIdAsync(id);

        public async Task<Sponsor> CreateAsync(Sponsor entity)
        {
            var exists = await _sponsors.GetByNameAsync(entity.Name);
            if (exists != null) throw new InvalidOperationException("Ese nombre ya está registrado.");

            return await _sponsors.CreateAsync(entity);
        }

        public async Task UpdateAsync(int id, Sponsor entity)
        {
            var current = await _sponsors.GetByIdAsync(id);
            if (current == null) throw new KeyNotFoundException("No se encontró el sponsor.");

            // Actualización directa de campos
            current.Name = entity.Name;
            current.ContactEmail = entity.ContactEmail;
            current.Phone = entity.Phone;
            current.WebsiteUrl = entity.WebsiteUrl;

            await _sponsors.UpdateAsync(current);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _sponsors.ExistsAsync(id);
            if (!exists) throw new KeyNotFoundException("Sponsor no encontrado.");

            await _sponsors.DeleteAsync(id);
        }

        public async Task<TournamentSponsor> RegisterSponsorAsync(int tId, int sId, decimal amount)
        {
            var exists = await _relations.GetByTournamentAndSponsorAsync(tId, sId);
            if (exists != null) throw new InvalidOperationException("Ya existe esta relación.");

            var relation = new TournamentSponsor { SponsorId = sId, TournamentId = tId, ContractAmount = amount };
            return await _relations.CreateWithIncludesAsync(relation);
        }

        public async Task<IEnumerable<TournamentSponsor>> GetSponsorsByTournamentAsync(int tId)
            => await _relations.GetSponsorsByTournamentAsync(tId);

        public async Task RemoveSponsorAsync(int tId, int sId)
        {
            var match = await _relations.GetByTournamentAndSponsorAsync(tId, sId);
            if (match == null) throw new KeyNotFoundException("Relación no encontrada.");

            await _relations.DeleteAsync(match.Id);
        }
    }
}