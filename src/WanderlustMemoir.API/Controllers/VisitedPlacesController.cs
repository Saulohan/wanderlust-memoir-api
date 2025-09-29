using Microsoft.AspNetCore.Mvc;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;
using WanderlustMemoir.Application.Interfaces;

namespace WanderlustMemoir.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VisitedPlacesController : ControllerBase
{
    private readonly IVisitedPlaceService _visitedPlaceService;

    public VisitedPlacesController(IVisitedPlaceService visitedPlaceService)
    {
        _visitedPlaceService = visitedPlaceService;
    }

    /// <summary>
    /// Gets all visited places
    /// </summary>
    /// <returns>List of visited places</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VisitedPlaceDto>>> GetAllVisitedPlaces()
    {
        var visitedPlaces = await _visitedPlaceService.GetAllVisitedPlacesAsync();
        return Ok(visitedPlaces);
    }

    /// <summary>
    /// Gets a visited place by id
    /// </summary>
    /// <param name="id">Visited place id</param>
    /// <returns>Visited place</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<VisitedPlaceDto>> GetVisitedPlaceById(int id)
    {
        var visitedPlace = await _visitedPlaceService.GetVisitedPlaceByIdAsync(id);
        
        if (visitedPlace == null)
            return NotFound($"Visited place with id {id} not found");
            
        return Ok(visitedPlace);
    }

    /// <summary>
    /// Creates a new visited place
    /// </summary>
    /// <param name="createVisitedPlaceDto">Visited place data</param>
    /// <returns>Created visited place</returns>
    [HttpPost]
    public async Task<ActionResult<VisitedPlaceDto>> CreateVisitedPlace([FromBody] CreateVisitedPlaceDto createVisitedPlaceDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var visitedPlace = await _visitedPlaceService.CreateVisitedPlaceAsync(createVisitedPlaceDto);
        return CreatedAtAction(nameof(GetVisitedPlaceById), new { id = visitedPlace.Id }, visitedPlace);
    }

    /// <summary>
    /// Updates an existing visited place
    /// </summary>
    /// <param name="id">Visited place id</param>
    /// <param name="updateVisitedPlaceDto">Updated visited place data</param>
    /// <returns>Updated visited place</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<VisitedPlaceDto>> UpdateVisitedPlace(int id, [FromBody] UpdateVisitedPlaceDto updateVisitedPlaceDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var visitedPlace = await _visitedPlaceService.UpdateVisitedPlaceAsync(id, updateVisitedPlaceDto);
        
        if (visitedPlace == null)
            return NotFound($"Visited place with id {id} not found");
            
        return Ok(visitedPlace);
    }

    /// <summary>
    /// Deletes a visited place
    /// </summary>
    /// <param name="id">Visited place id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteVisitedPlace(int id)
    {
        var success = await _visitedPlaceService.DeleteVisitedPlaceAsync(id);
        
        if (!success)
            return NotFound($"Visited place with id {id} not found");
            
        return NoContent();
    }
}