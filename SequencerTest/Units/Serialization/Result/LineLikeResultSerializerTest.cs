using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.Result;

namespace SequencerTest.Units.Serialization.Result
{
    [TestClass]
    public class LineLikeResultSerializerTest
    {
        LineLikeResultSerializer serializer;
        LineTaskResult result;

        [TestInitialize()]
        public void Initialize()
        {
            serializer = new LineLikeResultSerializer();
            result = new LineTaskResult();
        }

        [TestMethod]
        public void ExportSEQ()
        {
            serializer.ExportSEQ(result, "Resources/Out/LineLikeResultSerializerTest_ExportSEQ.seq");
        }

        [TestMethod]
        public void ExportJSON()
        {
            serializer.ExportJSON(result, "Resources/Out/LineLikeResultSerializerTest_ExportJSON.json");
        }

        [TestMethod]
        public void ExportXML()
        {
            serializer.ExportXML(result, "Resources/Out/LineLikeResultSerializerTest_ExportXML.xml");
        }

        //[TestMethod]
        //public void ImportSEQTest()
        //{
        //    serializer.ImportSEQ("Resources/Out/LineLikeResultSerializerTest_ExportSEQ.seq");
        //}

        //[TestMethod]
        //public void ImportXMLTest()
        //{
        //    serializer.ImportXML("Resources/Out/LineLikeResultSerializerTest_ExportXML.xml");
        //}

        [TestMethod]
        public void ImportJSONTest()
        {
            serializer.ImportJSON("Resources/Out/LineLikeResultSerializerTest_ExportJSON.json");
        }
    }
}