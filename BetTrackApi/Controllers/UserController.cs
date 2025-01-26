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
    public class UserController : ControllerBase
    {
        private readonly BetTrackContext _context;
        private readonly IMapper _mapper;

        public UserController(BetTrackContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DtoUsuario>>> GetUsers()
        {
            return _mapper.Map<List<DtoUsuario>>(await _context.Usuarios.ToListAsync());
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DtoUsuario>> GetUser(long id)
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
        public async Task<IActionResult> PutUser(long id, DtoUsuario usuario)
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
                if (!UserExists(id))
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
        public async Task<ActionResult<DtoUsuario>> PostUser(DtoUsuario user)
        {
            Usuario userContext = _mapper.Map<Usuario>(user);
            string hashedPassword = PasswordHasher.HashPassword(user.Contrasenia);
            userContext.Contrasenia = hashedPassword;
            _context.Usuarios.Add(userContext);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (UserExists(userContext.UsuarioId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            user = _mapper.Map<DtoUsuario>(userContext);
            return CreatedAtAction("GetUser", new { id = userContext.UsuarioId }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
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

        private bool UserExists(long id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
