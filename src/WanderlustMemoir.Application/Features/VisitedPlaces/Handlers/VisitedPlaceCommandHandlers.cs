using AutoMapper;
using MediatR;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;
using WanderlustMemoir.Application.Features.VisitedPlaces.Commands;
using WanderlustMemoir.Domain.Entities;
using WanderlustMemoir.Domain.Repositories;

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
        var createdVisitedPlace = await _repository.CreateAsync(visitedPlace);
        return _mapper.Map<VisitedPlaceDto>(createdVisitedPlace);
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