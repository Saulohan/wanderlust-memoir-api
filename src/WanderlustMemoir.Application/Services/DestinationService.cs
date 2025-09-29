using AutoMapper;
using MediatR;
using WanderlustMemoir.Application.DTOs.Destinations;
using WanderlustMemoir.Application.Features.Destinations.Commands;
using WanderlustMemoir.Application.Features.Destinations.Queries;
using WanderlustMemoir.Application.Interfaces;

namespace WanderlustMemoir.Application.Services;

public class DestinationService : IDestinationService
{
    private readonly IMediator _mediator;

    public DestinationService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IEnumerable<DestinationDto>> GetAllDestinationsAsync()
    {
        return await _mediator.Send(new GetAllDestinationsQuery());
    }

    public async Task<DestinationDto?> GetDestinationByIdAsync(int id)
    {
        return await _mediator.Send(new GetDestinationByIdQuery(id));
    }

    public async Task<DestinationDto> CreateDestinationAsync(CreateDestinationDto createDestinationDto)
    {
        return await _mediator.Send(new CreateDestinationCommand(createDestinationDto));
    }

    public async Task<DestinationDto?> UpdateDestinationAsync(int id, UpdateDestinationDto updateDestinationDto)
    {
        return await _mediator.Send(new UpdateDestinationCommand(id, updateDestinationDto));
    }

    public async Task<bool> DeleteDestinationAsync(int id)
    {
        return await _mediator.Send(new DeleteDestinationCommand(id));
    }

    public async Task<DestinationDto?> ToggleDestinationVisitedAsync(int id)
    {
        return await _mediator.Send(new ToggleDestinationVisitedCommand(id));
    }
}