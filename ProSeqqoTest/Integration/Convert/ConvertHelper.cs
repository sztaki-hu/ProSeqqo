using ProSeqqoLib.GTSPTask.Serialization.Task;
using ProSeqqoLib.Helper;
using ProSeqqoLib.Task;
using SequencerTest.Helper;

namespace SequencerTest.Integration.Convert
{
    public class ConvertHelper
    {
        public void AssertSerialization(string input, string inputType, string output, string outputType)
        {
            GeneralTaskSerializer seq = new GeneralTaskSerializer();
            GeneralTask task = null;
            switch (inputType)
            {
                case "json":
                    task = seq.ImportJSON(input);
                    break;
                case "xml":
                    task = seq.ImportXML(input);
                    break;
                case "seq":
                case "txt":
                    task = seq.ImportSEQ(input);
                    break;
                default:
                    throw new SeqException("Unkown type!");
            }

            switch (outputType)
            {
                case "json":
                    seq.ExportJSON(task, output);
                    break;
                case "xml":
                    seq.ExportXML(task, output);
                    break;
                case "seq":
                case "txt":
                    seq.ExportSEQ(task, output);
                    break;
                default:
                    throw new SeqException("Unkown type!");
            }

            GeneralTask task2 = null;
            switch (outputType)
            {
                case "json":
                    task2 = seq.ImportJSON(output);
                    break;
                case "xml":
                    task2 = seq.ImportXML(output);
                    break;
                case "seq":
                case "txt":
                    task2 = seq.ImportSEQ(output);
                    break;
                default:
                    throw new SeqException("Unkown type!");
            }
            AssertGeneralTask.AssertTasks(task, task2);
            AssertRun(task, task2);
        }

        public void AssertRun(GeneralTask t, GeneralTask t2)
        {
            var result1 = t.Run();
            var result2 = t2.Run();
            AssertGeneralResult.AssertResults(result1, result2);
            AssertGeneralTask.AssertTasks(t, t2);
        }
    }
}