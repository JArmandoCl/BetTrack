using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BetTrackApi.Models;
using AutoMapper;
using BetTrackApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace BetTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriaUsuarioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly BetTrackContext _context;

        public CategoriaUsuarioController(BetTrackContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/CategoriaUsuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DtoCategoriaUsuario>>> ObtenerCategoriasUsuarios()
        {
            return _mapper.Map<List<DtoCategoriaUsuario>>(await _context.RelCategoriasUsuarios.ToListAsync());
        }

        // GET: api/CategoriaUsuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DtoCategoriaUsuario>> ObtenerCategoriaUsuario(long id)
        {
            var relCategoriasUsuarioContext = await _context.RelCategoriasUsuarios.FindAsync(id);

            if (relCategoriasUsuarioContext == null)
            {
                return NotFound();
            }
            DtoCategoriaUsuario categoriaUsuario = _mapper.Map<DtoCategoriaUsuario>(relCategoriasUsuarioContext);

            return categoriaUsuario;
        }

        // PUT: api/CategoriaUsuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCategoriaUsuario(long id, DtoCategoriaUsuario categoriaUsuario)
        {
            if (id != categoriaUsuario.CategoriaUsuarioId)
            {
                return BadRequest();
            }
            RelCategoriasUsuario categoriaUsuarioContext = _mapper.Map<RelCategoriasUsuario>(categoriaUsuario);

            _context.Entry(categoriaUsuarioContext).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExisteCategoriaUsuario(id))
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

        // POST: api/CategoriaUsuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DtoCategoriaUsuario>> RegistrarCategoriaUsuario(DtoCategoriaUsuario categoriaUsuario)
        {
            RelCategoriasUsuario categoriaUsuarioContext = _mapper.Map<RelCategoriasUsuario>(categoriaUsuario);

            _context.RelCategoriasUsuarios.Add(categoriaUsuarioContext);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (ExisteCategoriaUsuario(categoriaUsuarioContext.CategoriaUsuarioId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            categoriaUsuario = _mapper.Map<DtoCategoriaUsuario>(categoriaUsuarioContext);

            return CreatedAtAction("ObtenerCategoriaUsuario", new { id = categoriaUsuario.CategoriaUsuarioId }, categoriaUsuario);
        }

        // DELETE: api/CategoriaUsuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCategoriaUsuario(long id)
        {
            var relCategoriasUsuario = await _context.RelCategoriasUsuarios.FindAsync(id);
            if (relCategoriasUsuario == null)
            {
                return NotFound();
            }

            _context.RelCategoriasUsuarios.Remove(relCategoriasUsuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExisteCategoriaUsuario(long id)
        {
            return _context.RelCategoriasUsuarios.Any(e => e.CategoriaUsuarioId == id);
        }
    }
}
