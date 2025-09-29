using MediatR;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;
using WanderlustMemoir.Application.DTOs.TravelStats;
using WanderlustMemoir.Domain.Entities;

namespace WanderlustMemoir.Application.Features.VisitedPlaces.Queries;

public record GetAllVisitedPlacesQuery : IRequest<IEnumerable<VisitedPlaceDto>>;

public record GetVisitedPlaceByIdQuery(int Id) : IRequest<VisitedPlaceDto?>;

public record GetPhotoByIdQuery(int PhotoId) : IRequest<VisitedPlacePhoto?>;

public record GetTravelStatsQuery : IRequest<TravelStatsDto>;