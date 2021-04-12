using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.Result;

namespace SequencerTest.Units.Serialization.Result
{
    [TestClass]
    public class LineResultSerializerTest
    {
        LineResultSerializer serializer;
        LineTaskResult result;

        [TestInitialize()]
        public void Initialize()
        {
            serializer = new LineResultSerializer();
            result = new LineTaskResult();
        }

        [TestMethod]
        public void ExportSEQ()
        {
            serializer.ExportSEQ(result, "Resources/Out/LineResultSerializerTest_ExportSEQ.seq");
        }

        [TestMethod]
        public void ExportJSON()
        {
            serializer.ExportJSON(result, "Resources/Out/LineResultSerializerTest_ExportJSON.json");
        }

        [TestMethod]
        public void ExportXML()
        {
            serializer.ExportXML(result, "Resources/Out/LineResultSerializerTest_ExportXML.xml");
        }

        //[TestMethod]
        //public void ImportSEQTest()
        //{
        //    serializer.ImportSEQ("Resources/Out/LineResultSerializerTest_ExportSEQ.seq");
        //}

        //[TestMethod]
        //public void ImportXMLTest()
        //{
        //    serializer.ImportXML("Resources/Out/LineResultSerializerTest_ExportXML.xml");
        //}

        [TestMethod]
        public void ImportJSONTest()
        {
            serializer.ImportJSON("Resources/Out/LineResultSerializerTest_ExportJSON.json");
        }
    }
}