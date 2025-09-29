using AutoMapper;
using WanderlustMemoir.Application.DTOs.Destinations;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;
using WanderlustMemoir.Application.DTOs.TravelStats;
using WanderlustMemoir.Domain.Entities;
using WanderlustMemoir.Domain.ValueObjects;

namespace WanderlustMemoir.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Destination mappings
        CreateMap<Destination, DestinationDto>()
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString().ToLower()));
        
        CreateMap<CreateDestinationDto, Destination>()
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => ParsePriority(src.Priority)));
        
        CreateMap<UpdateDestinationDto, Destination>()
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority != null ? ParsePriority(src.Priority) : (Priority?)null))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // VisitedPlace mappings
        CreateMap<VisitedPlace, VisitedPlaceDto>();
        CreateMap<CreateVisitedPlaceDto, VisitedPlace>();
        CreateMap<UpdateVisitedPlaceDto, VisitedPlace>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Photo mappings
        CreateMap<Photo, PhotoDto>();

        // TravelStats mappings
        CreateMap<TravelStats, TravelStatsDto>();
    }

    private static Priority ParsePriority(string priorityString)
    {
        return priorityString?.ToLower() switch
        {
            "high" => Priority.High,
            "medium" => Priority.Medium,
            "low" => Priority.Low,
            _ => Priority.Medium
        };
    }
}