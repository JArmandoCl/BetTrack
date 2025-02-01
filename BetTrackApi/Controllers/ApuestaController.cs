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
    public class ApuestaController : ControllerBase
    {
        private readonly BetTrackContext _context;

        private readonly IMapper _mapper;

        public ApuestaController(BetTrackContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Apuesta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DtoApuesta>>> ObtenerApuestas()
        {
            return _mapper.Map<List<DtoApuesta>>(await _context.RelApuestas.ToListAsync());
        }

        // GET: api/Apuesta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DtoApuesta>> ObtenerApuesta(long id)
        {
            var relApuesta = await _context.RelApuestas.FindAsync(id);

            if (relApuesta == null)
            {
                return NotFound();
            }
            DtoApuesta apuesta = _mapper.Map<DtoApuesta>(relApuesta);
            return apuesta;
        }

        // PUT: api/Apuesta/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarApuesta(long id, DtoApuesta relApuesta)
        {
            if (id != relApuesta.ApuestaId)
            {
                return BadRequest();
            }
            RelApuesta apuesta = _mapper.Map<RelApuesta>(relApuesta);
            _context.Entry(apuesta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExisteApuesta(id))
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

        // POST: api/Apuesta
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DtoApuesta>> RegistrarApuesta(DtoApuesta relApuesta)
        {
            RelApuesta apuesta = _mapper.Map<RelApuesta>(relApuesta);

            _context.RelApuestas.Add(apuesta);
            await _context.SaveChangesAsync();
            relApuesta = _mapper.Map<DtoApuesta>(apuesta);
            return CreatedAtAction("ObtenerApuesta", new { id = relApuesta.ApuestaId }, relApuesta);
        }

        // DELETE: api/Apuesta/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarApuesta(long id)
        {
            var relApuesta = await _context.RelApuestas.FindAsync(id);
            if (relApuesta == null)
            {
                return NotFound();
            }

            _context.RelApuestas.Remove(relApuesta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExisteApuesta(long id)
        {
            return _context.RelApuestas.Any(e => e.ApuestaId == id);
        }
    }
}
