using System.Text.Json.Serialization;
using System.Text.Json;

namespace TestItemsWebAPI.Dto
{
    public class ItemDtoConverter : JsonConverter<ItemDto>
    {
        public override ItemDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var item = new ItemDto();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                string propertyName = reader.GetString();
                reader.Read();

                item.Code = int.Parse(propertyName);
                item.Value = reader.GetString();
            }
            return item;
        }

        public override void Write(Utf8JsonWriter writer, ItemDto value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}