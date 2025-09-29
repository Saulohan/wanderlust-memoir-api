using MediatR;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;
using WanderlustMemoir.Application.DTOs.TravelStats;
using WanderlustMemoir.Application.Features.VisitedPlaces.Commands;
using WanderlustMemoir.Application.Features.VisitedPlaces.Queries;
using WanderlustMemoir.Application.Interfaces;
using WanderlustMemoir.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace WanderlustMemoir.Application.Services;

public class VisitedPlaceService : IVisitedPlaceService
{
    private readonly IMediator _mediator;

    public VisitedPlaceService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IEnumerable<VisitedPlaceDto>> GetAllVisitedPlacesAsync()
    {
        return await _mediator.Send(new GetAllVisitedPlacesQuery());
    }

    public async Task<VisitedPlaceDto?> GetVisitedPlaceByIdAsync(int id)
    {
        return await _mediator.Send(new GetVisitedPlaceByIdQuery(id));
    }

    public async Task<VisitedPlaceDto> CreateVisitedPlaceAsync(CreateVisitedPlaceDto createVisitedPlaceDto)
    {
        return await _mediator.Send(new CreateVisitedPlaceCommand(createVisitedPlaceDto));
    }

    public async Task<VisitedPlaceDto?> UpdateVisitedPlaceAsync(int id, UpdateVisitedPlaceDto updateVisitedPlaceDto)
    {
        return await _mediator.Send(new UpdateVisitedPlaceCommand(id, updateVisitedPlaceDto));
    }

    public async Task<bool> DeleteVisitedPlaceAsync(int id)
    {
        return await _mediator.Send(new DeleteVisitedPlaceCommand(id));
    }

    public async Task<VisitedPlaceDto?> UploadVisitedPlacePhotosAsync(int id, List<IFormFile> photos)
    {
        return await _mediator.Send(new UploadVisitedPlacePhotosCommand(id, photos));
    }

    public async Task<VisitedPlacePhoto?> GetPhotoByIdAsync(int photoId)
    {
        return await _mediator.Send(new GetPhotoByIdQuery(photoId));
    }

    public async Task<VisitedPlaceDto?> UpdateVisitedPlaceRatingAsync(int id, int rating)
    {
        return await _mediator.Send(new UpdateVisitedPlaceRatingCommand(id, rating));
    }

    public async Task<TravelStatsDto> GetTravelStatsAsync()
    {
        return await _mediator.Send(new GetTravelStatsQuery());
    }

    public async Task<bool> DeleteVisitedPlacePhotoAsync(int placeId, int photoId)
    {
        return await _mediator.Send(new DeleteVisitedPlacePhotoCommand(placeId, photoId));
    }
}