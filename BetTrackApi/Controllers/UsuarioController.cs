using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BetTrackApi.Models;
using BetTrackApi.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using BetTrackApi.Models.Utilities;

namespace BetTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly BetTrackContext _context;
        private readonly IMapper _mapper;

        public UsuarioController(BetTrackContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DtoUsuario>>> ObtenerUsuarios()
        {
            return _mapper.Map<List<DtoUsuario>>(await _context.Usuarios.ToListAsync());
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DtoUsuario>> ObtenerUsuario(long id)
        {
            var userContext = await _context.Usuarios.FindAsync(id);

            if (userContext == null)
            {
                return NotFound();
            }
            DtoUsuario user = _mapper.Map<DtoUsuario>(userContext);

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(long id, DtoUsuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return BadRequest();
            }
            Usuario usuarioContext = _mapper.Map<Usuario>(usuario);
            _context.Entry(usuarioContext).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExisteUsuario(id))
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<DtoUsuario>> RegistrarUsuario(DtoUsuario usuario)
        {
            Usuario userContext = _mapper.Map<Usuario>(usuario);
            string hashedPassword = PasswordHasher.HashPassword(usuario.Contrasenia);
            userContext.Contrasenia = hashedPassword;
            _context.Usuarios.Add(userContext);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (ExisteUsuario(userContext.UsuarioId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            usuario = _mapper.Map<DtoUsuario>(userContext);
            return CreatedAtAction("ObtenerUsuario", new { id = usuario.UsuarioId }, usuario);
        }

        [HttpPost]
        public async Task<ActionResult<DtoSeguidor>> RegistrarSeguidor(DtoSeguidor seguidor)
        {
            RelSeguidore contextSeguidor = _mapper.Map<RelSeguidore>(seguidor);
            _context.RelSeguidores.Add(contextSeguidor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw;
            }
            seguidor = _mapper.Map<DtoSeguidor>(contextSeguidor);
            return seguidor;
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(long id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExisteUsuario(long id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
