using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BetTrackApi.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using BetTrackApi.Dtos;

namespace BetTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioCasinoController : ControllerBase
    {
        private readonly BetTrackContext _context;
        private readonly IMapper _mapper;

        public UsuarioCasinoController(BetTrackContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/UsuarioCasino
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DtoUsuarioCasino>>> ObtenerUsuarioCasinos()
        {
            return _mapper.Map<List<DtoUsuarioCasino>>(await _context.RelUsuariosCasinos.ToListAsync());
        }

        // GET: api/UsuarioCasino/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DtoUsuarioCasino>> ObtenerUsuarioCasino(long id)
        {
            var relUsuariosCasino = await _context.RelUsuariosCasinos.FindAsync(id);

            if (relUsuariosCasino == null)
            {
                return NotFound();
            }
            DtoUsuarioCasino usuarioCasino = _mapper.Map<DtoUsuarioCasino>(relUsuariosCasino);
            return usuarioCasino;
        }

        // PUT: api/UsuarioCasino/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuarioCasino(long id, DtoUsuarioCasino relUsuariosCasino)
        {
            if (id != relUsuariosCasino.UsuarioCasinoId)
            {
                return BadRequest();
            }
            RelUsuariosCasino usuarioCasino = _mapper.Map<RelUsuariosCasino>(relUsuariosCasino);
            _context.Entry(usuarioCasino).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExisteUsuarioCasino(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UsuarioCasino
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DtoUsuarioCasino>> RegistrarUsuarioCasino(DtoUsuarioCasino relUsuariosCasino)
        {
            RelUsuariosCasino usuarioCasino = _mapper.Map<RelUsuariosCasino>(relUsuariosCasino);
            _context.RelUsuariosCasinos.Add(usuarioCasino);
            await _context.SaveChangesAsync();
            relUsuariosCasino = _mapper.Map<DtoUsuarioCasino>(usuarioCasino);
            return CreatedAtAction("ObtenerUsuarioCasino", new { id = relUsuariosCasino.UsuarioCasinoId }, relUsuariosCasino);
        }

        // DELETE: api/UsuarioCasino/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuarioCasino(long id)
        {
            var relUsuariosCasino = await _context.RelUsuariosCasinos.FindAsync(id);
            if (relUsuariosCasino == null)
            {
                return NotFound();
            }

            _context.RelUsuariosCasinos.Remove(relUsuariosCasino);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExisteUsuarioCasino(long id)
        {
            return _context.RelUsuariosCasinos.Any(e => e.UsuarioCasinoId == id);
        }
    }
}
