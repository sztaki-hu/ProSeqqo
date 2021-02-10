using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencerTest.Units.Serialization.Result
{

    [TestClass]
    public class LineLikeResultSerializerTest
    {
        LineTaskResult result;

        [TestInitialize()]
        public void Initialize()
        {
            
        }

        [TestMethod]
        public void GetSet()
        {
            LineLikeResultSerializer lineLikeResultSerializer = new LineLikeResultSerializer();
            //lineLikeResultSerializer.ExportSEQ("Resources/Out/LineLikeResultSerializerTest_ExportSEQ.seq");
        }


        [TestMethod]
        public void ExportSEQ()
        {
            LineLikeResultSerializer lineLikeResultSerializer = new LineLikeResultSerializer();
            //lineLikeResultSerializer.ExportSEQ("Resources/Out/LineLikeResultSerializerTest_ExportSEQ.seq");
        }

        [TestMethod]
        public void ImportExportJSONTest()
        {


        }

        [TestMethod]
        public void ImportExportXMLTest()
        {



        }
    }
}