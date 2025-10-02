using AutoMapper;
using MediatR;
using WanderlustMemoir.Application.DTOs.Destinations;
using WanderlustMemoir.Application.Features.Destinations.Commands;
using WanderlustMemoir.Domain.Entities;
using WanderlustMemoir.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace WanderlustMemoir.Application.Features.Destinations.Handlers;

public class CreateDestinationHandler : IRequestHandler<CreateDestinationCommand, DestinationDto>
{
    private readonly IDestinationRepository _repository;
    private readonly IMapper _mapper;

    public CreateDestinationHandler(IDestinationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DestinationDto> Handle(CreateDestinationCommand request, CancellationToken cancellationToken)
    {
        var destination = _mapper.Map<Destination>(request.Destination);
        var createdDestination = await _repository.CreateAsync(destination);
        return _mapper.Map<DestinationDto>(createdDestination);
    }
}

public class UpdateDestinationHandler : IRequestHandler<UpdateDestinationCommand, DestinationDto?>
{
    private readonly IDestinationRepository _repository;
    private readonly IMapper _mapper;

    public UpdateDestinationHandler(IDestinationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DestinationDto?> Handle(UpdateDestinationCommand request, CancellationToken cancellationToken)
    {
        var existingDestination = await _repository.GetByIdAsync(request.Id);
        if (existingDestination == null)
            return null;

        _mapper.Map(request.Destination, existingDestination);
        existingDestination.UpdatedAt = DateTime.UtcNow;

        var updatedDestination = await _repository.UpdateAsync(existingDestination);
        return _mapper.Map<DestinationDto>(updatedDestination);
    }
}

public class DeleteDestinationHandler : IRequestHandler<DeleteDestinationCommand, bool>
{
    private readonly IDestinationRepository _repository;

    public DeleteDestinationHandler(IDestinationRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteDestinationCommand request, CancellationToken cancellationToken)
    {
        var destination = await _repository.GetByIdAsync(request.Id);
        if (destination == null)
            return false;

        await _repository.DeleteAsync(request.Id);
        return true;
    }
}

public class ToggleDestinationVisitedHandler : IRequestHandler<ToggleDestinationVisitedCommand, DestinationDto?>
{
    private readonly IDestinationRepository _repository;
    private readonly IVisitedPlaceRepository _visitedPlaceRepository;
    private readonly IMapper _mapper;

    public ToggleDestinationVisitedHandler(IDestinationRepository repository, IVisitedPlaceRepository visitedPlaceRepository, IMapper mapper)
    {
        _repository = repository;
        _visitedPlaceRepository = visitedPlaceRepository;
        _mapper = mapper;
    }

    public async Task<DestinationDto?> Handle(ToggleDestinationVisitedCommand request, CancellationToken cancellationToken)
    {
        var destination = await _repository.GetByIdAsync(request.Id);
        if (destination == null)
            return null;

        var wasVisited = destination.IsVisited;
        destination.IsVisited = !destination.IsVisited;
        destination.UpdatedAt = DateTime.UtcNow;

        // Se foi marcado como visitado, criar entrada em VisitedPlaces
        if (!wasVisited && destination.IsVisited)
        {
            destination.DateVisited = DateTime.UtcNow;
            await CreateVisitedPlaceFromDestination(destination);
        }
        // Se foi desmarcado como visitado, remover entrada de VisitedPlaces
        else if (wasVisited && !destination.IsVisited)
        {
            destination.DateVisited = null;
            destination.Rating = null;
            destination.Description = null;
            await RemoveVisitedPlaceFromDestination(destination);
        }

        var updatedDestination = await _repository.UpdateAsync(destination);
        return _mapper.Map<DestinationDto>(updatedDestination);
    }

    private async Task CreateVisitedPlaceFromDestination(Destination destination)
    {
        var visitedPlace = new VisitedPlace
        {
            Name = destination.Name,
            Country = destination.Country,
            VisitDate = destination.DateVisited ?? DateTime.UtcNow,
            Description = GenerateRatingBasedDescription(destination),
            Rating = destination.Rating ?? 5,
            CreatedAt = DateTime.UtcNow
        };

        await _visitedPlaceRepository.CreateAsync(visitedPlace);
    }

