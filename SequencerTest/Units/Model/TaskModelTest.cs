using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencerTest.Units.Model
{
    [TestClass]
    public class TaskModelTest
    {
        [TestMethod()]
        public void ConstructorTest()
        {
            var task = new Task();
            var task2 = new Task();

            Assert.AreNotEqual(task.GlobalID, task2.GlobalID);
            Assert.AreNotEqual(task.Name, task2.Name);
            Assert.IsFalse(task.Virtual);
            Assert.AreEqual(task.GlobalID, task.UserID);
            Assert.AreEqual(-1, task.ResourceID);
            Assert.AreEqual((string)(task.UserID + "_Task_" + task.GlobalID), task.Name);
            Assert.AreEqual(0, task.Positions.Count);
        }

        [TestMethod()]
        public void GetterSetterTest()
        {
            var positions = new List<Position>() { new Position(), new Position(), new Position() };
            Task task = new Task()
            {
                UserID = 1,
                Name = "MyTask",
                Virtual = true,
                ResourceID = 2,
                Positions = positions
            };

            Assert.AreEqual(1, task.UserID);
            Assert.AreEqual("MyTask", task.Name);
            Assert.IsTrue(task.Virtual);
            Assert.AreEqual(2, task.ResourceID);
            Assert.AreEqual(positions, task.Positions);
        }
    }
}
