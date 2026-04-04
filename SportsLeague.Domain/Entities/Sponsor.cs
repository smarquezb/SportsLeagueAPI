using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities
{
    public class Sponsor : AuditBase
    {
        // Información básica del patrocinador
        public string Name { get; set; } = "";
        public string ContactEmail { get; set; } = "";

      
        public SponsorCategory Category { get; set; }

      
        public string? Phone { get; set; }
        public string? WebsiteUrl { get; set; }

        // Propiedades de navegación
        public virtual ICollection<TournamentSponsor> TournamentSponsors { get; set; } = new List<TournamentSponsor>();
    }
}
