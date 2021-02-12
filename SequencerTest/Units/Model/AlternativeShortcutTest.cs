﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.Model;
using SequencerTest.Units.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencerTest.Units.Model
{
    [TestClass]
    public class AlternativeShortcutTest
    {
        Alternative alternative = new Alternative();
        EuclidianDistanceFunction distanceFunction = new EuclidianDistanceFunction();
        NoResourceFunction noResource = new NoResourceFunction();
        AlternativeShortcut shotcut = new AlternativeShortcut();

        [TestInitialize()]
        public void Initialize()
        {
            alternative = TestObjects.GetAlternative();
        }

        [TestMethod]
        public void GetSet()
        {
            shotcut = new AlternativeShortcut()
            { 
                UserID = 80,
                ResourceID = 81,
                SequencingID = 82,
                Name = "Cut",
                Virtual = false
            };

            Assert.AreEqual(shotcut.UserID, 80);
            Assert.AreEqual(shotcut.ResourceID, 81);
            Assert.AreEqual(shotcut.SequencingID, 82);
            Assert.AreEqual(shotcut.Name, "Cut");
            Assert.IsFalse(shotcut.Virtual);

            Assert.IsNull(shotcut.FrontProxy);
            Assert.IsNull(shotcut.BackProxy);
            Assert.IsNull(shotcut.Original);
            Assert.IsNotNull(shotcut.CriticalPaths);
            Assert.IsNotNull(shotcut.StrictSystemEdgeWeightSet);


        }

        [TestMethod]
        public void EmptyCall()
        {
            var alternative = new AlternativeShortcut();
            alternative.CreateShortcut(distanceFunction, noResource);
            alternative.FindPrecedenceHeaderOfPositions(new SequencePlanner.GTSP.GTSPPrecedenceConstraint(TestObjects.GetPosA(), TestObjects.GetPosB()));
            PointTaskResult pointTaskResult = new PointTaskResult();
            alternative.ResolveSolution(pointTaskResult);
            alternative.Assimilate(new Alternative());
        }
    }
}
