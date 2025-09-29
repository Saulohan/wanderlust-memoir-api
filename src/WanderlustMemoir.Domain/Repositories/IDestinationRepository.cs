using WanderlustMemoir.Domain.Entities;

namespace WanderlustMemoir.Domain.Repositories;

public interface IDestinationRepository
{
    Task<IEnumerable<Destination>> GetAllAsync();
    Task<Destination?> GetByIdAsync(int id);
    Task<Destination> CreateAsync(Destination destination);
    Task<Destination> UpdateAsync(Destination destination);
    Task DeleteAsync(int id);
}