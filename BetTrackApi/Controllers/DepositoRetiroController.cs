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
    public class DepositoRetiroController : ControllerBase
    {
        private readonly BetTrackContext _context;

        private readonly IMapper _mapper;

        public DepositoRetiroController(BetTrackContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/DepositoRetiro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DtoDepositoRetiro>>> ObtenerDepositosRetiros()
        {
            return _mapper.Map<List<DtoDepositoRetiro>>(await _context.RelDepositosRetiros.ToListAsync());
        }

        // GET: api/DepositoRetiro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DtoDepositoRetiro>> ObtenerDepositoRetiro(long id)
        {
            var relDepositosRetiro = await _context.RelDepositosRetiros.FindAsync(id);

            if (relDepositosRetiro == null)
            {
                return NotFound();
            }
            DtoDepositoRetiro depositoRetiro = _mapper.Map<DtoDepositoRetiro>(relDepositosRetiro);
            return depositoRetiro;
        }

        // PUT: api/DepositoRetiro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarDepositoRetiro(long id, DtoDepositoRetiro relDepositosRetiro)
        {
            if (id != relDepositosRetiro.DepositoRetiroId)
            {
                return BadRequest();
            }
            RelDepositosRetiro depositoRetiro = _mapper.Map<RelDepositosRetiro>(relDepositosRetiro);

            _context.Entry(depositoRetiro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExisteDepositoRetiro(id))
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

        // POST: api/DepositoRetiro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DtoDepositoRetiro>> RegistrarDepositoRetiro(DtoDepositoRetiro relDepositosRetiro)
        {
            RelDepositosRetiro depositoRetiro = _mapper.Map<RelDepositosRetiro>(relDepositosRetiro);

            _context.RelDepositosRetiros.Add(depositoRetiro);
            await _context.SaveChangesAsync();
            relDepositosRetiro = _mapper.Map<DtoDepositoRetiro>(depositoRetiro);

            return CreatedAtAction("ObtenerDepositoRetiro", new { id = relDepositosRetiro.DepositoRetiroId }, relDepositosRetiro);
        }

        // DELETE: api/DepositoRetiro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarDepositoRetiro(long id)
        {
            var relDepositosRetiro = await _context.RelDepositosRetiros.FindAsync(id);
            if (relDepositosRetiro == null)
            {
                return NotFound();
            }

            _context.RelDepositosRetiros.Remove(relDepositosRetiro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExisteDepositoRetiro(long id)
        {
            return _context.RelDepositosRetiros.Any(e => e.DepositoRetiroId == id);
        }
    }
}
