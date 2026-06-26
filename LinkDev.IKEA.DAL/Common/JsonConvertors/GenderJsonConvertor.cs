using LinkDev.IKEA.DAL.Common.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LinkDev.IKEA.DAL.Common.JsonConvertors
{
    internal class GenderJsonConvertor : JsonConverter<Gender>
    {
        public override Gender Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var GenderTypeAsString = reader.GetString();
            return GenderTypeAsString?.ToLower() switch
            {
                "male" => Gender.Male,
                "female" => Gender.Female,
                _ => Gender.Male
            };
        }

        public override void Write(Utf8JsonWriter writer, Gender value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
