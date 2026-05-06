using MediatR;
using RpgApi.Application.Common.Mappings;
using RpgApi.Application.Items.DTOs;
using RpgApi.Domain.Interfaces;

namespace RpgApi.Application.Items.Queries;

public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemDto?>
{
    private readonly IUnitOfWork _uow;
    public GetItemByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ItemDto?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _uow.Items.GetByIdAsync(request.ItemId, cancellationToken);
        return item?.ToDto();
    }
}

public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, IEnumerable<ItemDto>>
{
    private readonly IUnitOfWork _uow;
    public GetAllItemsQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<ItemDto>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _uow.Items.GetAllAsync(cancellationToken);
        return items.Select(i => i.ToDto()).ToList();
    }
}
