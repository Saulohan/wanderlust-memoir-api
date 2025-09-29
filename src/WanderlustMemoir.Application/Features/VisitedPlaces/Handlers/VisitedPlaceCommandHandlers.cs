using AutoMapper;
using MediatR;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;
using WanderlustMemoir.Application.Features.VisitedPlaces.Commands;
using WanderlustMemoir.Domain.Entities;
using WanderlustMemoir.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace WanderlustMemoir.Application.Features.VisitedPlaces.Handlers;

public class CreateVisitedPlaceHandler : IRequestHandler<CreateVisitedPlaceCommand, VisitedPlaceDto>
{
    private readonly IVisitedPlaceRepository _repository;
    private readonly IMapper _mapper;

    public CreateVisitedPlaceHandler(IVisitedPlaceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<VisitedPlaceDto> Handle(CreateVisitedPlaceCommand request, CancellationToken cancellationToken)
    {
        var visitedPlace = _mapper.Map<VisitedPlace>(request.VisitedPlace);
        
        // Se a descrição estiver vazia, gerar baseada no rating
        if (string.IsNullOrWhiteSpace(visitedPlace.Description))
        {
            visitedPlace.Description = GenerateRatingBasedDescription(visitedPlace);
        }
        
        var createdVisitedPlace = await _repository.CreateAsync(visitedPlace);
        return _mapper.Map<VisitedPlaceDto>(createdVisitedPlace);
    }

    /// <summary>
    /// Gera descrição personalizada baseada no rating com emojis de coração
    /// </summary>
    /// <param name="visitedPlace">Lugar visitado</param>
    /// <returns>Descrição personalizada com base no rating</returns>
    private static string GenerateRatingBasedDescription(VisitedPlace visitedPlace)
    {
        var rating = visitedPlace.Rating;

        return rating switch
        {
            5 => $"Uma experiência incrível e inesquecível em {visitedPlace.Name}!",
            4 => $"Uma experiência muito boa em {visitedPlace.Name}!",
            3 => $"Uma experiência legal em {visitedPlace.Name}!",
            2 => $"Uma experiência regular em {visitedPlace.Name}.",
            1 => $"Uma experiência decepcionante em {visitedPlace.Name}.",
            _ => $"Visitamos {visitedPlace.Name}!"
        };
    }
}

public class UpdateVisitedPlaceHandler : IRequestHandler<UpdateVisitedPlaceCommand, VisitedPlaceDto?>
{
    private readonly IVisitedPlaceRepository _repository;
    private readonly IMapper _mapper;

    public UpdateVisitedPlaceHandler(IVisitedPlaceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<VisitedPlaceDto?> Handle(UpdateVisitedPlaceCommand request, CancellationToken cancellationToken)
    {
        var existingVisitedPlace = await _repository.GetByIdAsync(request.Id);
        if (existingVisitedPlace == null)
            return null;

        _mapper.Map(request.VisitedPlace, existingVisitedPlace);
        existingVisitedPlace.UpdatedAt = DateTime.UtcNow;

        var updatedVisitedPlace = await _repository.UpdateAsync(existingVisitedPlace);
        return _mapper.Map<VisitedPlaceDto>(updatedVisitedPlace);
    }
}

public class DeleteVisitedPlaceHandler : IRequestHandler<DeleteVisitedPlaceCommand, bool>
{
    private readonly IVisitedPlaceRepository _repository;

    public DeleteVisitedPlaceHandler(IVisitedPlaceRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteVisitedPlaceCommand request, CancellationToken cancellationToken)
    {
        var visitedPlace = await _repository.GetByIdAsync(request.Id);
        if (visitedPlace == null)
            return false;

        await _repository.DeleteAsync(request.Id);
        return true;
    }
}

public class DeleteVisitedPlacePhotoHandler : IRequestHandler<DeleteVisitedPlacePhotoCommand, bool>
{
    private readonly IVisitedPlaceRepository _repository;

    public DeleteVisitedPlacePhotoHandler(IVisitedPlaceRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteVisitedPlacePhotoCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeletePhotoAsync(request.PlaceId, request.PhotoId);
    }
}

public class UpdateVisitedPlaceRatingHandler : IRequestHandler<UpdateVisitedPlaceRatingCommand, VisitedPlaceDto?>
{
    private readonly IVisitedPlaceRepository _repository;
    private readonly IMapper _mapper;

