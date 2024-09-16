using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Nodes;
using TestItemsWebAPI.Dto;
using TestItemsWebAPI.ExHandlers;
using TestItemsWebAPI.Interfaces;

namespace TestItemsWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class ItemsController : ControllerBase
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly IItemService _itemService;

        public ItemsController(ILogger<ItemsController> logger, IItemService itemService)
        {
            _logger = logger;
            _itemService = itemService;
        }

        [HttpPost]
        public async Task<IActionResult> AddMultipleItems([FromBody] IEnumerable<ItemDto> items, CancellationToken token)
        {
            var result = await _itemService.AddMultipleItemsAsync(items, token);
            if (result.IsSuccess)
            {
                return Ok("Items saved successfully");
            }
            return result.ErrorMessage != null ? BadRequest(result.ErrorMessage) : StatusCode(500, "Internal server error");
        }
        [HttpGet]
        public async Task<IActionResult> GetItems([FromQuery] int? code, [FromQuery] int page, [FromQuery] int pageSize, CancellationToken token)
        {
            try
            {
                var result = await _itemService.GetItemsAsync(code, page, pageSize, token);
                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }
                return BadRequest(result.ErrorMessage ?? "An unspecified error occurred.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting items.");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
