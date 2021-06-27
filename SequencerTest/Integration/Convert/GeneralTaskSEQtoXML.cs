using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSPTask.Serialization.Task;
using SequencePlanner.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencerTest.Integration.Convert
{
    [TestClass]
    public class GeneralTaskSEQtoXML
    {
        [TestInitialize()]
        public void Initialize()
        {

        }

        [TestMethod]
        public void GetterSetter()
        {
            GeneralTaskSerializer seq = new GeneralTaskSerializer();
            GeneralTask task = seq.ImportSEQ("Resources/HelloWorld/HelloWorld.seq");
            var result = task.Run();
            seq = new GeneralTaskSerializer();
            seq.ExportSEQ(task, "Resources/HelloWorld/Out/HelloWorld.xml");

        }
    }
}
