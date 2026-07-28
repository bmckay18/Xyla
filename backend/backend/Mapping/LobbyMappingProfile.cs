using AutoMapper;
using Backend.Services.Lobbies.Models;

namespace Backend.Mapping
{
    public class LobbyMappingProfile : Profile
    {
        public LobbyMappingProfile()
        {
            CreateMap<Lobby, LobbyDto>();
        }
    }
}
