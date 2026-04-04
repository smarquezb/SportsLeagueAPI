namespace SportsLeague.API.DTOs.Response
{
    /// <summary>
    /// Representa la información detallada de la vinculación entre un torneo y su patrocinador.
    /// </summary>
    public class TournamentSponsorResponseDTO
    {
        // Identificador principal de la relación en la base de datos
        public int Id { get; set; }

        /// <value>Obtiene o establece el ID del torneo asociado</value>
        public int TournamentId { get; set; }

        /// <value>Nombre descriptivo del torneo</value>
        public string TournamentName { get; set; } = string.Empty;

        // Referencia al ID único del patrocinador
        public int SponsorId { get; set; }

        // Nombre o razón social del sponsor
        public string SponsorName { get; set; } = string.Empty;

        // Monto pactado en el contrato de patrocinio
        public decimal ContractAmount { get; set; }

        // Registro de fecha y hora en que se formalizó la unión
        public DateTime JoinedAT { get; set; }
    }
}



