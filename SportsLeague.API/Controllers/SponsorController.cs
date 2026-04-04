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

        public SponsorController(ISponsorService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<SponsorResponseDTO>>(data));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sponsor = await _service.GetByIdAsync(id);
            if (sponsor == null) return NotFound();
            return Ok(_mapper.Map<SponsorResponseDTO>(sponsor));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SponsorRequestDTO dto)
        {
            try
            {
                var entity = _mapper.Map<Sponsor>(dto);
                var result = await _service.CreateAsync(entity);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, _mapper.Map<SponsorResponseDTO>(result));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SponsorRequestDTO dto)
        {
            try
            {
                var entity = _mapper.Map<Sponsor>(dto);
                await _service.UpdateAsync(id, entity);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch { return NotFound(); }
        }
    }
}