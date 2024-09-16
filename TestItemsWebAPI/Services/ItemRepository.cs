using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Text;
using TestItemsWebAPI.Configurations;
using TestItemsWebAPI.Entities;
using TestItemsWebAPI.Interfaces;

namespace TestItemsWebAPI.Services
{
    public class ItemRepository : IItemRepository
    {
        private readonly DbOptions _dbOptions;

        public ItemRepository(IOptions<DbOptions> dbOptions)
        {
            _dbOptions = dbOptions.Value;
        }

        public async Task InsertItemsAsync(IEnumerable<Item> items, CancellationToken token)
        {
            if (items == null || !items.Any())
            {
                return; 
            }

            await using var conn = new SqlConnection(_dbOptions.DefaultConnection);
            await conn.OpenAsync(token);

            var commandText = "INSERT INTO Items (Code, Value) VALUES ";

            var parameters = new List<string>();
            var parameterIndex = 0;

            foreach (var item in items)
            {
                parameters.Add($"(@Code{parameterIndex}, @Value{parameterIndex})");
                parameterIndex++;
            }

            commandText += string.Join(", ", parameters);

            await using var insertCmd = new SqlCommand(commandText, conn);

            parameterIndex = 0;
            foreach (var item in items)
            {
                insertCmd.Parameters.AddWithValue($"@Code{parameterIndex}", item.Code);
                insertCmd.Parameters.AddWithValue($"@Value{parameterIndex}", item.Value);
                parameterIndex++;
            }

            await insertCmd.ExecuteNonQueryAsync(token);
        }
        public async Task ClearTableAsync(CancellationToken token)
        {
            await using var conn = new SqlConnection(_dbOptions.DefaultConnection);
            await conn.OpenAsync(token);
            await using var clearCmd = new SqlCommand("TRUNCATE TABLE Items", conn);
            await clearCmd.ExecuteNonQueryAsync(token);
        }

        public async Task<List<Item>> GetItemsAsync(int? code, int page, int pageSize, CancellationToken token)
        {
            var items = new List<Item>();
            await using var conn = new SqlConnection(_dbOptions.DefaultConnection);
            await conn.OpenAsync(token);
            var sql = CreateSqlRequest(code.HasValue);
            await using var cmd = new SqlCommand(sql, conn);
            if (code.HasValue)
            {
                cmd.Parameters.AddWithValue("@Code", code.Value);
            }
            SetPaginationParams(page, pageSize, cmd);

            await using var reader = await cmd.ExecuteReaderAsync(token);
            while (await reader.ReadAsync(token))
            {
                items.Add(new Item
                {
                    Id = reader.GetInt32(0),
                    Code = reader.GetInt32(1),
                    Value = reader.GetString(2)
                });
            }

            return items;
        }

        private string CreateSqlRequest(bool useFilter)
        {
            var sqlSb = new StringBuilder("SELECT Id, Code, Value FROM Items");
            if (useFilter)
            {
                sqlSb.Append(" WHERE Code = @Code");
            }
            sqlSb.Append(" ORDER BY Code OFFSET @Offset ROWS FETCH NEXT @Fetch ROWS ONLY");
            return sqlSb.ToString();
        }

        private void SetPaginationParams(int page, int pageSize, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
            cmd.Parameters.AddWithValue("@Fetch", pageSize);
        }
    }
}
