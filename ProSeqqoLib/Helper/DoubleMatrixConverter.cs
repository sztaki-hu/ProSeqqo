using Newtonsoft.Json;
using System;
using System.Text.Json;

namespace ProSeqqoLib.Helper
{
    public class DoubleMatrixConverter : System.Text.Json.Serialization.JsonConverter<double[,]>
    {
        public override double[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonConvert.DeserializeObject<double[,]>(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, double[,] matrix, JsonSerializerOptions options)
        {
            writer.WriteStringValue(JsonConvert.SerializeObject(matrix));
        }
    }
}