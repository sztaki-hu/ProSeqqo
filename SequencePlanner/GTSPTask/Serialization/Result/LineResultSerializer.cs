using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.Helper;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    public class LineResultSerializer
    {
        public void ExportSEQ(LineTaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: Line", nameof(LineResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineResultSerializer));
            SeqLogger.Debug("Output type: SEQ", nameof(LineResultSerializer));
            var ser = new LineResultSerializationObject(result);
            var seqString = ser.ToSEQ();
            File.WriteAllText(path, seqString);
            SeqLogger.Debug("Output created!", nameof(LineResultSerializer));
        }

        public LineTaskResult ImportSEQ(string path)
        {
            SeqLogger.Debug("Input task type: Line", nameof(LineResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineResultSerializer));
            SeqLogger.Debug("Input type: SEQ", nameof(LineResultSerializer));
            var seqString = File.ReadAllLines(path).ToList();
            SeqLogger.Debug("Input readed!", nameof(LineResultSerializer));
            var seqObject = new LineResultSerializationObject(seqString);
            return seqObject.ToLineResult();
        }

        public void ExportJSON(LineTaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: Line", nameof(LineResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineResultSerializer));
            SeqLogger.Debug("Output type: JSON", nameof(LineResultSerializer));
            var seqObject = new LineResultSerializationObject(result);
            var jsonString = JsonSerializer.Serialize(seqObject);
            File.WriteAllText(path, jsonString);
            SeqLogger.Debug("Output created!", nameof(LineResultSerializer));
        }

        public LineTaskResult ImportJSON(string path)
        {
            SeqLogger.Debug("Input task type: Line", nameof(LineResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineResultSerializer));
            SeqLogger.Debug("Input type: JSON", nameof(LineResultSerializer));
            var jsonString = File.ReadAllText(path);
            var seqObject = JsonSerializer.Deserialize<LineResultSerializationObject>(jsonString);
            SeqLogger.Debug("Input readed!", nameof(LineResultSerializer));
            return seqObject.ToLineResult();
        }

        public void ExportXML(LineTaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: Line", nameof(LineResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineResultSerializer));
            SeqLogger.Debug("Output type: XML", nameof(LineResultSerializer));
            var seqObject = new LineResultSerializationObject(result);
            XmlSerializer x = new XmlSerializer(typeof(LineResultSerializationObject));
            TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, seqObject);
            writer.Close();
            SeqLogger.Debug("Output created!", nameof(LineResultSerializer));
        }

        public LineTaskResult ImportXML(string path)
        {
            SeqLogger.Debug("Input task type: Line", nameof(LineResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineResultSerializer));
            SeqLogger.Debug("Input type: XML", nameof(LineResultSerializer));
            XmlSerializer x = new XmlSerializer(typeof(LineResultSerializer));
            TextReader reader = new StreamReader(path);
            var seqObject = (LineResultSerializationObject)x.Deserialize(reader);
            SeqLogger.Debug("Input readed!", nameof(LineResultSerializer));
            return seqObject.ToLineResult();
        }
    }
}