    public UpdateVisitedPlaceRatingHandler(IVisitedPlaceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<VisitedPlaceDto?> Handle(UpdateVisitedPlaceRatingCommand request, CancellationToken cancellationToken)
    {
        var visitedPlace = await _repository.GetByIdAsync(request.Id);
        if (visitedPlace == null)
            return null;

        var oldDescription = visitedPlace.Description;
        visitedPlace.Rating = request.Rating;
        visitedPlace.UpdatedAt = DateTime.UtcNow;

        // Atualizar a descrição com base no novo rating
        visitedPlace.Description = GenerateRatingBasedDescription(visitedPlace, oldDescription);

        var updatedVisitedPlace = await _repository.UpdateAsync(visitedPlace);
        return _mapper.Map<VisitedPlaceDto>(updatedVisitedPlace);
    }

    /// <summary>
    /// Gera descrição personalizada baseada no rating com emojis de coração
    /// </summary>
    /// <param name="visitedPlace">Lugar visitado</param>
    /// <param name="originalDescription">Descrição original para extrair comentário do usuário</param>
    /// <returns>Descrição personalizada com base no rating</returns>
    private static string GenerateRatingBasedDescription(VisitedPlace visitedPlace, string originalDescription)
    {
        var rating = visitedPlace.Rating;
        
        // Tentar extrair comentário do usuário da descrição original
        var userDescription = ExtractUserDescriptionFromOriginal(originalDescription);

        return rating switch
        {
            5 => $"Uma experiência incrível e inesquecível em {visitedPlace.Name}!{userDescription}",
            4 => $"Uma experiência muito boa em {visitedPlace.Name}!{userDescription}",
            3 => $"Uma experiência legal em {visitedPlace.Name}!{userDescription}",
            2 => $"Uma experiência regular em {visitedPlace.Name}.{userDescription}",
            1 => $"Uma experiência decepcionante em {visitedPlace.Name}.{userDescription}",
            _ => $"Visitamos {visitedPlace.Name}!{userDescription}"
        };
    }

    /// <summary>
    /// Extrai o comentário do usuário da descrição original, removendo a parte automática gerada
    /// </summary>
    /// <param name="originalDescription">Descrição original</param>
    /// <returns>Comentário do usuário extraído ou string vazia</returns>
    private static string ExtractUserDescriptionFromOriginal(string originalDescription)
    {
        if (string.IsNullOrWhiteSpace(originalDescription))
            return "";

        // Padrões para identificar e remover a parte automática
        var patterns = new[]
        {
            @"Com \d+ ❤️: uma experiência .+ em .+!",
            @"Com \d+ ❤️: uma experiência .+ em .+\.",
            @"Uma experiência .+ em .+!",
            @"Uma experiência .+ em .+\.",
            @"Visitamos .+!"
        };

        var userComment = originalDescription;

        foreach (var pattern in patterns)
        {
            var regex = new System.Text.RegularExpressions.Regex(pattern);
            var match = regex.Match(userComment);
            
            if (match.Success)
            {
                // Remove a parte que corresponde ao padrão automático
                userComment = userComment.Replace(match.Value, "").Trim();
                break;
            }
        }

        // Se restou algo, adiciona um espaço no início
        return !string.IsNullOrWhiteSpace(userComment) ? $" {userComment}" : "";
    }
}

public class UploadVisitedPlacePhotosHandler : IRequestHandler<UploadVisitedPlacePhotosCommand, VisitedPlaceDto?>
{
    private readonly IVisitedPlaceRepository _repository;
    private readonly IMapper _mapper;

    public UploadVisitedPlacePhotosHandler(IVisitedPlaceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<VisitedPlaceDto?> Handle(UploadVisitedPlacePhotosCommand request, CancellationToken cancellationToken)
    {
        // PRIMEIRO: Verificar se o VisitedPlace existe
        var visitedPlace = await _repository.GetByIdAsync(request.Id);
        if (visitedPlace == null)
            return null; // Retorna null para indicar NotFound no controller

        // SEGUNDO: Processar e salvar as fotos como BLOB
        var photoEntities = new List<VisitedPlacePhoto>();
        
        foreach (var photo in request.Photos)
        {
            if (photo.Length > 0)
            {
                // Ler dados da imagem para byte array (BLOB)
                using var memoryStream = new MemoryStream();
                await photo.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();
                
                var fileName = $"{Guid.NewGuid()}_{photo.FileName}";
                
                var photoEntity = new VisitedPlacePhoto
                {
                    FileName = fileName,
                    FilePath = $"/api/visitedplaces/{request.Id}/photos/", // URL base
                    Caption = photo.FileName,
                    Description = $"Foto de {visitedPlace.Name}",
                    DateTaken = DateTime.UtcNow,
                    ImageData = imageData, // Armazenar como BLOB
                    ContentType = photo.ContentType ?? "image/jpeg",
                    FileSize = photo.Length,
                    VisitedPlaceId = request.Id,
                    CreatedAt = DateTime.UtcNow
                };
                
                photoEntities.Add(photoEntity);
            }
        }

        // TERCEIRO: Adicionar fotos ao VisitedPlace e salvar
        visitedPlace.Photos.AddRange(photoEntities);
        visitedPlace.UpdatedAt = DateTime.UtcNow;

        var updatedVisitedPlace = await _repository.UpdateAsync(visitedPlace);
        return _mapper.Map<VisitedPlaceDto>(updatedVisitedPlace);
    }
}