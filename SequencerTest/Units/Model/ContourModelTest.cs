using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencerTest.Units.Model
{
    
    [TestClass]
    public class ContourModelTest
    {

        [TestMethod()]
        public void ConstructorTest()
        {
            var contour = new Contour();
            var contour2 = new Contour();

            Assert.AreNotEqual(contour.GlobalID, contour2.GlobalID);
            Assert.AreNotEqual(contour.Name, contour2.Name);
            Assert.IsFalse(contour.Virtual);
            Assert.AreEqual(contour.GlobalID, contour.UserID);
            Assert.AreEqual(-1, contour.ResourceID);
            Assert.AreEqual((string)(contour.UserID + "_Contour_" + contour.GlobalID), contour.Name );
            Assert.AreEqual(0, contour.Lines.Count);
        }

        [TestMethod()]
        public void GetterSetterTest()
        {
            var lines = new List<Line>() { new Line(), new Line(), new Line() };
            Contour contour = new Contour()
            {
                UserID = 1,
                Name = "MyContour",
                Virtual = true,
                ResourceID = 2,
                Lines = lines
            };

            Assert.AreEqual(1, contour.UserID);
            Assert.AreEqual("MyContour", contour.Name);
            Assert.IsTrue(contour.Virtual);
            Assert.AreEqual(2, contour.ResourceID);
            Assert.AreEqual(lines, contour.Lines);
        }
    }
}