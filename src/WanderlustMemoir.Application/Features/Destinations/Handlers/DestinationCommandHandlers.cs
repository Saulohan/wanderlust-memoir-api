using AutoMapper;
using MediatR;
using WanderlustMemoir.Application.DTOs.Destinations;
using WanderlustMemoir.Application.Features.Destinations.Commands;
using WanderlustMemoir.Domain.Entities;
using WanderlustMemoir.Domain.Repositories;

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
    private readonly IMapper _mapper;

    public ToggleDestinationVisitedHandler(IDestinationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DestinationDto?> Handle(ToggleDestinationVisitedCommand request, CancellationToken cancellationToken)
    {
        var destination = await _repository.GetByIdAsync(request.Id);
        if (destination == null)
            return null;

        destination.IsVisited = !destination.IsVisited;
        destination.UpdatedAt = DateTime.UtcNow;

        var updatedDestination = await _repository.UpdateAsync(destination);
        return _mapper.Map<DestinationDto>(updatedDestination);
    }
}