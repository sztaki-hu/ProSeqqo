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
            SeqLogger.Debug("Output task type: LineLike", nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineLikeTaskSerializer));
            SeqLogger.Debug("Output type: SEQ", nameof(LineLikeTaskSerializer));
            var ser = new LineLikeTaskSerializationObject(task);
            var seqString = ser.ToSEQ();
            File.WriteAllText(path, seqString);
            SeqLogger.Debug("Output created!", nameof(LineLikeTaskSerializer));
        }

        public LineLikeTask ImportSEQ(string path)
        {
            SeqLogger.Debug("Input task type: LineLike", nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineLikeTaskSerializer));
            SeqLogger.Debug("Input type: SEQ", nameof(LineLikeTaskSerializer));
            var seqString = File.ReadAllLines(path).ToList();
            SeqLogger.Debug("Input readed!", nameof(LineLikeTaskSerializer));
            var seqObject = new LineLikeTaskSerializationObject(seqString);
            return seqObject.ToLineLikeTask();
        }

        public void ExportJSON(LineLikeTask task, string path)
        {
            SeqLogger.Debug("Output task type: LineLike", nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineLikeTaskSerializer));
            SeqLogger.Debug("Output type: JSON", nameof(LineLikeTaskSerializer));
            var seqObject = new LineLikeTaskSerializationObject(task);
            var jsonString = JsonSerializer.Serialize(seqObject);
            File.WriteAllText(path, jsonString);
            SeqLogger.Debug("Output created!", nameof(LineLikeTaskSerializer));
        }

        public LineLikeTask ImportJSON(string path)
        {
            SeqLogger.Debug("Input task type: LineLike", nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineLikeTaskSerializer));
            SeqLogger.Debug("Input type: JSON", nameof(LineLikeTaskSerializer));
            var jsonString = File.ReadAllText(path);
            var seqObject = JsonSerializer.Deserialize<LineLikeTaskSerializationObject>(jsonString);
            SeqLogger.Debug("Input readed!", nameof(LineLikeTaskSerializer));
            return seqObject.ToLineLikeTask();
        }

        public void ExportXML(LineLikeTask task, string path)
        {
            SeqLogger.Debug("Output task type: LineLike", nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(LineLikeTaskSerializer));
            SeqLogger.Debug("Output type: XML", nameof(LineLikeTaskSerializer));
            var seqObject = new LineLikeTaskSerializationObject(task);
            XmlSerializer x = new XmlSerializer(typeof(LineLikeTaskSerializationObject));
            TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, seqObject);
            writer.Close();
            SeqLogger.Debug("Output created!", nameof(LineLikeTaskSerializer));
        }

        public LineLikeTask ImportXML(string path)
        {
            SeqLogger.Debug("Input task type: LineLike", nameof(LineLikeTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(LineLikeTaskSerializer));
            SeqLogger.Debug("Input type: XML", nameof(LineLikeTaskSerializer));
            XmlSerializer x = new XmlSerializer(typeof(LineLikeTaskSerializationObject));
            TextReader reader = new StreamReader(path);
            var seqObject = (LineLikeTaskSerializationObject)x.Deserialize(reader);
            SeqLogger.Debug("Input readed!", nameof(LineLikeTaskSerializer));
            return seqObject.ToLineLikeTask();
        }
    }
}