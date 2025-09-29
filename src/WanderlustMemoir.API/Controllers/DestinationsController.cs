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
        try
        {
            var destinations = await _destinationService.GetAllDestinationsAsync();
            return Ok(destinations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a destination by id
    /// </summary>
    /// <param name="id">Destination id</param>
    /// <returns>Destination</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<DestinationDto>> GetDestinationById(int id)
    {
        try
        {
            var destination = await _destinationService.GetDestinationByIdAsync(id);
            
            if (destination == null)
                return NotFound($"Destination with id {id} not found");
                
            return Ok(destination);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new destination
    /// </summary>
    /// <param name="createDestinationDto">Destination data</param>
    /// <returns>Created destination</returns>
    [HttpPost]
    public async Task<ActionResult<DestinationDto>> CreateDestination([FromBody] CreateDestinationDto createDestinationDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var destination = await _destinationService.CreateDestinationAsync(createDestinationDto);
            return CreatedAtAction(nameof(GetDestinationById), new { id = destination.Id }, destination);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
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
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var destination = await _destinationService.UpdateDestinationAsync(id, updateDestinationDto);
            
            if (destination == null)
                return NotFound($"Destination with id {id} not found");
                
            return Ok(destination);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a destination
    /// </summary>
    /// <param name="id">Destination id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDestination(int id)
    {
        try
        {
            var success = await _destinationService.DeleteDestinationAsync(id);
            
            if (!success)
                return NotFound($"Destination with id {id} not found");
                
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Toggles the visited status of a destination
    /// </summary>
    /// <param name="id">Destination id</param>
    /// <param name="toggleDto">Toggle data with visit date</param>
    /// <returns>Updated destination</returns>
    [HttpPatch("{id}/toggle-visited")]
    public async Task<ActionResult<DestinationDto>> ToggleDestinationVisited(int id, [FromBody] ToggleVisitedDto toggleDto)
    {
        try
        {
            var destination = await _destinationService.ToggleDestinationVisitedAsync(id, toggleDto.VisitDate);
            
            if (destination == null)
                return NotFound($"Destination with id {id} not found");
                
            return Ok(destination);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates the rating of a destination
    /// </summary>
    /// <param name="id">Destination id</param>
    /// <param name="ratingDto">Rating data</param>
    /// <returns>Updated destination</returns>
    [HttpPatch("{id}/rating")]
    public async Task<ActionResult<DestinationDto>> UpdateDestinationRating(int id, [FromBody] UpdateRatingDto ratingDto)
    {
        try
        {
            if (ratingDto.Rating < 1 || ratingDto.Rating > 5)
                return BadRequest("Rating must be between 1 and 5");

            var destination = await _destinationService.UpdateDestinationRatingAsync(id, ratingDto.Rating);
            
            if (destination == null)
                return NotFound($"Destination with id {id} not found");
                
            return Ok(destination);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Uploads photos for a destination (saves as BLOB in database)
    /// </summary>
    /// <param name="id">Destination id</param>
    /// <param name="photos">Photos to upload</param>
    /// <returns>Updated destination with photos</returns>
    [HttpPost("{id}/photos")]
    public async Task<ActionResult<DestinationDto>> UploadDestinationPhotos(int id, List<IFormFile> photos)
    {
        try
        {
            if (photos == null || !photos.Any())
                return BadRequest("No photos provided");

            // Verificar se o destino existe primeiro
            var existingDestination = await _destinationService.GetDestinationByIdAsync(id);
            if (existingDestination == null)
                return NotFound($"Destination with id {id} not found");

            // Validar fotos
            foreach (var photo in photos)
            {
                if (photo.Length > 10 * 1024 * 1024) // 10MB
                    return BadRequest($"Photo {photo.FileName} is too large. Maximum size is 10MB.");
                
                if (!IsValidImageType(photo.ContentType))
                    return BadRequest($"Photo {photo.FileName} has invalid format. Only JPEG, PNG, GIF and WebP are allowed.");
            }

            var destination = await _destinationService.UploadDestinationPhotosAsync(id, photos);
            
            if (destination == null)
            {
                return StatusCode(500, "Unexpected error occurred during photo upload");
            }
                
            return Ok(destination);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error uploading photos: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a photo by id from destination (serves BLOB data)
    /// </summary>
    /// <param name="id">Destination id</param>
    /// <param name="photoId">Photo id</param>
    /// <returns>Photo file as BLOB</returns>
    [HttpGet("{id}/photos/{photoId}")]
    public async Task<ActionResult> GetDestinationPhoto(int id, int photoId)
    {
        try
        {
            var photo = await _destinationService.GetDestinationPhotoByIdAsync(photoId);
            
            if (photo == null)
                return NotFound($"Photo with id {photoId} not found");

            // Verificar se a foto pertence ao destino
            if (photo.DestinationId != id)
                return NotFound($"Photo with id {photoId} not found for destination {id}");
                
            return File(photo.ImageData, photo.ContentType, photo.FileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving photo: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a thumbnail photo by id from destination (serves BLOB data)
    /// </summary>
    /// <param name="id">Destination id</param>
    /// <param name="photoId">Photo id</param>
    /// <returns>Thumbnail photo file</returns>
    [HttpGet("{id}/photos/{photoId}/thumbnail")]
    public async Task<ActionResult> GetDestinationPhotoThumbnail(int id, int photoId)
    {
        try
        {
            var photo = await _destinationService.GetDestinationPhotoByIdAsync(photoId);
            
            if (photo == null)
                return NotFound($"Photo with id {photoId} not found");

            // Verificar se a foto pertence ao destino
            if (photo.DestinationId != id)
                return NotFound($"Photo with id {photoId} not found for destination {id}");

            return File(photo.ImageData, photo.ContentType, $"thumb_{photo.FileName}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving thumbnail: {ex.Message}");
        }
    }

    /// <summary>
    /// Validates if the content type is a valid image format
    /// </summary>
    /// <param name="contentType">Content type to validate</param>
    /// <returns>True if valid image format</returns>
    private static bool IsValidImageType(string contentType)
    {
        var validTypes = new[]
        {
            "image/jpeg",
            "image/jpg", 
            "image/png",
            "image/gif",
            "image/webp"
        };

        return validTypes.Contains(contentType.ToLower());
    }
}