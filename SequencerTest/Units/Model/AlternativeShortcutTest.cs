using Microsoft.VisualStudio.TestTools.UnitTesting;
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
