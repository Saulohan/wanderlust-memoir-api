using Microsoft.EntityFrameworkCore;
using WanderlustMemoir.Domain.Entities;
using WanderlustMemoir.Domain.Repositories;
using WanderlustMemoir.Infrastructure.Data;

namespace WanderlustMemoir.Infrastructure.Repositories;

public class VisitedPlaceRepository : IVisitedPlaceRepository
{
    private readonly WanderlustMemoirDbContext _context;

    public VisitedPlaceRepository(WanderlustMemoirDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VisitedPlace>> GetAllAsync()
    {
        return await _context.VisitedPlaces
            .Include(vp => vp.Photos)
            .ToListAsync();
    }

    public async Task<VisitedPlace?> GetByIdAsync(int id)
    {
        return await _context.VisitedPlaces
            .Include(vp => vp.Photos)
            .FirstOrDefaultAsync(vp => vp.Id == id);
    }

    public async Task<VisitedPlace> CreateAsync(VisitedPlace visitedPlace)
    {
        visitedPlace.CreatedAt = DateTime.UtcNow;
        _context.VisitedPlaces.Add(visitedPlace);
        await _context.SaveChangesAsync();
        return visitedPlace;
    }

    public async Task<VisitedPlace> UpdateAsync(VisitedPlace visitedPlace)
    {
        _context.Entry(visitedPlace).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return visitedPlace;
    }

    public async Task DeleteAsync(int id)
    {
        var visitedPlace = await _context.VisitedPlaces.FindAsync(id);
        if (visitedPlace != null)
        {
            _context.VisitedPlaces.Remove(visitedPlace);
            await _context.SaveChangesAsync();
        }
    }
}