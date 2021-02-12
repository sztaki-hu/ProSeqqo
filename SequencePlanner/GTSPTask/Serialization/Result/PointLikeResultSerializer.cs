using Newtonsoft.Json;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.Helper;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    public class PointLikeResultSerializer
    {
        public void ExportSEQ(PointTaskResult result, string path)
        {
            SeqLogger.Info("Output task type: PointLike", nameof(PointLikeResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(PointLikeResultSerializer));
            SeqLogger.Info("Output type: SEQ", nameof(PointLikeResultSerializer));
            var ser = new PointLikeResultSerializationObject(result);
            var seqString = ser.ToSEQ();
            File.WriteAllText(path, seqString);
            SeqLogger.Info("Output created!", nameof(PointLikeResultSerializer));
        }

        public PointTaskResult ImportSEQ(string path)
        {
            SeqLogger.Info("Output task type: PointLike", nameof(PointLikeResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(PointLikeResultSerializer));
            SeqLogger.Info("Output type: SEQ", nameof(PointLikeResultSerializer));
            var seqString = File.ReadAllLines(path).ToList();
            var seqObject = new PointLikeResultSerializationObject(seqString);
            SeqLogger.Info("Input readed!", nameof(PointLikeResultSerializer));
            return seqObject.ToPointLikeResult();
        }

        public void ExportJSON(PointTaskResult result, string path)
        {
            SeqLogger.Info("Output task type: PointLike", nameof(PointLikeResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(PointLikeResultSerializer));
            SeqLogger.Info("Output type: JSON", nameof(PointLikeResultSerializer));
            var seqObject = new PointLikeResultSerializationObject(result);
            var jsonString = JsonConvert.SerializeObject(seqObject);
            File.WriteAllText(path, jsonString);

            SeqLogger.Info("Output created!", nameof(PointLikeResultSerializer));
        }

        public PointTaskResult ImportJSON(string path)
        {
            SeqLogger.Info("Input task type: PointLike", nameof(PointLikeResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(PointLikeResultSerializer));
            SeqLogger.Info("Input type: JSON", nameof(PointLikeResultSerializer));
            var jsonString = File.ReadAllText(path);
            var seqObject = JsonConvert.DeserializeObject<PointLikeResultSerializationObject>(jsonString);
            SeqLogger.Info("Input readed!", nameof(PointLikeResultSerializer));
            return seqObject.ToPointLikeResult();
        }

        public void ExportXML(PointTaskResult result, string path)
        {
            SeqLogger.Info("Output task type: PointLike", nameof(PointLikeResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(PointLikeResultSerializer));
            SeqLogger.Info("Output type: XML", nameof(PointLikeResultSerializer));
            var seqObject = new PointLikeResultSerializationObject(result);
            XmlSerializer x = new XmlSerializer(typeof(PointLikeResultSerializationObject));
            TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, seqObject);
            writer.Close();
            SeqLogger.Info("Output created!", nameof(PointLikeResultSerializer));
        }

        public PointTaskResult ImportXML(string path)
        {
            SeqLogger.Info("Input task type: PointLike", nameof(PointLikeResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(PointLikeResultSerializer));
            SeqLogger.Info("Input type: XML", nameof(PointLikeResultSerializer));
            XmlSerializer x = new XmlSerializer(typeof(PointLikeResultSerializationObject));
            TextReader reader = new StreamReader(path);
            var seqObject = (PointLikeResultSerializationObject)x.Deserialize(reader);
            SeqLogger.Info("Input readed!", nameof(PointLikeResultSerializer));
            return seqObject.ToPointLikeResult();
        }
    }
}