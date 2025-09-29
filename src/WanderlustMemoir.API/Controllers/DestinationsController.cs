using Microsoft.AspNetCore.Mvc;
using WanderlustMemoir.Application.DTOs.Destinations;
using WanderlustMemoir.Application.Interfaces;

namespace WanderlustMemoir.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DestinationsController : ControllerBase
{
    private readonly IDestinationService _destinationService;

    public DestinationsController(IDestinationService destinationService)
    {
        _destinationService = destinationService;
    }

    /// <summary>
    /// Gets all destinations
    /// </summary>
    /// <returns>List of destinations</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DestinationDto>>> GetAllDestinations()
    {
        var destinations = await _destinationService.GetAllDestinationsAsync();
        return Ok(destinations);
    }

    /// <summary>
    /// Gets a destination by id
    /// </summary>
    /// <param name="id">Destination id</param>
    /// <returns>Destination</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<DestinationDto>> GetDestinationById(int id)
    {
        var destination = await _destinationService.GetDestinationByIdAsync(id);
        
        if (destination == null)
            return NotFound($"Destination with id {id} not found");
            
        return Ok(destination);
    }

    /// <summary>
    /// Creates a new destination
    /// </summary>
    /// <param name="createDestinationDto">Destination data</param>
    /// <returns>Created destination</returns>
    [HttpPost]
    public async Task<ActionResult<DestinationDto>> CreateDestination([FromBody] CreateDestinationDto createDestinationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var destination = await _destinationService.CreateDestinationAsync(createDestinationDto);
        return CreatedAtAction(nameof(GetDestinationById), new { id = destination.Id }, destination);
    }

    /// <summary>
    /// Updates an existing destination
    /// </summary>
    /// <param name="id">Destination id</param>
    /// <param name="updateDestinationDto">Updated destination data</param>
    /// <returns>Updated destination</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<DestinationDto>> UpdateDestination(int id, [FromBody] UpdateDestinationDto updateDestinationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var destination = await _destinationService.UpdateDestinationAsync(id, updateDestinationDto);
        
        if (destination == null)
            return NotFound($"Destination with id {id} not found");
            
        return Ok(destination);
    }

    /// <summary>
    /// Deletes a destination
    /// </summary>
    /// <param name="id">Destination id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDestination(int id)
    {
        var success = await _destinationService.DeleteDestinationAsync(id);
        
        if (!success)
            return NotFound($"Destination with id {id} not found");
            
        return NoContent();
    }

    /// <summary>
    /// Toggles the visited status of a destination
    /// </summary>
    /// <param name="id">Destination id</param>
    /// <returns>Updated destination</returns>
    [HttpPatch("{id}/toggle-visited")]
    public async Task<ActionResult<DestinationDto>> ToggleDestinationVisited(int id)
    {
        var destination = await _destinationService.ToggleDestinationVisitedAsync(id);
        
        if (destination == null)
            return NotFound($"Destination with id {id} not found");
            
        return Ok(destination);
    }
}