using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Common.JsonConvertors
{
    public class DateOnlyJsonConvertor : JsonConverter<DateOnly>
    {
        private static readonly string[] Formats =
        {
            "dd/MM/yy",
            "dd/MM/yyyy",
            "yyyy-MM-dd",
        };

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            if (string.IsNullOrWhiteSpace(value))
                throw new JsonException("Date value is null or empty.");

            if (DateOnly.TryParseExact(
                    value,
                    Formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date))
            {
                return date;
            }

            throw new JsonException($"Invalid date format. Supported formats: {string.Join(", ", Formats)}");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            
            writer.WriteStringValue(value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
        }
    }
}
