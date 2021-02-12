using Newtonsoft.Json;
using System;
using System.Text.Json;

namespace SequencePlanner.Helper
{
    public class DoubleMatrixConverter : System.Text.Json.Serialization.JsonConverter<double[,]>
    {
        public override double[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            //double[].Parse(reader.GetString());
            return JsonConvert.DeserializeObject<double[,]>(reader.GetString());

        }


        public override void Write(Utf8JsonWriter writer, double[,] matrix, JsonSerializerOptions options){
                writer.WriteStringValue(JsonConvert.SerializeObject(matrix));
                //writer.WriteStringValue(JsonConvert.SerializeObject(matrix));
        }
                    
        //private string MatrixToJSON(double[,] matrix)
        //{
        //    string tmp ="{";
        //    for (int i = 0; i < matrix.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < matrix.GetLength(1); j++)
        //        {
        //            matrix[i, j].ToString("0.####");   //24.16 (rounded up)
        //            matrix[i,j] = 
        //        }
        //    }
        //    return tmp;
        //}
       
    }
}
