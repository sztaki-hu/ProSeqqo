using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSPTask.Serialization.Task;
using SequencePlanner.GTSPTask.Task.LineTask;
using System;

namespace SequencerTest.Integration.Sequencing
{
    [TestClass]
    public class LineSequencingSEQ
    {
        [TestInitialize()]
        public void Initialize()
        {

        }

        [TestMethod]
        public void Test1()
        {
            LineTaskSerializer seq = new LineTaskSerializer();
            LineTask task = seq.ImportSEQ("Resources/Example/Line_Matrix.txt");
            task.ValidateModel();
            var result = task.RunModel();
            seq = new LineTaskSerializer();
            seq.ExportSEQ(task, "Resources/Export/Line_Matrix.txt");
            Console.WriteLine(result);

        }

        [TestMethod]
        public void Test2()
        {
            LineTaskSerializer seq = new LineTaskSerializer();
            LineTask task = seq.ImportSEQ("Resources/Example/Line_Original.txt");
            var result = task.RunModel();
            seq = new LineTaskSerializer();
            seq.ExportSEQ(task, "Resources/Export/Line_Original.txt");
            Console.WriteLine(result);
        }

        [TestMethod]
        public void Test3()
        {
            LineTaskSerializer seq = new LineTaskSerializer();
            LineTask task = seq.ImportSEQ("Resources/Example/CSOPA.txt");
            var result = task.RunModel();
            seq = new LineTaskSerializer();
            seq.ExportSEQ(task, "Resources/Export/CSOPA.txt");
            Console.WriteLine(result);
        }
    }
}
