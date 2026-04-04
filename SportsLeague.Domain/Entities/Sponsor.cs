using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities
{
    public class Sponsor : AuditBase
    {
        // Basic sponsor information
        public string Name { get; set; } = "";
        public string ContactEmail { get; set; } = "";

       public string? Phone { get; set; }
        public string? WebsiteUrl { get; set; }

        public SponsorCategory Category { get; set; }

        // Navigation properties
        public virtual ICollection<TournamentSponsor> TournamentSponsors { get; set; } = new List<TournamentSponsor>();
    }
}
