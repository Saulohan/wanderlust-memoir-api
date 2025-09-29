using AutoMapper;
using WanderlustMemoir.Application.DTOs.Destinations;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;
using WanderlustMemoir.Domain.Entities;

namespace WanderlustMemoir.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        try
        {
            // Destination mappings
            CreateMap<Destination, DestinationDto>()
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString().ToLower()))
                .ForMember(dest => dest.VisitDate, opt => opt.MapFrom(src => src.DateVisited))
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos ?? new List<DestinationPhoto>()));
            
            CreateMap<CreateDestinationDto, Destination>()
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => ParsePriority(src.Priority)))
                .ForMember(dest => dest.Photos, opt => opt.Ignore());
            
            CreateMap<UpdateDestinationDto, Destination>()
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority != null ? ParsePriority(src.Priority) : (Priority?)null))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // DestinationPhoto mappings
            CreateMap<DestinationPhoto, DestinationPhotoDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => $"/api/destinations/{src.DestinationId}/photos/{src.Id}"))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => $"/api/destinations/{src.DestinationId}/photos/{src.Id}/thumbnail"))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Caption ?? src.Description))
                .ForMember(dest => dest.DateTaken, opt => opt.MapFrom(src => src.DateTaken));

            // VisitedPlace mappings
            CreateMap<VisitedPlace, VisitedPlaceDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.VisitDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos ?? new List<VisitedPlacePhoto>()));
            
            CreateMap<CreateVisitedPlaceDto, VisitedPlace>()
                .ForMember(dest => dest.VisitDate, opt => opt.MapFrom(src => 
                    !string.IsNullOrEmpty(src.Date) ? DateTime.Parse(src.Date) : src.VisitDate))
                .ForMember(dest => dest.Photos, opt => opt.Ignore());
            
            CreateMap<UpdateVisitedPlaceDto, VisitedPlace>()
                .ForMember(dest => dest.VisitDate, opt => opt.MapFrom(src => 
                    !string.IsNullOrEmpty(src.Date) ? DateTime.Parse(src.Date) : src.VisitDate))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // VisitedPlacePhoto mappings - Gerar URLs para servir as imagens BLOB
            CreateMap<VisitedPlacePhoto, VisitedPlacePhotoDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => $"/api/visitedplaces/{src.VisitedPlaceId}/photos/{src.Id}"))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => $"/api/visitedplaces/{src.VisitedPlaceId}/photos/{src.Id}/thumbnail"))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Caption ?? src.Description))
                .ForMember(dest => dest.DateTaken, opt => opt.MapFrom(src => src.DateTaken));
        }
        catch (Exception ex)
        {
            // Log error if mapping fails
            System.Diagnostics.Debug.WriteLine($"AutoMapper configuration error: {ex.Message}");
            throw;
        }
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