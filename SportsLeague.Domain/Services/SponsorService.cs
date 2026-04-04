using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System.Net.Mail;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;
        private readonly ILogger<SponsorService> _logger;

        public SponsorService(
            ISponsorRepository sponsorRepository,
            ITournamentRepository tournamentRepository,
            ITournamentSponsorRepository tournamentSponsorRepository,
            ILogger<SponsorService> logger)
        {
            _sponsorRepository = sponsorRepository;
            _tournamentRepository = tournamentRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;
            _logger = logger;
        }

        // Sponsor's disengagement
        public async Task RemoveSponsorAsync(int tournamentId, int sponsorId)
        {
            TournamentSponsor link = await _tournamentSponsorRepository
                .GetByTournamentAndSponsorAsync(tournamentId, sponsorId)
                ?? throw new KeyNotFoundException("El sponsor no está registrado en el torneo");

            await _tournamentSponsorRepository.DeleteAsync(link.Id);

            _logger.LogInformation($"Sponsor {sponsorId} eliminado del torneo {tournamentId}");
        }

        // Validate Email
        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            return await _sponsorRepository.GetAllAsync();
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            return await _sponsorRepository.GetByIdAsync(id);
        }

        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            Sponsor? existing = await _sponsorRepository.GetByNameAsync(sponsor.Name);

            if (existing != null)
                throw new InvalidOperationException($"Ya existe un sponsor con el nombre {sponsor.Name}");

            if (!IsValidEmail(sponsor.ContactEmail))
                throw new InvalidOperationException("El correo electrónico no tiene un formato válido.");

            var result = await _sponsorRepository.CreateAsync(sponsor);

            _logger.LogInformation($"Sponsor creado: {sponsor.Name}");

            return result;
        }

        public async Task UpdateAsync(int id, Sponsor sponsor)
        {
            Sponsor? existing = await _sponsorRepository.GetByIdAsync(id);

            if (existing == null)
                throw new KeyNotFoundException($"No se encontró el sponsor con ID {id}");

            if (!IsValidEmail(sponsor.ContactEmail))
                throw new InvalidOperationException("El correo electrónico no tiene un formato válido.");

            existing.Name = sponsor.Name;
            existing.ContactEmail = sponsor.ContactEmail;
            existing.Phone = sponsor.Phone;
            existing.WebsiteUrl = sponsor.WebsiteUrl;

            await _sponsorRepository.UpdateAsync(existing);

            _logger.LogInformation($"Sponsor actualizado: {id}");
        }

        public async Task DeleteAsync(int id)
        {
            bool exists = await _sponsorRepository.ExistsAsync(id);

            if (!exists)
                throw new KeyNotFoundException($"Sponsor con ID {id} no encontrado");

            await _sponsorRepository.DeleteAsync(id);

            _logger.LogInformation($"Sponsor eliminado: {id}");
        }

        // Register a Sponsor for a tournament
        public async Task<TournamentSponsor> RegisterSponsorAsync(int tournamentId, int sponsorId, decimal contractAmount)
        {
            if (contractAmount <= 0)
                throw new InvalidOperationException("El monto del contrato debe ser mayor a 0.");

            Tournament tournament = await _tournamentRepository.GetByIdAsync(tournamentId)
                ?? throw new KeyNotFoundException("Torneo no encontrado");

            Sponsor sponsor = await _sponsorRepository.GetByIdAsync(sponsorId)
                ?? throw new KeyNotFoundException("Sponsor no encontrado");

            TournamentSponsor? existing = await _tournamentSponsorRepository
                .GetByTournamentAndSponsorAsync(tournamentId, sponsorId);

            if (existing != null)
                throw new InvalidOperationException("El sponsor ya está registrado en este torneo");

            TournamentSponsor ts = new TournamentSponsor
            {
                SponsorId = sponsorId,
                TournamentId = tournamentId,
                ContractAmount = contractAmount
            };

            var result = await _tournamentSponsorRepository.CreateWithIncludesAsync(ts);

            _logger.LogInformation($"Sponsor {sponsorId} registrado en torneo {tournamentId}");

            return result;
        }

        public async Task<IEnumerable<TournamentSponsor>> GetSponsorsByTournamentAsync(int tournamentId)
        {
            return await _tournamentSponsorRepository.GetSponsorsByTournamentAsync(tournamentId);
        }
    }
}





