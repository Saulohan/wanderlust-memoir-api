using Microsoft.AspNetCore.Mvc;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;
using WanderlustMemoir.Application.DTOs.TravelStats;
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
    /// Gets travel statistics
    /// </summary>
    /// <returns>Travel statistics including dream destinations, visited places, shared photos, and explored countries</returns>
    [HttpGet("stats")]
    public async Task<ActionResult<TravelStatsDto>> GetTravelStats()
    {
        try
        {
            var stats = await _visitedPlaceService.GetTravelStatsAsync();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all visited places
    /// </summary>
    /// <returns>List of visited places</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VisitedPlaceDto>>> GetAllVisitedPlaces()
    {
        try
        {
            var visitedPlaces = await _visitedPlaceService.GetAllVisitedPlacesAsync();
            return Ok(visitedPlaces);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a visited place by id
    /// </summary>
    /// <param name="id">Visited place id</param>
    /// <returns>Visited place</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<VisitedPlaceDto>> GetVisitedPlaceById(int id)
    {
        try
        {
            var visitedPlace = await _visitedPlaceService.GetVisitedPlaceByIdAsync(id);
            
            if (visitedPlace == null)
                return NotFound($"Visited place with id {id} not found");
                
            return Ok(visitedPlace);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new visited place
    /// </summary>
    /// <param name="createVisitedPlaceDto">Visited place data</param>
    /// <returns>Created visited place</returns>
    [HttpPost]
    public async Task<ActionResult<VisitedPlaceDto>> CreateVisitedPlace([FromBody] CreateVisitedPlaceDto createVisitedPlaceDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var visitedPlace = await _visitedPlaceService.CreateVisitedPlaceAsync(createVisitedPlaceDto);
            return CreatedAtAction(nameof(GetVisitedPlaceById), new { id = visitedPlace.Id }, visitedPlace);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
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
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var visitedPlace = await _visitedPlaceService.UpdateVisitedPlaceAsync(id, updateVisitedPlaceDto);
            
            if (visitedPlace == null)
                return NotFound($"Visited place with id {id} not found");
                
            return Ok(visitedPlace);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a visited place
    /// </summary>
    /// <param name="id">Visited place id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteVisitedPlace(int id)
    {
        try
        {
            var success = await _visitedPlaceService.DeleteVisitedPlaceAsync(id);
            
            if (!success)
                return NotFound($"Visited place with id {id} not found");
                
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates the rating of a visited place
    /// </summary>
    /// <param name="id">Visited place id</param>
    /// <param name="ratingDto">Rating data</param>
    /// <returns>Updated visited place</returns>
    [HttpPatch("{id}/rating")]
    public async Task<ActionResult<VisitedPlaceDto>> UpdateVisitedPlaceRating(int id, [FromBody] UpdateVisitedPlaceRatingDto ratingDto)
    {
        try
        {
            if (ratingDto.Rating < 1 || ratingDto.Rating > 5)
                return BadRequest("Rating must be between 1 and 5");

            var visitedPlace = await _visitedPlaceService.UpdateVisitedPlaceRatingAsync(id, ratingDto.Rating);
            
            if (visitedPlace == null)
                return NotFound($"Visited place with id {id} not found");
                
            return Ok(visitedPlace);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Uploads photos for a visited place (saves as BLOB in database)
    /// </summary>
    /// <param name="id">Visited place id</param>
    /// <param name="photos">Photos to upload</param>
    /// <returns>Updated visited place with photos</returns>
    [HttpPost("{id}/photos")]
    public async Task<ActionResult<VisitedPlaceDto>> UploadVisitedPlacePhotos(int id, List<IFormFile> photos)
    {
        try
        {
            // 1. VALIDAÇÕES BÁSICAS
            if (photos == null || !photos.Any())
                return BadRequest("No photos provided");

            // 2. VERIFICAR SE O VISITED PLACE EXISTE PRIMEIRO
            var existingVisitedPlace = await _visitedPlaceService.GetVisitedPlaceByIdAsync(id);
            if (existingVisitedPlace == null)
                return NotFound($"Visited place with id {id} not found");

            // 3. VALIDAR FOTOS
            foreach (var photo in photos)
            {
                if (photo.Length > 10 * 1024 * 1024) // 10MB
                    return BadRequest($"Photo {photo.FileName} is too large. Maximum size is 10MB.");
                
                if (!IsValidImageType(photo.ContentType))
                    return BadRequest($"Photo {photo.FileName} has invalid format. Only JPEG, PNG, GIF and WebP are allowed.");
            }

            // 4. FAZER UPLOAD DAS FOTOS (agora sabemos que o VisitedPlace existe)
            var updatedVisitedPlace = await _visitedPlaceService.UploadVisitedPlacePhotosAsync(id, photos);
            
            // 5. COMO JÁ VERIFICAMOS QUE EXISTE, ISSO NÃO DEVERIA SER NULL
            if (updatedVisitedPlace == null)
            {
                // Log do erro inesperado
                return StatusCode(500, "Unexpected error occurred during photo upload");
            }
                
            return Ok(updatedVisitedPlace);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error uploading photos: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a specific photo from a visited place
    /// </summary>
    /// <param name="id">Visited place id</param>
    /// <param name="photoId">Photo id to delete</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}/photos/{photoId}")]
    public async Task<ActionResult> DeleteVisitedPlacePhoto(int id, int photoId)
    {
        try
        {
            var success = await _visitedPlaceService.DeleteVisitedPlacePhotoAsync(id, photoId);
            
            if (!success)
                return NotFound($"Photo with id {photoId} not found for visited place {id}");
                
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error deleting photo: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a photo by id from visited place (serves BLOB data)
    /// </summary>
    /// <param name="id">Visited place id</param>
    /// <param name="photoId">Photo id</param>
    /// <returns>Photo file as BLOB</returns>
    [HttpGet("{id}/photos/{photoId}")]
    public async Task<ActionResult> GetVisitedPlacePhoto(int id, int photoId)
    {
        try
        {
            var photo = await _visitedPlaceService.GetPhotoByIdAsync(photoId);
            
            if (photo == null)
                return NotFound($"Photo with id {photoId} not found");

            // Verificar se a foto pertence ao lugar visitado
            if (photo.VisitedPlaceId != id)
                return NotFound($"Photo with id {photoId} not found for visited place {id}");
                
            return File(photo.ImageData, photo.ContentType, photo.FileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving photo: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a thumbnail photo by id from visited place (serves BLOB data)
    /// </summary>
    /// <param name="id">Visited place id</param>
    /// <param name="photoId">Photo id</param>
    /// <returns>Thumbnail photo file</returns>
    [HttpGet("{id}/photos/{photoId}/thumbnail")]
    public async Task<ActionResult> GetVisitedPlacePhotoThumbnail(int id, int photoId)
    {
        try
        {
            var photo = await _visitedPlaceService.GetPhotoByIdAsync(photoId);
            
            if (photo == null)
                return NotFound($"Photo with id {photoId} not found");

            // Verificar se a foto pertence ao lugar visitado
            if (photo.VisitedPlaceId != id)
                return NotFound($"Photo with id {photoId} not found for visited place {id}");

            // Por enquanto retorna a imagem original
            // TODO: Implementar redimensionamento para thumbnail
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