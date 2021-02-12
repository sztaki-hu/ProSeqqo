using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SequencerTest.Units.Model
{
    [TestClass]
    public class PositionModelTest
    {

        [TestInitialize()]
        public void Initialize()
        {
            
        }

        [TestMethod()]
        public void ConstructorTest()
        {
            Position position = new Position();
            Position position2 = new Position();

            Assert.AreNotEqual(position.GlobalID, position2.GlobalID);
            Assert.AreNotEqual(position.Name, position2.Name);
            Assert.AreEqual(0, position.Dimension);
            Assert.IsFalse(position.Virtual);
            Assert.AreEqual(position.GlobalID, position.UserID);
            Assert.AreEqual(-1, position.ResourceID);
            Assert.AreEqual(position.Name, (string) (position.UserID+"_Position_"+position.GlobalID));
            Assert.AreEqual(0, position.Vector.Length);
        }

        [TestMethod()]
        public void GetterSetterTest()
        {
            var vec = new double[] { 1, 2, 3 };
            Position position = new Position()
            {
                UserID = 1,
                Name = "MyPosition",
                Virtual = true,
                ResourceID = 2,
                Vector = vec
            };

            Assert.AreEqual("MyPosition", position.Name);
            Assert.AreEqual(1, position.UserID);
            Assert.IsTrue(position.Virtual);
            Assert.AreEqual(2, position.ResourceID);
            Assert.AreEqual(vec, position.Vector);
        }
    }
}