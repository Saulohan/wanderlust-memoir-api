using AutoMapper;
using MediatR;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;
using WanderlustMemoir.Application.Features.VisitedPlaces.Queries;
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
        return visitedPlace == null ? null : _mapper.Map<VisitedPlaceDto>(visitedPlace);
    }
}