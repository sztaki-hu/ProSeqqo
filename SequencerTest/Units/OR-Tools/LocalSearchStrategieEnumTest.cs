using Google.OrTools.ConstraintSolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.OR_Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencerTest.Units.OR_Tools
{
    [TestClass]
    public class LocalSearchStrategieEnumTest
    {
        [TestMethod]
        public void TestResolveString()
        {
            //AUTOMATIC
            LocalSearchStrategieEnum.Metaheuristics metaheuristics = LocalSearchStrategieEnum.ResolveEnum("Automatic");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.Automatic);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("AUTOMATIC");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.Automatic);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("automatic");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.Automatic);

            //GREEDYDESCENT
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("GreedyDescent");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.GreedyDescent);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("GREEDYDESCENT");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.GreedyDescent);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("greedydescent");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.GreedyDescent);

            //GuidedLocalSearch
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("GuidedLocalSearch");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.GuidedLocalSearch);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("GUIDEDLOCALSEARCH");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.GuidedLocalSearch);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("guidedlocalsearch");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.GuidedLocalSearch);

            //ObjectiveTabuSearch
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("ObjectiveTabuSearch");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.ObjectiveTabuSearch);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("OBJECTIVETABUSEARCH");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.ObjectiveTabuSearch);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("objectivetabusearch");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.ObjectiveTabuSearch);

            //SimulatedAnnealing
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("SimulatedAnnealing");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.SimulatedAnnealing);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("SIMULATEDANNEALING");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.SimulatedAnnealing);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("simulatedannealing");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.SimulatedAnnealing);

            //TabuSearch
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("TabuSearch");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.TabuSearch);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("TABUSEARCH");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.TabuSearch);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("tabusearch");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.TabuSearch);

            //DEFAULT
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.Automatic);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum(null);
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.Automatic);
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum("ASDF");
            Assert.AreEqual(metaheuristics, LocalSearchStrategieEnum.Metaheuristics.Automatic);
        }

        public void TestResolvingEnum()
        {
            //AUTOMATIC
            var metaheuristics = LocalSearchStrategieEnum.ResolveEnum(LocalSearchStrategieEnum.Metaheuristics.Automatic);
            Assert.AreEqual(metaheuristics,LocalSearchMetaheuristic.Types.Value.Automatic);

            //GREEDYDESCENT
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum(LocalSearchStrategieEnum.Metaheuristics.GreedyDescent);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

            //GuidedLocalSearch
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum(LocalSearchStrategieEnum.Metaheuristics.GuidedLocalSearch);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

            //ObjectiveTabuSearch
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum(LocalSearchStrategieEnum.Metaheuristics.ObjectiveTabuSearch);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

            //SimulatedAnnealing
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum(LocalSearchStrategieEnum.Metaheuristics.SimulatedAnnealing);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

            //TabuSearch
            metaheuristics = LocalSearchStrategieEnum.ResolveEnum(LocalSearchStrategieEnum.Metaheuristics.TabuSearch);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

        }
    }

}
