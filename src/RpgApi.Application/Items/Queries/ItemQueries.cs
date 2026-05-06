using MediatR;
using RpgApi.Application.Items.DTOs;

namespace RpgApi.Application.Items.Queries;

public record GetItemByIdQuery(Guid ItemId) : IRequest<ItemDto?>;
public record GetAllItemsQuery : IRequest<IEnumerable<ItemDto>>;
