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
    public class LocalSearchStrategyEnumTest
    {
        [TestMethod]
        public void TestResolveString()
        {
            //AUTOMATIC
            LocalSearchStrategyEnum.Metaheuristics metaheuristics = LocalSearchStrategyEnum.ResolveEnum("Automatic");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.Automatic);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("AUTOMATIC");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.Automatic);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("automatic");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.Automatic);

            //GREEDYDESCENT
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("GreedyDescent");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.GreedyDescent);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("GREEDYDESCENT");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.GreedyDescent);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("greedydescent");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.GreedyDescent);

            //GuidedLocalSearch
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("GuidedLocalSearch");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.GuidedLocalSearch);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("GUIDEDLOCALSEARCH");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.GuidedLocalSearch);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("guidedlocalsearch");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.GuidedLocalSearch);

            //ObjectiveTabuSearch
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("ObjectiveTabuSearch");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.ObjectiveTabuSearch);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("OBJECTIVETABUSEARCH");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.ObjectiveTabuSearch);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("objectivetabusearch");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.ObjectiveTabuSearch);

            //SimulatedAnnealing
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("SimulatedAnnealing");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.SimulatedAnnealing);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("SIMULATEDANNEALING");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.SimulatedAnnealing);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("simulatedannealing");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.SimulatedAnnealing);

            //TabuSearch
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("TabuSearch");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.TabuSearch);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("TABUSEARCH");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.TabuSearch);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("tabusearch");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.TabuSearch);

            //DEFAULT
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.Automatic);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum(null);
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.Automatic);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("ASDF");
            Assert.AreEqual(metaheuristics, LocalSearchStrategyEnum.Metaheuristics.Automatic);
        }

        public void TestResolvingEnum()
        {
            //AUTOMATIC
            var metaheuristics = LocalSearchStrategyEnum.ResolveEnum(LocalSearchStrategyEnum.Metaheuristics.Automatic);
            Assert.AreEqual(metaheuristics,LocalSearchMetaheuristic.Types.Value.Automatic);

            //GREEDYDESCENT
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum(LocalSearchStrategyEnum.Metaheuristics.GreedyDescent);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

            //GuidedLocalSearch
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum(LocalSearchStrategyEnum.Metaheuristics.GuidedLocalSearch);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

            //ObjectiveTabuSearch
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum(LocalSearchStrategyEnum.Metaheuristics.ObjectiveTabuSearch);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

            //SimulatedAnnealing
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum(LocalSearchStrategyEnum.Metaheuristics.SimulatedAnnealing);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

            //TabuSearch
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum(LocalSearchStrategyEnum.Metaheuristics.TabuSearch);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

        }
    }

}
