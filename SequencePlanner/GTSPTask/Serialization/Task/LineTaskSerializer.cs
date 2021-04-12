using SequencePlanner.GTSPTask.Task.LineTask;
using SequencePlanner.Helper;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;

namespace SequencePlanner.GTSPTask.Serialization.Task
{
    public class LineTaskSerializer
    {
        public void ExportSEQ(LineTask task, string path)
        {
            SeqLogger.Debug("Output task type: Line", nameof(LineTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineTaskSerializer));
            SeqLogger.Debug("Output type: SEQ", nameof(LineTaskSerializer));
            var ser = new LineTaskSerializationObject(task);
            var seqString = ser.ToSEQ();
            File.WriteAllText(path, seqString);
            SeqLogger.Debug("Output created!", nameof(LineTaskSerializer));
        }

        public LineTask ImportSEQ(string path)
        {
            SeqLogger.Debug("Input task type: Line", nameof(LineTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineTaskSerializer));
            SeqLogger.Debug("Input type: SEQ", nameof(LineTaskSerializer));
            var seqString = File.ReadAllLines(path).ToList();
            SeqLogger.Debug("Input readed!", nameof(LineTaskSerializer));
            var seqObject = new LineTaskSerializationObject(seqString);
            return seqObject.ToLineTask();
        }

        public void ExportJSON(LineTask task, string path)
        {
            SeqLogger.Debug("Output task type: Line", nameof(LineTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineTaskSerializer));
            SeqLogger.Debug("Output type: JSON", nameof(LineTaskSerializer));
            var seqObject = new LineTaskSerializationObject(task);
            var jsonString = JsonSerializer.Serialize(seqObject);
            File.WriteAllText(path, jsonString);
            SeqLogger.Debug("Output created!", nameof(LineTaskSerializer));
        }

        public LineTask ImportJSON(string path)
        {
            SeqLogger.Debug("Input task type: Line", nameof(LineTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineTaskSerializer));
            SeqLogger.Debug("Input type: JSON", nameof(LineTaskSerializer));
            var jsonString = File.ReadAllText(path);
            var seqObject = JsonSerializer.Deserialize<LineTaskSerializationObject>(jsonString);
            SeqLogger.Debug("Input readed!", nameof(LineTaskSerializer));
            return seqObject.ToLineTask();
        }

        public void ExportXML(LineTask task, string path)
        {
            SeqLogger.Debug("Output task type: Line", nameof(LineTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineTaskSerializer));
            SeqLogger.Debug("Output type: XML", nameof(LineTaskSerializer));
            var seqObject = new LineTaskSerializationObject(task);
            XmlSerializer x = new XmlSerializer(typeof(LineTaskSerializationObject));
            TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, seqObject);
            writer.Close();
            SeqLogger.Debug("Output created!", nameof(LineTaskSerializer));
        }

        public LineTask ImportXML(string path)
        {
            SeqLogger.Debug("Input task type: Line", nameof(LineTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineTaskSerializer));
            SeqLogger.Debug("Input type: XML", nameof(LineTaskSerializer));
            XmlSerializer x = new XmlSerializer(typeof(LineTaskSerializationObject));
            TextReader reader = new StreamReader(path);
            var seqObject = (LineTaskSerializationObject)x.Deserialize(reader);
            SeqLogger.Debug("Input readed!", nameof(LineTaskSerializer));
            return seqObject.ToLineTask();
        }
    }
}