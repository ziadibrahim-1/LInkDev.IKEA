using LinkDev.IKEA.DAL.Common.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LinkDev.IKEA.DAL.Common.JsonConvertors
{
    public class EmployeeTypeJsonConvertor : JsonConverter<EmployeeType>
    {
        public override EmployeeType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var EmployeeTypeAsString = reader.GetString();
            return EmployeeTypeAsString?.ToLower() switch
            {
                "fulltime" => EmployeeType.FullTime,
                "parttime" => EmployeeType.PartTime,
                "intern" => EmployeeType.Intern,
                _ => EmployeeType.FullTime
            };
        }
        public override void Write(Utf8JsonWriter writer, EmployeeType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
