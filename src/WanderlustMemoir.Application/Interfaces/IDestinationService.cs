using WanderlustMemoir.Application.DTOs.Destinations;

namespace WanderlustMemoir.Application.Interfaces;

public interface IDestinationService
{
    Task<IEnumerable<DestinationDto>> GetAllDestinationsAsync();
    Task<DestinationDto?> GetDestinationByIdAsync(int id);
    Task<DestinationDto> CreateDestinationAsync(CreateDestinationDto createDestinationDto);
    Task<DestinationDto?> UpdateDestinationAsync(int id, UpdateDestinationDto updateDestinationDto);
    Task<bool> DeleteDestinationAsync(int id);
    Task<DestinationDto?> ToggleDestinationVisitedAsync(int id);
}