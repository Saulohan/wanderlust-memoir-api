using WanderlustMemoir.Domain.Entities;

namespace WanderlustMemoir.Domain.Repositories;

public interface IPhotoRepository
{
    Task<IEnumerable<Photo>> GetByVisitedPlaceIdAsync(int visitedPlaceId);
    Task<Photo?> GetByIdAsync(int id);
    Task<Photo> CreateAsync(Photo photo);
    Task DeleteAsync(int id);
    Task<int> GetTotalCountAsync();
}