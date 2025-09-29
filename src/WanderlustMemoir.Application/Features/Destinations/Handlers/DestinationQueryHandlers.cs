using AutoMapper;
using MediatR;
using WanderlustMemoir.Application.DTOs.Destinations;
using WanderlustMemoir.Application.Features.Destinations.Queries;
using WanderlustMemoir.Domain.Repositories;

namespace WanderlustMemoir.Application.Features.Destinations.Handlers;

public class GetAllDestinationsHandler : IRequestHandler<GetAllDestinationsQuery, IEnumerable<DestinationDto>>
{
    private readonly IDestinationRepository _repository;
    private readonly IMapper _mapper;

    public GetAllDestinationsHandler(IDestinationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DestinationDto>> Handle(GetAllDestinationsQuery request, CancellationToken cancellationToken)
    {
        var destinations = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<DestinationDto>>(destinations.OrderByDescending(d => d.Priority).ThenBy(d => d.Name));
    }
}

public class GetDestinationByIdHandler : IRequestHandler<GetDestinationByIdQuery, DestinationDto?>
{
    private readonly IDestinationRepository _repository;
    private readonly IMapper _mapper;

    public GetDestinationByIdHandler(IDestinationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DestinationDto?> Handle(GetDestinationByIdQuery request, CancellationToken cancellationToken)
    {
        var destination = await _repository.GetByIdAsync(request.Id);
        return destination == null ? null : _mapper.Map<DestinationDto>(destination);
    }
}