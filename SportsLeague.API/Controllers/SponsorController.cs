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
        private readonly IMapper _mapper;

        public SponsorController(ISponsorService sponsorService, IMapper mapper)
        {
            _service = sponsorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<SponsorResponseDTO>>(data));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SponsorResponseDTO>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result is null) return NotFound();

            return Ok(_mapper.Map<SponsorResponseDTO>(result));
        }

        [HttpPost]
        public async Task<ActionResult<SponsorResponseDTO>> Create([FromBody] SponsorRequestDTO request)
        {
            try
            {
                var entity = _mapper.Map<Sponsor>(request);
                var created = await _service.CreateAsync(entity);

                var output = _mapper.Map<SponsorResponseDTO>(created);

                return CreatedAtAction(nameof(GetById), new { id = output.Id }, output);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SponsorRequestDTO payload)
        {
            try
            {
                var entity = _mapper.Map<Sponsor>(payload);
                await _service.UpdateAsync(id, entity);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
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

      
        [HttpGet("tournaments/{tournamentId:int}/sponsors")]
        public async Task<ActionResult<IEnumerable<TournamentSponsorResponseDTO>>> GetSponsorsByTournament(int tournamentId)
        {
            var data = await _service.GetSponsorsByTournamentAsync(tournamentId);
            return Ok(_mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(data));
        }

      
        [HttpPost("tournaments/{tournamentId:int}/sponsors")]
        public async Task<IActionResult> RegisterSponsor(int tournamentId, [FromBody] TournamentSponsorRequestDTO input)
        {
            try
            {
                var result = await _service.RegisterSponsorAsync(
                    tournamentId,
                    input.SponsorId,
                    input.ContractAmount
                );

                var dto = _mapper.Map<TournamentSponsorResponseDTO>(result);

                return CreatedAtAction(nameof(GetSponsorsByTournament), new { tournamentId }, dto);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(new { details = e.Message });
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { details = e.Message });
            }
        }

     
        [HttpDelete("tournaments/{tournamentId:int}/sponsors/{sponsorId:int}")]
        public async Task<IActionResult> RemoveSponsor(int tournamentId, int sponsorId)
        {
            try
            {
                await _service.RemoveSponsorAsync(tournamentId, sponsorId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}

