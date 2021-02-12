using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSPTask.Serialization.Task;
using SequencePlanner.GTSPTask.Task.LineLike;
using System;

namespace SequencerTest.Integration.Sequencing
{
    [TestClass]
    public class LineLikeSequencingSEQ
    {
        [TestInitialize()]
        public void Initialize()
        {

        }

        [TestMethod]
        public void Test1()
        {
            LineLikeTaskSerializer seq = new LineLikeTaskSerializer();
            LineLikeTask task = seq.ImportSEQ("Resources/LineLike_Matrix.txt");
            task.ValidateModel();
            var result = task.RunModel();
            seq = new LineLikeTaskSerializer();
            seq.ExportSEQ(task, "Resources/Export/LineLike_Matrix.txt");
            Console.WriteLine(result);

        }

        [TestMethod]
        public void Test2()
        {
            LineLikeTaskSerializer seq = new LineLikeTaskSerializer();
            LineLikeTask task = seq.ImportSEQ("Resources/LineLike_Original.txt");
            var result = task.RunModel();
            seq = new LineLikeTaskSerializer();
            seq.ExportSEQ(task, "Resources/Export/LineLike_Original.txt");
            Console.WriteLine(result);
        }

        [TestMethod]
        public void Test3()
        {
            LineLikeTaskSerializer seq = new LineLikeTaskSerializer();
            LineLikeTask task = seq.ImportSEQ("Resources/CSOPA.txt");
            var result = task.RunModel();
            seq = new LineLikeTaskSerializer();
            seq.ExportSEQ(task, "Resources/Export/CSOPA.txt");
            Console.WriteLine(result);
        }
    }
}
