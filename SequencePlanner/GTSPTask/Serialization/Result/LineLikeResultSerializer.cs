using SequencePlanner.GTSPTask.Result;
using SequencePlanner.Helper;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    public class LineLikeResultSerializer
    {
        public void ExportSEQ(LineTaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: LineLike", nameof(LineLikeResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineLikeResultSerializer));
            SeqLogger.Debug("Output type: SEQ", nameof(LineLikeResultSerializer));
            var ser = new LineLikeResultSerializationObject(result);
            var seqString = ser.ToSEQ();
            File.WriteAllText(path, seqString);
            SeqLogger.Debug("Output created!", nameof(LineLikeResultSerializer));
        }

        public LineTaskResult ImportSEQ(string path)
        {
            SeqLogger.Debug("Input task type: LineLike", nameof(LineLikeResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineLikeResultSerializer));
            SeqLogger.Debug("Input type: SEQ", nameof(LineLikeResultSerializer));
            var seqString = File.ReadAllLines(path).ToList();
            SeqLogger.Debug("Input readed!", nameof(LineLikeResultSerializer));
            var seqObject = new LineLikeResultSerializationObject(seqString);
            return seqObject.ToLineLikeResult();
        }

        public void ExportJSON(LineTaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: LineLike", nameof(LineLikeResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineLikeResultSerializer));
            SeqLogger.Debug("Output type: JSON", nameof(LineLikeResultSerializer));
            var seqObject = new LineLikeResultSerializationObject(result);
            var jsonString = JsonSerializer.Serialize(seqObject);
            File.WriteAllText(path, jsonString);
            SeqLogger.Debug("Output created!", nameof(LineLikeResultSerializer));
        }

        public LineTaskResult ImportJSON(string path)
        {
            SeqLogger.Debug("Input task type: LineLike", nameof(LineLikeResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineLikeResultSerializer));
            SeqLogger.Debug("Input type: JSON", nameof(LineLikeResultSerializer));
            var jsonString = File.ReadAllText(path);
            var seqObject = JsonSerializer.Deserialize<LineLikeResultSerializationObject>(jsonString);
            SeqLogger.Debug("Input readed!", nameof(LineLikeResultSerializer));
            return seqObject.ToLineLikeResult();
        }

        public void ExportXML(LineTaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: LineLike", nameof(LineLikeResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineLikeResultSerializer));
            SeqLogger.Debug("Output type: XML", nameof(LineLikeResultSerializer));
            var seqObject = new LineLikeResultSerializationObject(result);
            XmlSerializer x = new XmlSerializer(typeof(LineLikeResultSerializationObject));
            TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, seqObject);
            writer.Close();
            SeqLogger.Debug("Output created!", nameof(LineLikeResultSerializer));
        }

        public LineTaskResult ImportXML(string path)
        {
            SeqLogger.Debug("Input task type: LineLike", nameof(LineLikeResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineLikeResultSerializer));
            SeqLogger.Debug("Input type: XML", nameof(LineLikeResultSerializer));
            XmlSerializer x = new XmlSerializer(typeof(LineLikeResultSerializer));
            TextReader reader = new StreamReader(path);
            var seqObject = (LineLikeResultSerializationObject)x.Deserialize(reader);
            SeqLogger.Debug("Input readed!", nameof(LineLikeResultSerializer));
            return seqObject.ToLineLikeResult();
        }
    }
}