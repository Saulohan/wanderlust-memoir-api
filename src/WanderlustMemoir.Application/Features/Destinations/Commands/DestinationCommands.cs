using MediatR;
using WanderlustMemoir.Application.DTOs.Destinations;
using Microsoft.AspNetCore.Http;

namespace WanderlustMemoir.Application.Features.Destinations.Commands;

public record CreateDestinationCommand(CreateDestinationDto Destination) : IRequest<DestinationDto>;

public record UpdateDestinationCommand(int Id, UpdateDestinationDto Destination) : IRequest<DestinationDto?>;

public record DeleteDestinationCommand(int Id) : IRequest<bool>;

public record ToggleDestinationVisitedCommand(int Id) : IRequest<DestinationDto?>;

public record ToggleDestinationVisitedWithDateCommand(int Id, string? VisitDate) : IRequest<DestinationDto?>;

public record UpdateDestinationRatingCommand(int Id, int Rating) : IRequest<DestinationDto?>;

public record UploadDestinationPhotosCommand(int Id, List<IFormFile> Photos) : IRequest<DestinationDto?>;