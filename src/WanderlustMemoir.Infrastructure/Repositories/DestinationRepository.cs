using Microsoft.EntityFrameworkCore;
using WanderlustMemoir.Domain.Entities;
using WanderlustMemoir.Domain.Repositories;
using WanderlustMemoir.Infrastructure.Data;

namespace WanderlustMemoir.Infrastructure.Repositories;

public class DestinationRepository : IDestinationRepository
{
    private readonly WanderlustMemoirDbContext _context;

    public DestinationRepository(WanderlustMemoirDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Destination>> GetAllAsync()
    {
        return await _context.Destinations.ToListAsync();
    }

    public async Task<Destination?> GetByIdAsync(int id)
    {
        return await _context.Destinations.FindAsync(id);
    }

    public async Task<Destination> CreateAsync(Destination destination)
    {
        destination.CreatedAt = DateTime.UtcNow;
        _context.Destinations.Add(destination);
        await _context.SaveChangesAsync();
        return destination;
    }

    public async Task<Destination> UpdateAsync(Destination destination)
    {
        _context.Entry(destination).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return destination;
    }

    public async Task DeleteAsync(int id)
    {
        var destination = await _context.Destinations.FindAsync(id);
        if (destination != null)
        {
            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();
        }
    }
}