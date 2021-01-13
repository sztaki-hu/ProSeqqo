using Newtonsoft.Json;
using SequencePlanner.GTSPTask.Task.PointLike;
using SequencePlanner.Helper;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SequencePlanner.GTSPTask.Serialization.Task
{
    public class PointLikeTaskSerializer
    {
        public void ExportSEQ(PointLikeTask task, string path)
        {
            SeqLogger.Info("Output task type: PointLike", nameof(PointLikeTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(PointLikeTaskSerializer));
            SeqLogger.Info("Output type: SEQ", nameof(PointLikeTaskSerializer));
            var ser = new PointLikeTaskSerializationObject(task);
            var seqString = ser.ToSEQ();
            File.WriteAllText(path, seqString);
            SeqLogger.Info("Output created!", nameof(PointLikeTaskSerializer));
        }

        public PointLikeTask ImportSEQ(string path)
        {
            SeqLogger.Info("Input task type: PointLike", nameof(PointLikeTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(PointLikeTaskSerializer));
            SeqLogger.Info("Input type: SEQ", nameof(PointLikeTaskSerializer));
            var seqString = File.ReadAllLines(path).ToList();
            SeqLogger.Info("Input readed!", nameof(PointLikeTaskSerializer));
            var seqObject = new PointLikeTaskSerializationObject(seqString);
            return seqObject.ToPointLikeTask();
        }

        public void ExportJSON(PointLikeTask task, string path)
        {
            SeqLogger.Info("Output task type: PointLike", nameof(PointLikeTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(PointLikeTaskSerializer));
            SeqLogger.Info("Output type: JSON", nameof(PointLikeTaskSerializer));
            var seqObject = new PointLikeTaskSerializationObject(task);
            var jsonString = JsonConvert.SerializeObject(seqObject);
            File.WriteAllText(path, jsonString);
            SeqLogger.Info("Output created!", nameof(PointLikeTaskSerializer));
        }

        public PointLikeTask ImportJSON(string path)
        {
            SeqLogger.Info("Input task type: PointLike", nameof(PointLikeTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(PointLikeTaskSerializer));
            SeqLogger.Info("Input type: JSON", nameof(PointLikeTaskSerializer));
            var jsonString = File.ReadAllText(path);
            var seqObject = JsonConvert.DeserializeObject<PointLikeTaskSerializationObject>(jsonString);
            SeqLogger.Info("Input readed!", nameof(PointLikeTaskSerializer));
            return seqObject.ToPointLikeTask();
        }

        public void ExportXML(PointLikeTask task, string path)
        {
            SeqLogger.Info("Output task type: PointLike", nameof(PointLikeTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(PointLikeTaskSerializer));
            SeqLogger.Info("Output type: XML", nameof(PointLikeTaskSerializer));
            var seqObject = new PointLikeTaskSerializationObject(task);
            XmlSerializer x = new XmlSerializer(typeof(PointLikeTaskSerializationObject));
            TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, seqObject);
            writer.Close();
            SeqLogger.Info("Output created!", nameof(PointLikeTaskSerializer));
        }

        public PointLikeTask ImportXML(string path)
        {
            SeqLogger.Info("Input task type: PointLike", nameof(PointLikeTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(PointLikeTaskSerializer));
            SeqLogger.Info("Input type: XML", nameof(PointLikeTaskSerializer));
            XmlSerializer x = new XmlSerializer(typeof(PointLikeTaskSerializationObject));
            TextReader reader = new StreamReader(path);
            var seqObject = (PointLikeTaskSerializationObject)x.Deserialize(reader);
            SeqLogger.Info("Input readed!", nameof(PointLikeTaskSerializer));
            return seqObject.ToPointLikeTask();
        }
    }
}