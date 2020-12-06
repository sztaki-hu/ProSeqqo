using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencerTest.Units.Model
{

    [TestClass]
    public class AlternativeModelTest
    {
        [TestMethod()]
        public void ConstructorTest()
        {
            var alternative = new Alternative();
            var alternative2 = new Alternative();

            Assert.AreNotEqual(alternative.GlobalID, alternative2.GlobalID);
            Assert.AreNotEqual(alternative.Name, alternative2.Name);
            Assert.IsFalse(alternative.Virtual);
            Assert.AreEqual(alternative.GlobalID, alternative.UserID);
            Assert.AreEqual(-1, alternative.ResourceID);
            Assert.AreEqual((string)(alternative.UserID + "_Alternative_" + alternative.GlobalID), alternative.Name);
            Assert.AreEqual(0, alternative.Tasks.Count);
        }

        [TestMethod()]
        public void GetterSetterTest()
        {
            var tasks = new List<Task>() { new Task(), new Task(), new Task() };
            Alternative alternative = new Alternative()
            {
                UserID = 1,
                Name = "MyAlternative",
                Virtual = true,
                ResourceID = 2,
                Tasks = tasks
            };

            Assert.AreEqual(1, alternative.UserID);
            Assert.AreEqual("MyAlternative", alternative.Name);
            Assert.IsTrue(alternative.Virtual);
            Assert.AreEqual(2, alternative.ResourceID);
            Assert.AreEqual(tasks, alternative.Tasks);
        }
    }
}
