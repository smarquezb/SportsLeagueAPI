using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class TournamentSponsorService : ITournamentSponsorService
    {
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;
        private readonly ILogger<TournamentSponsorService> _logger;

        public TournamentSponsorService(
            ITournamentSponsorRepository tournamentSponsorRepository,
            ILogger<TournamentSponsorService> logger)
        {
            _tournamentSponsorRepository = tournamentSponsorRepository;
            _logger = logger;
        }
    }
}