    private async Task RemoveVisitedPlaceFromDestination(Destination destination)
    {
        // Buscar VisitedPlace correspondente e remover
        var visitedPlaces = await _visitedPlaceRepository.GetAllAsync();
        var matchingVisitedPlace = visitedPlaces.FirstOrDefault(vp => 
            vp.Name.Equals(destination.Name, StringComparison.OrdinalIgnoreCase) && 
            vp.Country.Equals(destination.Country, StringComparison.OrdinalIgnoreCase));

        if (matchingVisitedPlace != null)
        {
            await _visitedPlaceRepository.DeleteAsync(matchingVisitedPlace.Id);
        }
    }

    /// <summary>
    /// Gera descrição personalizada baseada no rating com emojis de coração
    /// </summary>
    /// <param name="destination">Destino visitado</param>
    /// <returns>Descrição personalizada com base no rating</returns>
    private static string GenerateRatingBasedDescription(Destination destination)
    {
        var rating = destination.Rating ?? 5;
        var userDescription = !string.IsNullOrWhiteSpace(destination.Description) 
            ? $" {destination.Description}" 
            : "";

        return rating switch
        {
            5 => $"Uma experiência incrível e inesquecível em {destination.Name}!{userDescription}",
            4 => $"Uma experiência muito boa em {destination.Name}!{userDescription}",
            3 => $"Uma experiência legal em {destination.Name}!{userDescription}",
            2 => $"Uma experiência regular em {destination.Name}.{userDescription}",
            1 => $"Uma experiência decepcionante em {destination.Name}.{userDescription}",
            _ => $"Visitamos {destination.Name}!{userDescription}"
        };
    }
}

public class ToggleDestinationVisitedWithDateHandler : IRequestHandler<ToggleDestinationVisitedWithDateCommand, DestinationDto?>
{
    private readonly IDestinationRepository _repository;
    private readonly IVisitedPlaceRepository _visitedPlaceRepository;
    private readonly IMapper _mapper;

    public ToggleDestinationVisitedWithDateHandler(IDestinationRepository repository, IVisitedPlaceRepository visitedPlaceRepository, IMapper mapper)
    {
        _repository = repository;
        _visitedPlaceRepository = visitedPlaceRepository;
        _mapper = mapper;
    }

    public async Task<DestinationDto?> Handle(ToggleDestinationVisitedWithDateCommand request, CancellationToken cancellationToken)
    {
        var destination = await _repository.GetByIdAsync(request.Id);
        if (destination == null)
            return null;

        var wasVisited = destination.IsVisited;
        destination.IsVisited = !destination.IsVisited;

        if (destination.IsVisited && !string.IsNullOrEmpty(request.VisitDate))
        {
            if (DateTime.TryParse(request.VisitDate, out var visitDate))
            {
                destination.DateVisited = visitDate;
            }
            else
            {
                destination.DateVisited = DateTime.UtcNow;
            }
        }
        else if (!destination.IsVisited)
        {
            destination.DateVisited = null;
            destination.Rating = null;
            destination.Description = null;
        }

        destination.UpdatedAt = DateTime.UtcNow;

        // Se foi marcado como visitado, criar entrada em VisitedPlaces
        if (!wasVisited && destination.IsVisited)
        {
            await CreateVisitedPlaceFromDestination(destination);
        }
        // Se foi desmarcado como visitado, remover entrada de VisitedPlaces
        else if (wasVisited && !destination.IsVisited)
        {
            await RemoveVisitedPlaceFromDestination(destination);
        }

        var updatedDestination = await _repository.UpdateAsync(destination);
        return _mapper.Map<DestinationDto>(updatedDestination);
    }

