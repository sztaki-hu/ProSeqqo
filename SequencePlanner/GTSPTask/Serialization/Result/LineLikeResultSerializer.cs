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
            SeqLogger.Info("Output task type: Line_Like", nameof(LineLikeResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineLikeResultSerializer));
            SeqLogger.Info("Output type: SEQ", nameof(LineLikeResultSerializer));
            var ser = new LineLikeResultSerializationObject(result);
            var seqString = ser.ToSEQ();
            File.WriteAllText(path, seqString);
            SeqLogger.Info("Output created!", nameof(LineLikeResultSerializer));
        }

        public LineTaskResult ImportSEQ(string path)
        {
            SeqLogger.Info("Input task type: Line_Like", nameof(LineLikeResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineLikeResultSerializer));
            SeqLogger.Info("Input type: SEQ", nameof(LineLikeResultSerializer));
            var seqString = File.ReadAllLines(path).ToList();
            SeqLogger.Info("Input readed!", nameof(LineLikeResultSerializer));
            var seqObject = new LineLikeResultSerializationObject(seqString);
            return seqObject.ToLineLikeResult();
        }

        public void ExportJSON(LineTaskResult result, string path)
        {
            SeqLogger.Info("Output task type: Line_Like", nameof(LineLikeResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineLikeResultSerializer));
            SeqLogger.Info("Output type: JSON", nameof(LineLikeResultSerializer));
            var seqObject = new LineLikeResultSerializationObject(result);
            var jsonString = JsonSerializer.Serialize(seqObject);
            File.WriteAllText(path, jsonString);
            SeqLogger.Info("Output created!", nameof(LineLikeResultSerializer));
        }

        public LineTaskResult ImportJSON(string path)
        {
            SeqLogger.Info("Input task type: Line_Like", nameof(LineLikeResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineLikeResultSerializer));
            SeqLogger.Info("Input type: JSON", nameof(LineLikeResultSerializer));
            var jsonString = File.ReadAllText(path);
            var seqObject = JsonSerializer.Deserialize<LineLikeResultSerializationObject>(jsonString);
            SeqLogger.Info("Input readed!", nameof(LineLikeResultSerializer));
            return seqObject.ToLineLikeResult();
        }

        public void ExportXML(LineTaskResult result, string path)
        {
            SeqLogger.Info("Output task type: Line_Like", nameof(LineLikeResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineLikeResultSerializer));
            SeqLogger.Info("Output type: XML", nameof(LineLikeResultSerializer));
            var seqObject = new LineLikeResultSerializationObject(result);
            XmlSerializer x = new XmlSerializer(typeof(LineLikeResultSerializationObject));
            TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, seqObject);
            writer.Close();
            SeqLogger.Info("Output created!", nameof(LineLikeResultSerializer));
        }

        public LineTaskResult ImportXML(string path)
        {
            SeqLogger.Info("Input task type: Line_Like", nameof(LineLikeResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineLikeResultSerializer));
            SeqLogger.Info("Input type: XML", nameof(LineLikeResultSerializer));
            XmlSerializer x = new XmlSerializer(typeof(LineLikeResultSerializer));
            TextReader reader = new StreamReader(path);
            var seqObject = (LineLikeResultSerializationObject)x.Deserialize(reader);
            SeqLogger.Info("Input readed!", nameof(LineLikeResultSerializer));
            return seqObject.ToLineLikeResult();
        }
    }
}