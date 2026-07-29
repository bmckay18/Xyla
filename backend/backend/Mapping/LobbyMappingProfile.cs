using AutoMapper;
using Backend.Services.Lobbies.Models;

namespace Backend.Mapping
{
    public class LobbyMappingProfile : Profile
    {
        public LobbyMappingProfile()
        {
            CreateMap<Lobby, LobbyDetailsDto>()
                .ForMember(
                    dest => dest.HostId,
                    opt => opt.MapFrom(src => src.Host.Id))
                .ForMember(
                    dest => dest.Players,
                    opt => opt.MapFrom(src => src.Players));
        }
    }
}
