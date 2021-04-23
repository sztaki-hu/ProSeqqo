using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Xml.Serialization;
using SequencePlanner.Helper;
using SequencePlanner.GeneralModels;

namespace SequencePlanner.GTSPTask.Serialization.Task
{
    public class NewGeneralTaskSerializer
    {
        public void ExportSEQ(NewGeneralTask task, string path)
        {
            SeqLogger.Debug("Output task type: General", nameof(NewGeneralTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(NewGeneralTaskSerializer));
            SeqLogger.Debug("Output type: SEQ", nameof(NewGeneralTaskSerializer));
            var ser = new NewGeneralTaskSerializationObject(task);
            var seqString = ser.ToSEQ();
            File.WriteAllText(path, seqString);
            SeqLogger.Debug("Output created!", nameof(NewGeneralTaskSerializer));
        }

        public NewGeneralTask ImportSEQ(string path)
        {
            SeqLogger.Debug("Input task type: PointLike", nameof(NewGeneralTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(NewGeneralTaskSerializer));
            SeqLogger.Debug("Input type: SEQ", nameof(NewGeneralTaskSerializer));
            var seqString = File.ReadAllLines(path).ToList();
            SeqLogger.Debug("Input readed!", nameof(NewGeneralTaskSerializer));
            var seqObject = new NewGeneralTaskSerializationObject(seqString);
            return seqObject.ToGeneralTask();
        }

        public void ExportJSON(NewGeneralTask task, string path)
        {
            SeqLogger.Debug("Output task type: GeneralTask", nameof(NewGeneralTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(NewGeneralTaskSerializer));
            SeqLogger.Debug("Output type: JSON", nameof(NewGeneralTaskSerializer));
            var seqObject = new NewGeneralTaskSerializationObject(task);
            var jsonString = JsonConvert.SerializeObject(seqObject);
            File.WriteAllText(path, jsonString);
            SeqLogger.Debug("Output created!", nameof(NewGeneralTaskSerializer));
        }

        public NewGeneralTask ImportJSON(string path)
        {
            SeqLogger.Debug("Input task type: GeneralTask", nameof(NewGeneralTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(NewGeneralTaskSerializer));
            SeqLogger.Debug("Input type: JSON", nameof(NewGeneralTaskSerializer));
            var jsonString = File.ReadAllText(path);
            var seqObject = JsonConvert.DeserializeObject<NewGeneralTaskSerializationObject>(jsonString);
            SeqLogger.Debug("Input readed!", nameof(NewGeneralTaskSerializer));
            return seqObject.ToGeneralTask();
        }

        public void ExportXML(NewGeneralTask task, string path)
        {
            SeqLogger.Debug("Output task type: GeneralTask", nameof(NewGeneralTaskSerializer));
            SeqLogger.Info("Output file: " + path, nameof(NewGeneralTaskSerializer));
            SeqLogger.Debug("Output type: XML", nameof(NewGeneralTaskSerializer));
            var seqObject = new NewGeneralTaskSerializationObject(task);
            XmlSerializer x = new XmlSerializer(typeof(NewGeneralTaskSerializationObject));
            TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, seqObject);
            writer.Close();
            SeqLogger.Debug("Output created!", nameof(NewGeneralTaskSerializer));
        }

        public NewGeneralTask ImportXML(string path)
        {
            SeqLogger.Debug("Input task type: GeneralTask", nameof(NewGeneralTaskSerializer));
            SeqLogger.Info("Input file: " + path, nameof(NewGeneralTaskSerializer));
            SeqLogger.Debug("Input type: XML", nameof(NewGeneralTaskSerializer));
            XmlSerializer x = new XmlSerializer(typeof(NewGeneralTaskSerializationObject));
            TextReader reader = new StreamReader(path);
            var seqObject = (NewGeneralTaskSerializationObject)x.Deserialize(reader);
            SeqLogger.Debug("Input readed!", nameof(NewGeneralTaskSerializer));
            return seqObject.ToGeneralTask();
        }
    }
}