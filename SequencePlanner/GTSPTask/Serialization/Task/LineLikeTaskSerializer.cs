using SequencePlanner.GTSPTask.Task.LineLike;
using SequencePlanner.Helper;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;

namespace SequencePlanner.GTSPTask.Serialization.Task
{
    public class LineLikeTaskSerializer
    {
        public void ExportSEQ(LineLikeTask task, string path)
        {
            SeqLogger.Info("Output task type: Line_Like", nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Output type: SEQ", nameof(LineLikeTaskSerializer));
            var ser = new LineLikeTaskSerializationObject(task);
            var seqString = ser.ToSEQ();
            File.WriteAllText(path, seqString);
            SeqLogger.Info("Output created!", nameof(LineLikeTaskSerializer));
        }

        public LineLikeTask ImportSEQ(string path)
        {
            SeqLogger.Info("Input task type: Line_Like", nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Input type: SEQ", nameof(LineLikeTaskSerializer));
            var seqString = File.ReadAllLines(path).ToList();
            SeqLogger.Info("Input readed!", nameof(LineLikeTaskSerializer));
            var seqObject = new LineLikeTaskSerializationObject(seqString);
            return seqObject.ToLineLikeTask();
        }

        public void ExportJSON(LineLikeTask task, string path)
        {
            SeqLogger.Info("Output task type: Line_Like", nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Output type: JSON", nameof(LineLikeTaskSerializer));
            var seqObject = new LineLikeTaskSerializationObject(task);
            var jsonString = JsonSerializer.Serialize(seqObject);
            File.WriteAllText(path, jsonString);
            SeqLogger.Info("Output created!", nameof(LineLikeTaskSerializer));
        }

        public LineLikeTask ImportJSON(string path)
        {
            SeqLogger.Info("Input task type: Line_Like", nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Input type: JSON", nameof(LineLikeTaskSerializer));
            var jsonString = File.ReadAllText(path);
            var seqObject = JsonSerializer.Deserialize<LineLikeTaskSerializationObject>(jsonString);
            SeqLogger.Info("Input readed!", nameof(LineLikeTaskSerializer));
            return seqObject.ToLineLikeTask();
        }

        public void ExportXML(LineLikeTask task, string path)
        {
            SeqLogger.Info("Output task type: Line_Like", nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Output type: XML", nameof(LineLikeTaskSerializer));
            var seqObject = new LineLikeTaskSerializationObject(task);
            XmlSerializer x = new XmlSerializer(typeof(LineLikeTaskSerializationObject));
            TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, seqObject);
            writer.Close();
            SeqLogger.Info("Output created!", nameof(LineLikeTaskSerializer));
        }

        public LineLikeTask ImportXML(string path)
        {
            SeqLogger.Info("Input task type: Line_Like", nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Input type: XML", nameof(LineLikeTaskSerializer));
            XmlSerializer x = new XmlSerializer(typeof(LineLikeTaskSerializationObject));
            TextReader reader = new StreamReader(path);
            var seqObject = (LineLikeTaskSerializationObject)x.Deserialize(reader);
            SeqLogger.Info("Input readed!", nameof(LineLikeTaskSerializer));
            return seqObject.ToLineLikeTask();
        }
    }
}