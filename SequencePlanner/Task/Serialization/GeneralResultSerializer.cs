using Newtonsoft.Json;
using SequencePlanner.Helper;
using SequencePlanner.Task;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    public class GeneralResultSerializer
    {
        public void ExportSEQ(GeneralTaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: General", nameof(GeneralResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(GeneralResultSerializer));
            SeqLogger.Info("Output type: SEQ", nameof(GeneralResultSerializer));
            var ser = new GeneralResultSerializationObject(result);
            var seqString = ser.ToSEQ();
            File.WriteAllText(path, seqString);
            SeqLogger.Debug("Output created!", nameof(GeneralResultSerializer));
        }

        public GeneralTaskResult ImportSEQ(string path)
        {
            SeqLogger.Debug("Input task type: General", nameof(GeneralResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(GeneralResultSerializer));
            SeqLogger.Info("Input type: SEQ", nameof(GeneralResultSerializer));
            var seqString = File.ReadAllLines(path).ToList();
            var seqObject = new GeneralResultSerializationObject(seqString);
            SeqLogger.Debug("Input readed!", nameof(GeneralResultSerializer));
            return seqObject.ToGeneralResult();
        }

        public void ExportJSON(GeneralTaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: General", nameof(GeneralResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(GeneralResultSerializer));
            SeqLogger.Info("Output type: JSON", nameof(GeneralResultSerializer));
            var seqObject = new GeneralResultSerializationObject(result);
            var jsonString = JsonConvert.SerializeObject(seqObject);
            File.WriteAllText(path, jsonString);
            SeqLogger.Debug("Output created!", nameof(GeneralResultSerializer));
        }

        public GeneralTaskResult ImportJSON(string path)
        {
            SeqLogger.Debug("Input task type: General", nameof(GeneralResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(GeneralResultSerializer));
            SeqLogger.Info("Input type: JSON", nameof(GeneralResultSerializer));
            var jsonString = File.ReadAllText(path);
            var seqObject = JsonConvert.DeserializeObject<GeneralResultSerializationObject>(jsonString);
            SeqLogger.Debug("Input readed!", nameof(GeneralResultSerializer));
            return seqObject.ToGeneralResult();
        }

        public void ExportXML(GeneralTaskResult result, string path)
        {
            SeqLogger.Debug("Output task type: General", nameof(GeneralResultSerializer));
            SeqLogger.Info("Output file: " + path, nameof(GeneralResultSerializer));
            SeqLogger.Info("Output type: XML", nameof(GeneralResultSerializer));
            var seqObject = new GeneralResultSerializationObject(result);
            XmlSerializer x = new XmlSerializer(typeof(GeneralResultSerializationObject));
            TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, seqObject);
            writer.Close();
            SeqLogger.Debug("Output created!", nameof(GeneralResultSerializer));
        }

        public GeneralTaskResult ImportXML(string path)
        {
            SeqLogger.Debug("Input task type: PointLike", nameof(GeneralResultSerializer));
            SeqLogger.Info("Input file: " + path, nameof(GeneralResultSerializer));
            SeqLogger.Info("Input type: XML", nameof(GeneralResultSerializer));
            XmlSerializer x = new XmlSerializer(typeof(GeneralResultSerializationObject));
            TextReader reader = new StreamReader(path);
            var seqObject = (GeneralResultSerializationObject)x.Deserialize(reader);
            SeqLogger.Debug("Input readed!", nameof(GeneralResultSerializer));
            return seqObject.ToGeneralResult();
        }
    }
}