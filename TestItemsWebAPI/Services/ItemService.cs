using AutoMapper;
using TestItemsWebAPI.Dto;
using TestItemsWebAPI.Entities;
using TestItemsWebAPI.ExHandlers;
using TestItemsWebAPI.Interfaces;

namespace TestItemsWebAPI.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        public ItemService(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<Result> AddMultipleItemsAsync(IEnumerable<ItemDto> items, CancellationToken token)
        {
            if (items == null || !items.Any())
            {
                return Result.Failure("Item list cannot be null or empty.");
            }

            await _itemRepository.ClearTableAsync(token);

            var sortedItemList = _mapper.Map<List<Item>>(items).OrderBy(i => i.Code).ToList();

            await _itemRepository.InsertItemsAsync(sortedItemList, token);

            return Result.Success();            
        }

        public async Task<Result<List<Item>>> GetItemsAsync(int? code, int page, int pageSize, CancellationToken token)
        {
            if (page <= 0)
            {
                return Result<List<Item>>.Failure("Page must be greater than zero.");
            }

            if (pageSize <= 0)
            {
                return Result<List<Item>>.Failure("Page size must be greater than zero.");
            }
            var items = await _itemRepository.GetItemsAsync(code, page, pageSize, token);
            if (items == null || !items.Any())
            {
                return Result<List<Item>>.Success(new List<Item>());
            }
            
            return Result<List<Item>>.Success(items);
        }
    }
}
