using WanderlustMemoir.Domain.Entities;

namespace WanderlustMemoir.Domain.Repositories;

public interface IVisitedPlaceRepository
{
    Task<IEnumerable<VisitedPlace>> GetAllAsync();
    Task<VisitedPlace?> GetByIdAsync(int id);
    Task<VisitedPlace> CreateAsync(VisitedPlace visitedPlace);
    Task<VisitedPlace> UpdateAsync(VisitedPlace visitedPlace);
    Task DeleteAsync(int id);
}