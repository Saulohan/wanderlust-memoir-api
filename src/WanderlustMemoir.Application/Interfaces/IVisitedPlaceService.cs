using WanderlustMemoir.Application.DTOs.VisitedPlaces;
using WanderlustMemoir.Application.DTOs.TravelStats;
using WanderlustMemoir.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace WanderlustMemoir.Application.Interfaces;

public interface IVisitedPlaceService
{
    Task<IEnumerable<VisitedPlaceDto>> GetAllVisitedPlacesAsync();
    Task<VisitedPlaceDto?> GetVisitedPlaceByIdAsync(int id);
    Task<VisitedPlaceDto> CreateVisitedPlaceAsync(CreateVisitedPlaceDto createVisitedPlaceDto);
    Task<VisitedPlaceDto?> UpdateVisitedPlaceAsync(int id, UpdateVisitedPlaceDto updateVisitedPlaceDto);
    Task<bool> DeleteVisitedPlaceAsync(int id);
    Task<VisitedPlaceDto?> UploadVisitedPlacePhotosAsync(int id, List<IFormFile> photos);
    Task<VisitedPlacePhoto?> GetPhotoByIdAsync(int photoId);
    Task<VisitedPlaceDto?> UpdateVisitedPlaceRatingAsync(int id, int rating);
    Task<TravelStatsDto> GetTravelStatsAsync();
    Task<bool> DeleteVisitedPlacePhotoAsync(int placeId, int photoId);
}