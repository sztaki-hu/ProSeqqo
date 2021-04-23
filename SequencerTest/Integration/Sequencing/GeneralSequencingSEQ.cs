using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GeneralModels;
using SequencePlanner.GTSPTask.Serialization.Task;
using System;

namespace SequencerTest.Integration.Sequencing
{
    [TestClass]
    public class GeneralSequencingSEQ
    {
        [TestInitialize()]
        public void Initialize()
        {

        }

        [TestMethod]
        public void Test1()
        {
            //NewGeneralTaskSerializer seq = new NewGeneralTaskSerializer();
            //NewGeneralTask task = seq.ImportSEQ("Resources/Example/PickAndPlace_Matrix.txt");
            //var result = task.Run();
            //seq = new NewGeneralTaskSerializer();
            //seq.ExportSEQ(task, "Resources/Export/PickAndPlace_Matrix.txt");
            //Console.WriteLine(result);

        }

        [TestMethod]
        public void Test2()
        {
            //NewGeneralTaskSerializer seq = new NewGeneralTaskSerializer();
            //NewGeneralTask task = seq.ImportSEQ("Resources/Example/Kocka.txt");
            //var result = task.Run();
            //seq = new NewGeneralTaskSerializer();
            //seq.ExportSEQ(task, "Resources/Export/Kocka.txt");
            //Console.WriteLine(result);
        }

        [TestMethod]
        public void Test3()
        {
            //GeneralTaskSerializer seq = new GeneralTaskSerializer();
            //GeneralTask task = seq.ImportSEQ("Resources/Example/PickAndPlace_Original.txt");
            //var result = task.RunModel();
            //seq = new GeneralTaskSerializer();
            //seq.ExportSEQ(task, "Resources/Export/PickAndPlace_Original.txt");
            //Console.WriteLine(result);
        }
    }
}
