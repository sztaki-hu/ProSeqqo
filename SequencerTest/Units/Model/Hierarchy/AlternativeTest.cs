using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Model.Hierarchy;

namespace SequencerTest.Units.Model.Hierarchy
{
    [TestClass]
    public class AlternativeTest
    {
        [TestInitialize()]
        public void Initialize()
        {

        }

        [TestMethod()]
        public void ConstructFunciton()
        {
            Alternative alternative = new Alternative() {
                ID = 1,
                Name  = "Alternative",
                Virtual = true,
            };
            Assert.IsNotNull(alternative.GlobalID);
            Assert.AreEqual(1, alternative.ID);
            Assert.AreEqual("Alternative", alternative.Name);
            Assert.AreEqual(true, alternative.Virtual);

            Alternative alternative2 = new Alternative()
            {
                ID = 1,
                Name = "Alternative",
                Virtual = true,
            };
            Assert.AreNotEqual(alternative.GlobalID, alternative2.GlobalID);
        }
    }
}