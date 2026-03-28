using AutoMapper;
using NarutoGame.Application.DTOs;
using NarutoGame.Core.Entities;

namespace NarutoGame.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>();
        
        // Player mappings
        CreateMap<Player, PlayerDto>()
            .ForMember(dest => dest.Village, opt => opt.MapFrom(src => src.Village.ToString()))
            .ForMember(dest => dest.Element, opt => opt.MapFrom(src => src.Element.ToString()));
        
        CreateMap<PlayerStats, PlayerStatsDto>();
        
        // Npc mappings
        CreateMap<Npc, NpcDto>()
            .ForMember(dest => dest.Dialogues, opt => opt.MapFrom(src => 
                src.Dialogues != null 
                    ? src.Dialogues.OrderBy(d => d.Order).Select(d => d.Text).ToList() 
                    : new List<string>()));
        
        // Mission mappings
        CreateMap<Mission, MissionDto>();
    }
}
