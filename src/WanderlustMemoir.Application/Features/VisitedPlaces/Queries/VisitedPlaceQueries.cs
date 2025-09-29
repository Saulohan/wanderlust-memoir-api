using MediatR;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;

namespace WanderlustMemoir.Application.Features.VisitedPlaces.Queries;

public record GetAllVisitedPlacesQuery : IRequest<IEnumerable<VisitedPlaceDto>>;

public record GetVisitedPlaceByIdQuery(int Id) : IRequest<VisitedPlaceDto?>;