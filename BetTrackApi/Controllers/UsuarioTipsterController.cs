using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class UsuarioTipsterController : ControllerBase
    {
        private readonly BetTrackContext _context;

        private readonly IMapper _mapper;

        public UsuarioTipsterController(BetTrackContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/UsuarioTipster
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DtoUsuarioTipster>>> ObtenerUsuarioTipsters()
        {
            return _mapper.Map<List<DtoUsuarioTipster>>(await _context.RelUsuarioTipsters.ToListAsync());
        }

        // GET: api/UsuarioTipster/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DtoUsuarioTipster>> ObtenerUsuarioTipster(long id)
        {
            var relUsuarioTipster = await _context.RelUsuarioTipsters.FindAsync(id);

            if (relUsuarioTipster == null)
            {
                return NotFound();
            }
            DtoUsuarioTipster usuarioTipster = _mapper.Map<DtoUsuarioTipster>(relUsuarioTipster);
            return usuarioTipster;
        }

        // PUT: api/UsuarioTipster/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuarioTipster(long id, DtoUsuarioTipster relUsuarioTipster)
        {
            if (id != relUsuarioTipster.UsuarioTipsterId)
            {
                return BadRequest();
            }
            RelUsuarioTipster usuarioTipster = _mapper.Map<RelUsuarioTipster>(relUsuarioTipster);
            _context.Entry(usuarioTipster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExisteUsuarioTipster(id))
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

        // POST: api/UsuarioTipster
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DtoUsuarioTipster>> RegistrarUsuarioTipster(DtoUsuarioTipster relUsuarioTipster)
        {
            RelUsuarioTipster usuarioTipster = _mapper.Map<RelUsuarioTipster>(relUsuarioTipster);
            _context.RelUsuarioTipsters.Add(usuarioTipster);
            await _context.SaveChangesAsync();
            relUsuarioTipster = _mapper.Map<DtoUsuarioTipster>(usuarioTipster);

            return CreatedAtAction("ObtenerUsuarioTipster", new { id = relUsuarioTipster.UsuarioTipsterId }, relUsuarioTipster);
        }

        // DELETE: api/UsuarioTipster/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuarioTipster(long id)
        {
            var relUsuarioTipster = await _context.RelUsuarioTipsters.FindAsync(id);
            if (relUsuarioTipster == null)
            {
                return NotFound();
            }

            _context.RelUsuarioTipsters.Remove(relUsuarioTipster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExisteUsuarioTipster(long id)
        {
            return _context.RelUsuarioTipsters.Any(e => e.UsuarioTipsterId == id);
        }
    }
}
