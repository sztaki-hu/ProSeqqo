﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSPTask.Serialization.Task;
using SequencePlanner.GTSPTask.Task.PointLike;
using System;

namespace SequencerTest.Integration.Sequencing
{
    [TestClass]
    public class PointLikeSequencingSEQ
    {
        [TestInitialize()]
        public void Initialize()
        {

        }

        [TestMethod]
        public void Test1()
        {
            PointLikeTaskSerializer seq = new PointLikeTaskSerializer();
            PointLikeTask task = seq.ImportSEQ("Resources/PickAndPlace_Matrix.txt");
            task.ValidateModel();
            var result = task.RunModel();
            seq = new PointLikeTaskSerializer();
            seq.ExportSEQ(task, "Resources/Export/PickAndPlace_Matrix.txt");
            Console.WriteLine(result);

        }

        [TestMethod]
        public void Test2()
        {
            PointLikeTaskSerializer seq = new PointLikeTaskSerializer();
            PointLikeTask task = seq.ImportSEQ("Resources/Kocka.txt");
            var result = task.RunModel();
            seq = new PointLikeTaskSerializer();
            seq.ExportSEQ(task, "Resources/Export/Kocka.txt");
            Console.WriteLine(result);
        }

        [TestMethod]
        public void Test3()
        {
            PointLikeTaskSerializer seq = new PointLikeTaskSerializer();
            PointLikeTask task = seq.ImportSEQ("Resources/PickAndPlace_Original.txt");
            var result = task.RunModel();
            seq = new PointLikeTaskSerializer();
            seq.ExportSEQ(task, "Resources/Export/PickAndPlace_Original.txt");
            Console.WriteLine(result);
        }
    }
}
