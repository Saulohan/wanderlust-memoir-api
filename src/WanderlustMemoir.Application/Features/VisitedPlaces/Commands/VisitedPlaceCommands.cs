using MediatR;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;
using Microsoft.AspNetCore.Http;

namespace WanderlustMemoir.Application.Features.VisitedPlaces.Commands;

public record CreateVisitedPlaceCommand(CreateVisitedPlaceDto VisitedPlace) : IRequest<VisitedPlaceDto>;

public record UpdateVisitedPlaceCommand(int Id, UpdateVisitedPlaceDto VisitedPlace) : IRequest<VisitedPlaceDto?>;

public record DeleteVisitedPlaceCommand(int Id) : IRequest<bool>;

public record UploadVisitedPlacePhotosCommand(int Id, List<IFormFile> Photos) : IRequest<VisitedPlaceDto?>;

public record UpdateVisitedPlaceRatingCommand(int Id, int Rating) : IRequest<VisitedPlaceDto?>;

public record DeleteVisitedPlacePhotoCommand(int PlaceId, int PhotoId) : IRequest<bool>;