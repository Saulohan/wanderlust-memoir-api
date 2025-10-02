using WanderlustMemoir.Application.DTOs.Destinations;
using WanderlustMemoir.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace WanderlustMemoir.Application.Interfaces;

public interface IDestinationService
{
    Task<IEnumerable<DestinationDto>> GetAllDestinationsAsync();
    Task<DestinationDto?> GetDestinationByIdAsync(int id);
    Task<DestinationDto> CreateDestinationAsync(CreateDestinationDto createDestinationDto);
    Task<DestinationDto?> UpdateDestinationAsync(int id, UpdateDestinationDto updateDestinationDto);
    Task<bool> DeleteDestinationAsync(int id);
    Task<DestinationDto?> ToggleDestinationVisitedAsync(int id);
    Task<DestinationDto?> ToggleDestinationVisitedAsync(int id, string? visitDate);
    Task<DestinationDto?> UpdateDestinationRatingAsync(int id, int rating);
    Task<DestinationDto?> UpdateDestinationPriorityAsync(int id, string priority);
    Task<DestinationDto?> UploadDestinationPhotosAsync(int id, List<IFormFile> photos);
    Task<DestinationPhoto?> GetDestinationPhotoByIdAsync(int photoId);
}