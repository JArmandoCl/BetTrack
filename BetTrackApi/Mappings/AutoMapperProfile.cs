using AutoMapper;
using BetTrackApi.Dtos;
using BetTrackApi.Models;

namespace BetTrackApi.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapeo de Usuario -> DtoUsuario
            CreateMap<Usuario, DtoUsuario>().ReverseMap();
        }
    }
}
