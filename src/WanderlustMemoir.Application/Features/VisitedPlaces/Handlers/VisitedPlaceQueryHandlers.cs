using AutoMapper;
using MediatR;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;
using WanderlustMemoir.Application.DTOs.TravelStats;
using WanderlustMemoir.Application.Features.VisitedPlaces.Queries;
using WanderlustMemoir.Domain.Entities;
using WanderlustMemoir.Domain.Repositories;

namespace WanderlustMemoir.Application.Features.VisitedPlaces.Handlers;

public class GetAllVisitedPlacesHandler : IRequestHandler<GetAllVisitedPlacesQuery, IEnumerable<VisitedPlaceDto>>
{
    private readonly IVisitedPlaceRepository _repository;
    private readonly IMapper _mapper;

    public GetAllVisitedPlacesHandler(IVisitedPlaceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VisitedPlaceDto>> Handle(GetAllVisitedPlacesQuery request, CancellationToken cancellationToken)
    {
        var visitedPlaces = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<VisitedPlaceDto>>(visitedPlaces);
    }
}

public class GetVisitedPlaceByIdHandler : IRequestHandler<GetVisitedPlaceByIdQuery, VisitedPlaceDto?>
{
    private readonly IVisitedPlaceRepository _repository;
    private readonly IMapper _mapper;

    public GetVisitedPlaceByIdHandler(IVisitedPlaceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<VisitedPlaceDto?> Handle(GetVisitedPlaceByIdQuery request, CancellationToken cancellationToken)
    {
        var visitedPlace = await _repository.GetByIdAsync(request.Id);
        return visitedPlace != null ? _mapper.Map<VisitedPlaceDto>(visitedPlace) : null;
    }
}

public class GetPhotoByIdHandler : IRequestHandler<GetPhotoByIdQuery, VisitedPlacePhoto?>
{
    private readonly IVisitedPlaceRepository _repository;

    public GetPhotoByIdHandler(IVisitedPlaceRepository repository)
    {
        _repository = repository;
    }

    public async Task<VisitedPlacePhoto?> Handle(GetPhotoByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetPhotoByIdAsync(request.PhotoId);
    }
}

public class GetTravelStatsHandler : IRequestHandler<GetTravelStatsQuery, TravelStatsDto>
{
    private readonly IVisitedPlaceRepository _visitedPlaceRepository;
    private readonly IDestinationRepository _destinationRepository;

    public GetTravelStatsHandler(IVisitedPlaceRepository visitedPlaceRepository, IDestinationRepository destinationRepository)
    {
        _visitedPlaceRepository = visitedPlaceRepository;
        _destinationRepository = destinationRepository;
    }

    public async Task<TravelStatsDto> Handle(GetTravelStatsQuery request, CancellationToken cancellationToken)
    {
        // Buscar todos os dados necessários
        var visitedPlaces = await _visitedPlaceRepository.GetAllAsync();
        var destinations = await _destinationRepository.GetAllAsync();

        // Calcular estatísticas
        var visitedPlacesCount = visitedPlaces.Count();
        var dreamDestinationsCount = destinations.Count(d => !d.IsVisited); // Destinos não visitados
        var sharedPhotosCount = visitedPlaces.Sum(vp => vp.Photos?.Count ?? 0) + destinations.Sum(d => d.Photos?.Count ?? 0);
        var exploredCountriesCount = visitedPlaces.Select(vp => vp.Country.Trim().ToLower()).Distinct().Count();

        return new TravelStatsDto
        {
            DreamDestinations = dreamDestinationsCount,
            VisitedPlaces = visitedPlacesCount,
            SharedPhotos = sharedPhotosCount,
            ExploredCountries = exploredCountriesCount
        };
    }
}