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
            SeqLogger.Debug("Output task type: PointLike", nameof(PointLikeResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(PointLikeResultSerializer));
            SeqLogger.Debug("Output type: SEQ", nameof(PointLikeResultSerializer));
            var ser = new PointLikeResultSerializationObject(result);
            var seqString = ser.ToSEQ();
            File.WriteAllText(path, seqString);
            SeqLogger.Debug("Output created!", nameof(PointLikeResultSerializer));
        }

        public PointTaskResult ImportSEQ(string path)
        {
            SeqLogger.Debug("Input task type: PointLike", nameof(PointLikeResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(PointLikeResultSerializer));
            SeqLogger.Debug("Input type: SEQ", nameof(PointLikeResultSerializer));
            var seqString = File.ReadAllLines(path).ToList();
            var seqObject = new PointLikeResultSerializationObject(seqString);
            SeqLogger.Debug("Input readed!", nameof(PointLikeResultSerializer));
            return seqObject.ToPointLikeResult();
        }

        public void ExportJSON(PointTaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: PointLike", nameof(PointLikeResultSerializer));
            SeqLogger.Debug("Output file: " + path, nameof(PointLikeResultSerializer));
            SeqLogger.Info("Output type: JSON", nameof(PointLikeResultSerializer));
            var seqObject = new PointLikeResultSerializationObject(result);
            var jsonString = JsonConvert.SerializeObject(seqObject);
            File.WriteAllText(path, jsonString);
            SeqLogger.Debug("Output created!", nameof(PointLikeResultSerializer));
        }

        public PointTaskResult ImportJSON(string path)
        {
            SeqLogger.Debug("Input task type: PointLike", nameof(PointLikeResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(PointLikeResultSerializer));
            SeqLogger.Debug("Input type: JSON", nameof(PointLikeResultSerializer));
            var jsonString = File.ReadAllText(path);
            var seqObject = JsonConvert.DeserializeObject<PointLikeResultSerializationObject>(jsonString);
            SeqLogger.Debug("Input readed!", nameof(PointLikeResultSerializer));
            return seqObject.ToPointLikeResult();
        }

        public void ExportXML(PointTaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: PointLike", nameof(PointLikeResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(PointLikeResultSerializer));
            SeqLogger.Debug("Output type: XML", nameof(PointLikeResultSerializer));
            var seqObject = new PointLikeResultSerializationObject(result);
            XmlSerializer x = new XmlSerializer(typeof(PointLikeResultSerializationObject));
            TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, seqObject);
            writer.Close();
            SeqLogger.Debug("Output created!", nameof(PointLikeResultSerializer));
        }

        public PointTaskResult ImportXML(string path)
        {
            SeqLogger.Debug("Input task type: PointLike", nameof(PointLikeResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(PointLikeResultSerializer));
            SeqLogger.Debug("Input type: XML", nameof(PointLikeResultSerializer));
            XmlSerializer x = new XmlSerializer(typeof(PointLikeResultSerializationObject));
            TextReader reader = new StreamReader(path);
            var seqObject = (PointLikeResultSerializationObject)x.Deserialize(reader);
            SeqLogger.Debug("Input readed!", nameof(PointLikeResultSerializer));
            return seqObject.ToPointLikeResult();
        }
    }
}