using MediatR;
using RpgApi.Application.Common.Exceptions;
using RpgApi.Application.Common.Mappings;
using RpgApi.Application.Items.DTOs;
using RpgApi.Domain.Entities;
using RpgApi.Domain.Interfaces;

namespace RpgApi.Application.Items.Commands;

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ItemDto>
{
    private readonly IUnitOfWork _uow;
    public CreateItemCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ItemDto> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = Item.Create(
            request.Name, request.Description, request.Type,
            request.StrengthBonus, request.IntelligenceBonus,
            request.AgilityBonus, request.DefenseBonus);

        await _uow.Items.AddAsync(item, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return item.ToDto();
    }
}

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ItemDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateItemCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ItemDto> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _uow.Items.GetByIdAsync(request.ItemId, cancellationToken)
            ?? throw new NotFoundException(nameof(Item), request.ItemId);

        item.Update(request.Name, request.Description,
            request.StrengthBonus, request.IntelligenceBonus,
            request.AgilityBonus, request.DefenseBonus);

        _uow.Items.Update(item);
        await _uow.SaveChangesAsync(cancellationToken);

        return item.ToDto();
    }
}

public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteItemCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _uow.Items.GetByIdAsync(request.ItemId, cancellationToken)
            ?? throw new NotFoundException(nameof(Item), request.ItemId);

        _uow.Items.Delete(item);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
