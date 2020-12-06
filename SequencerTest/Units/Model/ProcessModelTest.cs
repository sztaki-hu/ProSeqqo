using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencerTest.Units.Model
{
    [TestClass]
    public class ProcessModelTest
    {
        [TestMethod()]
        public void ConstructorTest()
        {
            var process = new Process();
            var process2 = new Process();

            Assert.AreNotEqual(process.GlobalID, process2.GlobalID);
            Assert.AreNotEqual(process.Name, process2.Name);
            Assert.IsFalse(process.Virtual);
            Assert.AreEqual(process.GlobalID, process.UserID);
            Assert.AreEqual(-1, process.ResourceID);
            Assert.AreEqual((string)(process.UserID + "_Process_" + process.GlobalID), process.Name);
            Assert.AreEqual(0, process.Alternatives.Count);
        }

        [TestMethod()]
        public void GetterSetterTest()
        {
            var alternatives = new List<Alternative>() { new Alternative(), new Alternative(), new Alternative() };
            Process process = new Process()
            {
                UserID = 1,
                Name = "MyAlternative",
                Virtual = true,
                ResourceID = 2,
                Alternatives = alternatives
            };

            Assert.AreEqual(1, process.UserID);
            Assert.AreEqual("MyAlternative", process.Name);
            Assert.IsTrue(process.Virtual);
            Assert.AreEqual(2, process.ResourceID);
            Assert.AreEqual(alternatives, process.Alternatives);
        }
    }
}
