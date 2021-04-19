using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Model;

namespace SequencerTest.Units.Model
{
    [TestClass]
    public class LineModelTest
    {
        Position A;
        Position B;

        [TestInitialize()]
        public void Initialize()
        {
            A = new Position();
            B = new Position();
        }

        [TestMethod()]
        public void ConstructorTest()
        {
            var line = new Line();
            var line2 = new Line();

            Assert.AreNotEqual(line.GlobalID, line2.GlobalID);
            Assert.AreNotEqual(line.Name, line2.Name);
            Assert.AreEqual(Line.BIDIRECTIONAL_DEFAULT, line.Bidirectional);
            Assert.IsNull(line.NodeA);
            Assert.IsNull(line.NodeB);
            Assert.IsFalse(line.Virtual);
            Assert.AreEqual(line.GlobalID, line.UserID);
            Assert.AreEqual(-1, line.ResourceID);
            Assert.AreEqual(line.Name, (string)(line.UserID + "_Line_" + line.GlobalID));
        }

        [TestMethod()]
        public void GetterSetterTest()
        {
            var vec = new double[] { 1, 2, 3 };
            Line line = new Line()
            {
                UserID = 1,
                Name = "MyLine",
                Virtual = true,
                ResourceID = 2,
                NodeA = A,
                NodeB = B,
                Bidirectional = true                
            };

            Assert.AreEqual(1, line.UserID);
            Assert.AreEqual("MyLine", line.Name);
            Assert.IsTrue(line.Virtual);
            Assert.AreEqual(2, line.ResourceID);
            Assert.AreEqual(A, line.NodeA);
            Assert.AreEqual(B, line.NodeB);
            Assert.IsTrue(line.Bidirectional);
        }
    }
}
