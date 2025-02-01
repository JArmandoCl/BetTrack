using AutoMapper;
using BetTrackApi.Dtos;
using BetTrackApi.Models;
using BetTrackApi.Models.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BetTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacionController : ControllerBase
    {
        private readonly string secretKey;
        private readonly BetTrackContext _context;
        private readonly IMapper _mapper;
        public AutorizacionController(IConfiguration configuration, BetTrackContext context, IMapper mapper)
        {
            secretKey = configuration["JwtSettings:SecretKey"];
            _mapper = mapper;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] DtoUsuario request)
        {
            Usuario userMatched = await _context.Usuarios.FirstOrDefaultAsync(x => EF.Functions.Like(x.Email, request.Email));
            string? hashedPass = userMatched?.Contrasenia;
            if ((!string.IsNullOrWhiteSpace(hashedPass)) && PasswordHasher.VerifyPassword(request.Contrasenia, hashedPass))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(secretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, request.Email)
                    }),
                    Expires = DateTime.UtcNow.AddHours(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { Token = tokenHandler.WriteToken(token) });
            }

            return Unauthorized("Usuario o contraseña incorrectos");
        }
    }
}
