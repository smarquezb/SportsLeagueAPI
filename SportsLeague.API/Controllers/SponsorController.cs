using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorService _service;
        private readonly IMapper _objectMapper;

        public SponsorController(ISponsorService service, IMapper objectMapper)
        {
            _service = service;
            _objectMapper = objectMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> ListAllSponsors()
        {
            var data = await _service.GetAllAsync();
            var results = _objectMapper.Map<IEnumerable<SponsorResponseDTO>>(data);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SponsorResponseDTO>> GetSponsorById(int id)
        {
            var record = await _service.GetByIdAsync(id);

            return record is not null
                ? Ok(_objectMapper.Map<SponsorResponseDTO>(record))
                : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<SponsorResponseDTO>> AddNewSponsor([FromBody] SponsorRequestDTO requestDto)
        {
            try
            {
                var newEntity = _objectMapper.Map<Sponsor>(requestDto);
                var createdSponsor = await _service.CreateAsync(newEntity);

                var finalOutput = _objectMapper.Map<SponsorResponseDTO>(createdSponsor);

                return CreatedAtAction(nameof(GetSponsorById), new { id = finalOutput.Id }, finalOutput);
            }
            catch (InvalidOperationException error)
            {
                return Conflict(new { error.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditSponsor(int id, [FromBody] SponsorRequestDTO updateDto)
        {
            try
            {
                var mappedSponsor = _objectMapper.Map<Sponsor>(updateDto);
                await _service.UpdateAsync(id, mappedSponsor);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSponsor(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/tournaments")]
        public async Task<ActionResult<IEnumerable<TournamentSponsorResponseDTO>>> FetchAssociatedTournaments(int id)
        {
            try
            {
                var tournamentLinks = await _service.GetSponsorsByTournamentAsync(id);
                return Ok(_objectMapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(tournamentLinks));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { info = ex.Message });
            }
        }

        [HttpPost("{id}/tournaments")]
        public async Task<IActionResult> AssignSponsorToTournament(int id, TournamentSponsorRequestDTO inputDto)
        {
            try
            {
                var operationResult = await _service.RegisterSponsorAsync(id, inputDto.SponsorId, inputDto.ContractAmount);
                var dtoResult = _objectMapper.Map<TournamentSponsorResponseDTO>(operationResult);

                return CreatedAtAction(nameof(FetchAssociatedTournaments), new { id }, dtoResult);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is KeyNotFoundException)
            {
                return ex is InvalidOperationException
                    ? Conflict(new { detail = ex.Message })
                    : NotFound(new { detail = ex.Message });
            }
        }

        [HttpDelete("{id}/tournaments/{tid}")]
        public async Task<IActionResult> UnregisterTournamentLink(int id, int tid)
        {
            try
            {
                await _service.RemoveSponsorAsync(id, tid);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("El vínculo especificado no existe.");
            }
        }
    }
}




