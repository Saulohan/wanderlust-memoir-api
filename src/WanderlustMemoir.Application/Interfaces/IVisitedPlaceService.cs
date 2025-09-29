using WanderlustMemoir.Application.DTOs.VisitedPlaces;

namespace WanderlustMemoir.Application.Interfaces;

public interface IVisitedPlaceService
{
    Task<IEnumerable<VisitedPlaceDto>> GetAllVisitedPlacesAsync();
    Task<VisitedPlaceDto?> GetVisitedPlaceByIdAsync(int id);
    Task<VisitedPlaceDto> CreateVisitedPlaceAsync(CreateVisitedPlaceDto createVisitedPlaceDto);
    Task<VisitedPlaceDto?> UpdateVisitedPlaceAsync(int id, UpdateVisitedPlaceDto updateVisitedPlaceDto);
    Task<bool> DeleteVisitedPlaceAsync(int id);
}