using Newtonsoft.Json;
using SequencePlanner.Helper;
using SequencePlanner.Task;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SequencePlanner.GTSPTask.Serialization.Task
{
    public class GeneralTaskSerializer
    {
        public void ExportSEQ(GeneralTask task, string path)
        {
            SeqLogger.Debug("Output task type: General", nameof(GeneralTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(GeneralTaskSerializer));
            SeqLogger.Debug("Output type: SEQ", nameof(GeneralTaskSerializer));
            var ser = new GeneralTaskSerializationObject(task);
            var seqString = ser.ToSEQ();
            File.WriteAllText(path, seqString);
            SeqLogger.Debug("Output created!", nameof(GeneralTaskSerializer));
        }

        public GeneralTask ImportSEQ(string path)
        {
            SeqLogger.Debug("Input task type: General", nameof(GeneralTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(GeneralTaskSerializer));
            SeqLogger.Debug("Input type: SEQ", nameof(GeneralTaskSerializer));
            var seqString = File.ReadAllLines(path).ToList();
            SeqLogger.Debug("Input readed!", nameof(GeneralTaskSerializer));
            var seqObject = new GeneralTaskSerializationObject(seqString);
            return seqObject.ToGeneralTask();
        }

        public void ExportJSON(GeneralTask task, string path)
        {
            SeqLogger.Debug("Output task type: General", nameof(GeneralTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(GeneralTaskSerializer));
            SeqLogger.Debug("Output type: JSON", nameof(GeneralTaskSerializer));
            var seqObject = new GeneralTaskSerializationObject(task);
            var jsonString = JsonConvert.SerializeObject(seqObject);
            File.WriteAllText(path, jsonString);
            SeqLogger.Debug("Output created!", nameof(GeneralTaskSerializer));
        }

        public GeneralTask ImportJSON(string path)
        {
            SeqLogger.Debug("Input task type: General", nameof(GeneralTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(GeneralTaskSerializer));
            SeqLogger.Debug("Input type: JSON", nameof(GeneralTaskSerializer));
            var jsonString = File.ReadAllText(path);
            var seqObject = JsonConvert.DeserializeObject<GeneralTaskSerializationObject>(jsonString);
            SeqLogger.Debug("Input readed!", nameof(GeneralTaskSerializer));
            return seqObject.ToGeneralTask();
        }

        public void ExportXML(GeneralTask task, string path)
        {
            SeqLogger.Debug("Output task type: General", nameof(GeneralTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(GeneralTaskSerializer));
            SeqLogger.Debug("Output type: XML", nameof(GeneralTaskSerializer));
            var seqObject = new GeneralTaskSerializationObject(task);
            XmlSerializer x = new XmlSerializer(typeof(GeneralTaskSerializationObject));
            TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, seqObject);
            writer.Close();
            SeqLogger.Debug("Output created!", nameof(GeneralTaskSerializer));
        }

        public GeneralTask ImportXML(string path)
        {
            SeqLogger.Debug("Input task type: General", nameof(GeneralTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(GeneralTaskSerializer));
            SeqLogger.Debug("Input type: XML", nameof(GeneralTaskSerializer));
            XmlSerializer x = new XmlSerializer(typeof(GeneralTaskSerializationObject));
            TextReader reader = new StreamReader(path);
            var seqObject = (GeneralTaskSerializationObject)x.Deserialize(reader);
            SeqLogger.Debug("Input readed!", nameof(GeneralTaskSerializer));
            return seqObject.ToGeneralTask();
        }
    }
}