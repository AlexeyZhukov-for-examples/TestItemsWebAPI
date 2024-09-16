using TestItemsWebAPI.Entities;

namespace TestItemsWebAPI.Interfaces
{
    public interface IItemRepository
    {
        Task InsertItemsAsync(IEnumerable<Item> items, CancellationToken token);
        Task ClearTableAsync(CancellationToken token);
        Task<List<Item>> GetItemsAsync(int? code, int page, int pageSize, CancellationToken token);
    }
}
