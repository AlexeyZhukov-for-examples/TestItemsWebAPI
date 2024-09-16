using TestItemsWebAPI.Dto;
using TestItemsWebAPI.Entities;
using TestItemsWebAPI.ExHandlers;

namespace TestItemsWebAPI.Interfaces
{
    public interface IItemService
    {
        Task<Result> AddMultipleItemsAsync(IEnumerable<ItemDto> items, CancellationToken token);
        Task<Result<List<Item>>> GetItemsAsync(int? code, int page, int pageSize, CancellationToken token);
    }
}
