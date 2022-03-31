using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TodoList.Converters;

public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateTimeString = reader.GetString();
        return DateTime.ParseExact(dateTimeString!, "yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy/MM/dd HH:mm:ss"));
    }
}