    private async Task CreateVisitedPlaceFromDestination(Destination destination)
    {
        // Verificar se já existe um VisitedPlace com mesmo nome e país
        var visitedPlaces = await _visitedPlaceRepository.GetAllAsync();
        var existingVisitedPlace = visitedPlaces.FirstOrDefault(vp => 
            vp.Name.Equals(destination.Name, StringComparison.OrdinalIgnoreCase) && 
            vp.Country.Equals(destination.Country, StringComparison.OrdinalIgnoreCase));

        if (existingVisitedPlace == null)
        {
            var visitedPlace = new VisitedPlace
            {
                Name = destination.Name,
                Country = destination.Country,
                VisitDate = destination.DateVisited ?? DateTime.UtcNow,
                Description = GenerateRatingBasedDescription(destination),
                Rating = destination.Rating ?? 5,
                CreatedAt = DateTime.UtcNow
            };

            await _visitedPlaceRepository.CreateAsync(visitedPlace);
        }
    }

    private async Task RemoveVisitedPlaceFromDestination(Destination destination)
    {
        // Buscar VisitedPlace correspondente e remover
        var visitedPlaces = await _visitedPlaceRepository.GetAllAsync();
        var matchingVisitedPlace = visitedPlaces.FirstOrDefault(vp => 
            vp.Name.Equals(destination.Name, StringComparison.OrdinalIgnoreCase) && 
            vp.Country.Equals(destination.Country, StringComparison.OrdinalIgnoreCase));

        if (matchingVisitedPlace != null)
        {
            await _visitedPlaceRepository.DeleteAsync(matchingVisitedPlace.Id);
        }
    }

    /// <summary>
    /// Gera descrição personalizada baseada no rating com emojis de coração
    /// </summary>
    /// <param name="destination">Destino visitado</param>
    /// <returns>Descrição personalizada com base no rating</returns>
    private static string GenerateRatingBasedDescription(Destination destination)
    {
        var rating = destination.Rating ?? 5;
        var userDescription = !string.IsNullOrWhiteSpace(destination.Description) 
            ? $" {destination.Description}" 
            : "";

        return rating switch
        {
            5 => $"Uma experiência incrível e inesquecível em {destination.Name}!{userDescription}",
            4 => $"Uma experiência muito boa em {destination.Name}!{userDescription}",
            3 => $"Uma experiência legal em {destination.Name}!{userDescription}",
            2 => $"Uma experiência regular em {destination.Name}.{userDescription}",
            1 => $"Uma experiência decepcionante em {destination.Name}.{userDescription}",
            _ => $"Visitamos {destination.Name}!{userDescription}"
        };
    }
}

public class UpdateDestinationRatingHandler : IRequestHandler<UpdateDestinationRatingCommand, DestinationDto?>
{
    private readonly IDestinationRepository _repository;
    private readonly IVisitedPlaceRepository _visitedPlaceRepository;
    private readonly IMapper _mapper;

    public UpdateDestinationRatingHandler(IDestinationRepository repository, IVisitedPlaceRepository visitedPlaceRepository, IMapper mapper)
    {
        _repository = repository;
        _visitedPlaceRepository = visitedPlaceRepository;
        _mapper = mapper;
    }

    public async Task<DestinationDto?> Handle(UpdateDestinationRatingCommand request, CancellationToken cancellationToken)
    {
        var destination = await _repository.GetByIdAsync(request.Id);
        if (destination == null)
            return null;

        destination.Rating = request.Rating;
        destination.UpdatedAt = DateTime.UtcNow;

        // Se o destino está visitado, atualizar também o rating no VisitedPlace correspondente
        if (destination.IsVisited)
        {
            await UpdateVisitedPlaceRating(destination, request.Rating);
        }

        var updatedDestination = await _repository.UpdateAsync(destination);
        return _mapper.Map<DestinationDto>(updatedDestination);
    }

    private async Task UpdateVisitedPlaceRating(Destination destination, int rating)
    {
        var visitedPlaces = await _visitedPlaceRepository.GetAllAsync();
        var matchingVisitedPlace = visitedPlaces.FirstOrDefault(vp => 
            vp.Name.Equals(destination.Name, StringComparison.OrdinalIgnoreCase) && 
            vp.Country.Equals(destination.Country, StringComparison.OrdinalIgnoreCase));

        if (matchingVisitedPlace != null)
        {
            matchingVisitedPlace.Rating = rating;
            // Atualizar também a descrição com o novo rating
            matchingVisitedPlace.Description = GenerateRatingBasedDescriptionForUpdate(destination, rating);
            matchingVisitedPlace.UpdatedAt = DateTime.UtcNow;
            await _visitedPlaceRepository.UpdateAsync(matchingVisitedPlace);
        }
    }

