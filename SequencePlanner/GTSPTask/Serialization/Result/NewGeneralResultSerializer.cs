using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using SequencePlanner.Helper;
using SequencePlanner.GeneralModels.Result;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    public class NewGeneralResultSerializer
    {
        public void ExportSEQ(TaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: PointLike", nameof(NewGeneralResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(NewGeneralResultSerializer));
            SeqLogger.Debug("Output type: SEQ", nameof(NewGeneralResultSerializer));
            var ser = new NewGeneralResultSerializationObject(result);
            var seqString = ser.ToSEQ();
            File.WriteAllText(path, seqString);
            SeqLogger.Debug("Output created!", nameof(NewGeneralResultSerializer));
        }

        public TaskResult ImportSEQ(string path)
        {
            SeqLogger.Debug("Input task type: PointLike", nameof(NewGeneralResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(NewGeneralResultSerializer));
            SeqLogger.Debug("Input type: SEQ", nameof(NewGeneralResultSerializer));
            var seqString = File.ReadAllLines(path).ToList();
            var seqObject = new NewGeneralResultSerializationObject(seqString);
            SeqLogger.Debug("Input readed!", nameof(NewGeneralResultSerializer));
            return seqObject.ToGeneralResult();
        }

        public void ExportJSON(TaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: PointLike", nameof(NewGeneralResultSerializer));
            SeqLogger.Debug("Output file: " + path, nameof(NewGeneralResultSerializer));
            SeqLogger.Info("Output type: JSON", nameof(NewGeneralResultSerializer));
            var seqObject = new NewGeneralResultSerializationObject(result);
            var jsonString = JsonConvert.SerializeObject(seqObject);
            File.WriteAllText(path, jsonString);
            SeqLogger.Debug("Output created!", nameof(NewGeneralResultSerializer));
        }

        public TaskResult ImportJSON(string path)
        {
            SeqLogger.Debug("Input task type: PointLike", nameof(NewGeneralResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(NewGeneralResultSerializer));
            SeqLogger.Debug("Input type: JSON", nameof(NewGeneralResultSerializer));
            var jsonString = File.ReadAllText(path);
            var seqObject = JsonConvert.DeserializeObject<NewGeneralResultSerializationObject>(jsonString);
            SeqLogger.Debug("Input readed!", nameof(NewGeneralResultSerializer));
            return seqObject.ToGeneralResult();
        }

        public void ExportXML(TaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: PointLike", nameof(NewGeneralResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(NewGeneralResultSerializer));
            SeqLogger.Debug("Output type: XML", nameof(NewGeneralResultSerializer));
            var seqObject = new NewGeneralResultSerializationObject(result);
            XmlSerializer x = new XmlSerializer(typeof(NewGeneralResultSerializationObject));
            TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, seqObject);
            writer.Close();
            SeqLogger.Debug("Output created!", nameof(NewGeneralResultSerializer));
        }

        public TaskResult ImportXML(string path)
        {
            SeqLogger.Debug("Input task type: PointLike", nameof(NewGeneralResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(NewGeneralResultSerializer));
            SeqLogger.Debug("Input type: XML", nameof(NewGeneralResultSerializer));
            XmlSerializer x = new XmlSerializer(typeof(NewGeneralResultSerializationObject));
            TextReader reader = new StreamReader(path);
            var seqObject = (NewGeneralResultSerializationObject)x.Deserialize(reader);
            SeqLogger.Debug("Input readed!", nameof(NewGeneralResultSerializer));
            return seqObject.ToGeneralResult();
        }
    }
}