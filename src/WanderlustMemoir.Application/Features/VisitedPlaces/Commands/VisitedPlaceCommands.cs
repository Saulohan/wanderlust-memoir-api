using MediatR;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;

namespace WanderlustMemoir.Application.Features.VisitedPlaces.Commands;

public record CreateVisitedPlaceCommand(CreateVisitedPlaceDto VisitedPlace) : IRequest<VisitedPlaceDto>;

public record UpdateVisitedPlaceCommand(int Id, UpdateVisitedPlaceDto VisitedPlace) : IRequest<VisitedPlaceDto?>;

public record DeleteVisitedPlaceCommand(int Id) : IRequest<bool>;