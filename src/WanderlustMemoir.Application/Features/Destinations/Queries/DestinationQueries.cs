using MediatR;
using WanderlustMemoir.Application.DTOs.Destinations;
using WanderlustMemoir.Domain.Entities;

namespace WanderlustMemoir.Application.Features.Destinations.Queries;

public record GetAllDestinationsQuery : IRequest<IEnumerable<DestinationDto>>;

public record GetDestinationByIdQuery(int Id) : IRequest<DestinationDto?>;

public record GetTravelStatsQuery : IRequest<WanderlustMemoir.Application.DTOs.TravelStats.TravelStatsDto>;

public record GetDestinationPhotoByIdQuery(int PhotoId) : IRequest<DestinationPhoto?>;