using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System.Text.RegularExpressions;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsors;
        private readonly ITournamentRepository _tournaments;
        private readonly ITournamentSponsorRepository _relations;

        public SponsorService(
            ISponsorRepository sponsors,
            ITournamentRepository tournaments,
            ITournamentSponsorRepository relations)
        {
            _sponsors = sponsors;
            _tournaments = tournaments;
            _relations = relations;
        }

        // Email format validation using regular expression
        private static bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync() => await _sponsors.GetAllAsync();

        public async Task<Sponsor?> GetByIdAsync(int sponsorId) => await _sponsors.GetByIdAsync(sponsorId);

        public async Task<Sponsor> CreateAsync(Sponsor entity)
        {
            var duplicate = await _sponsors.GetByNameAsync(entity.Name);
            if (duplicate != null)
                throw new InvalidOperationException($"El nombre '{entity.Name}' ya se encuentra registrado.");

            if (!IsEmailValid(entity.ContactEmail))
                throw new InvalidOperationException("La dirección de correo electrónico es inválida.");

            return await _sponsors.CreateAsync(entity);
        }

        public async Task UpdateAsync(int sponsorId, Sponsor entity)
        {
            var currentSponsor = await _sponsors.GetByIdAsync(sponsorId)
                ?? throw new KeyNotFoundException($"ID {sponsorId} no existe en la base de datos.");

            if (!IsEmailValid(entity.ContactEmail))
                throw new InvalidOperationException("El nuevo correo no tiene un formato aceptable.");

            currentSponsor.Name = entity.Name;
            currentSponsor.ContactEmail = entity.ContactEmail;
            currentSponsor.Phone = entity.Phone;
            currentSponsor.WebsiteUrl = entity.WebsiteUrl;

            await _sponsors.UpdateAsync(currentSponsor);
        }

        public async Task DeleteAsync(int sponsorId)
        {
            if (!await _sponsors.ExistsAsync(sponsorId))
                throw new KeyNotFoundException("No se puede eliminar: Patrocinador no encontrado.");

            await _sponsors.DeleteAsync(sponsorId);
        }

        public async Task<TournamentSponsor> RegisterSponsorAsync(int tId, int sId, decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("El valor del contrato debe ser superior a cero.");

            // Using the discard '_' to validate existence without assigning a variable
            _ = await _tournaments.GetByIdAsync(tId) ?? throw new KeyNotFoundException("Torneo inexistente.");
            _ = await _sponsors.GetByIdAsync(sId) ?? throw new KeyNotFoundException("Patrocinador inexistente.");

            var exists = await _relations.GetByTournamentAndSponsorAsync(tId, sId);
            if (exists != null)
                throw new InvalidOperationException("Esta vinculación ya fue creada previamente.");

            var newRelation = new TournamentSponsor
            {
                SponsorId = sId,
                TournamentId = tId,
                ContractAmount = amount
            };

            return await _relations.CreateWithIncludesAsync(newRelation);
        }

        public async Task<IEnumerable<TournamentSponsor>> GetSponsorsByTournamentAsync(int tId)
        {
            return await _relations.GetSponsorsByTournamentAsync(tId);
        }

        public async Task RemoveSponsorAsync(int tId, int sId)
        {
            var match = await _relations.GetByTournamentAndSponsorAsync(tId, sId)
                ?? throw new KeyNotFoundException("No existe relación activa entre este torneo y el sponsor.");

            await _relations.DeleteAsync(match.Id);
        }
    }
}