    /// <summary>
    /// Gera descrição personalizada para atualização de rating
    /// </summary>
    /// <param name="destination">Destino</param>
    /// <param name="newRating">Novo rating</param>
    /// <returns>Descrição atualizada</returns>
    private static string GenerateRatingBasedDescriptionForUpdate(Destination destination, int newRating)
    {
        var userDescription = !string.IsNullOrWhiteSpace(destination.Description) 
            ? $" {destination.Description}" 
            : "";

        return newRating switch
        {
            5 => $"Uma experiência incrível e inesquecível em {destination.Name}!{userDescription}",
            4 => $"Uma experiência muito boa em {destination.Name}!{userDescription}",
            3 => $"Uma experiência legal em {destination.Name}!{userDescription}",
            2 => $"Uma experiência regular em {destination.Name}.{userDescription}",
            1 => $"Uma experiência decepcionante em {destination.Name}.{userDescription}",
            _ => $"Visitamos {destination.Name}!{userDescription}"
        };
    }
}

public class UpdateDestinationPriorityHandler : IRequestHandler<UpdateDestinationPriorityCommand, DestinationDto?>
{
    private readonly IDestinationRepository _repository;
    private readonly IMapper _mapper;

    public UpdateDestinationPriorityHandler(IDestinationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DestinationDto?> Handle(UpdateDestinationPriorityCommand request, CancellationToken cancellationToken)
    {
        var destination = await _repository.GetByIdAsync(request.Id);
        if (destination == null)
            return null;

        // Validar prioridade
        var validPriorities = new[] { "high", "medium", "low" };
        if (!validPriorities.Contains(request.Priority.ToLower()))
        {
            throw new ArgumentException($"Invalid priority: {request.Priority}. Valid values are: high, medium, low");
        }

        // Converter string para enum
        destination.Priority = request.Priority.ToLower() switch
        {
            "high" => Priority.High,
            "medium" => Priority.Medium,
            "low" => Priority.Low,
            _ => Priority.Medium
        };

        destination.UpdatedAt = DateTime.UtcNow;

        var updatedDestination = await _repository.UpdateAsync(destination);
        return _mapper.Map<DestinationDto>(updatedDestination);
    }
}

public class UploadDestinationPhotosHandler : IRequestHandler<UploadDestinationPhotosCommand, DestinationDto?>
{
    private readonly IDestinationRepository _repository;
    private readonly IMapper _mapper;

    public UploadDestinationPhotosHandler(IDestinationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DestinationDto?> Handle(UploadDestinationPhotosCommand request, CancellationToken cancellationToken)
    {
        var destination = await _repository.GetByIdAsync(request.Id);
        if (destination == null)
            return null;

        var photoEntities = new List<DestinationPhoto>();
        
        foreach (var photo in request.Photos)
        {
            if (photo.Length > 0)
            {
                // Ler dados da imagem para byte array (BLOB)
                using var memoryStream = new MemoryStream();
                await photo.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();
                
                var fileName = $"{Guid.NewGuid()}_{photo.FileName}";
                var filePath = $"/api/destinations/{request.Id}/photos/";
                
                var photoEntity = new DestinationPhoto
                {
                    FileName = fileName,
                    FilePath = filePath,
                    Caption = photo.FileName,
                    ImageData = imageData, // Armazenar como BLOB
                    ContentType = photo.ContentType,
                    FileSize = photo.Length,
                    DestinationId = request.Id,
                    CreatedAt = DateTime.UtcNow
                };
                
                photoEntities.Add(photoEntity);
            }
        }

        destination.Photos.AddRange(photoEntities);
        destination.UpdatedAt = DateTime.UtcNow;

        var updatedDestination = await _repository.UpdateAsync(destination);
        return _mapper.Map<DestinationDto>(updatedDestination);
    }
}