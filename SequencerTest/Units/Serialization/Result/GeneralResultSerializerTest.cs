using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.Result;
using SequencerTest.Units.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencerTest.Units.Serialization.Result
{

    [TestClass]
    public class GeneralResultSerializerTest
    {
        GeneralResultSerializer serializer;
        GeneralTaskResult result;

        [TestInitialize()]
        public void Initialize()
        {
            serializer = new GeneralResultSerializer();
            result = new GeneralTaskResult();
        }

        [TestMethod]
        public void ExportSEQ()
        {
            serializer.ExportSEQ(result, "Resources/Out/PointLikeResultSerializerTest_ExportSEQ.seq");
        }

        [TestMethod]
        public void ExportJSON()
        {
            serializer.ExportJSON(result, "Resources/Out/PointLikeResultSerializerTest_ExportJSON.json");
        }

        [TestMethod]
        public void ExportXML()
        {
            serializer.ExportXML(result, "Resources/Out/PointLikeResultSerializerTest_ExportXML.xml");
        }

        //[TestMethod]
        //public void ImportSEQTest()
        //{
        //    serializer.ImportSEQ("Resources/Out/PointLikeResultSerializerTest_ExportSEQ.seq");
        //}

        [TestMethod]
        public void ImportXMLTest()
        {
            serializer.ImportXML("Resources/Out/PointLikeResultSerializerTest_ExportXML.xml");
        }

        [TestMethod]
        public void ImportJSONTest()
        {
            serializer.ImportJSON("Resources/Out/PointLikeResultSerializerTest_ExportJSON.json");
        }
    }
}