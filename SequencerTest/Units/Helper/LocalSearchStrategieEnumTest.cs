using Google.OrTools.ConstraintSolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Helper;

namespace SequencerTest.Units.OR_Tools
{
    [TestClass]
    public class LocalSearchStrategyEnumTest
    {
        [TestMethod]
        public void TestResolveString()
        {
            //AUTOMATIC
            Metaheuristics metaheuristics = LocalSearchStrategyEnum.ResolveEnum("Automatic");
            Assert.AreEqual(metaheuristics, Metaheuristics.Automatic);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("AUTOMATIC");
            Assert.AreEqual(metaheuristics, Metaheuristics.Automatic);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("automatic");
            Assert.AreEqual(metaheuristics, Metaheuristics.Automatic);

            //GREEDYDESCENT
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("GreedyDescent");
            Assert.AreEqual(metaheuristics, Metaheuristics.GreedyDescent);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("GREEDYDESCENT");
            Assert.AreEqual(metaheuristics, Metaheuristics.GreedyDescent);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("greedydescent");
            Assert.AreEqual(metaheuristics, Metaheuristics.GreedyDescent);

            //GuidedLocalSearch
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("GuidedLocalSearch");
            Assert.AreEqual(metaheuristics, Metaheuristics.GuidedLocalSearch);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("GUIDEDLOCALSEARCH");
            Assert.AreEqual(metaheuristics, Metaheuristics.GuidedLocalSearch);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("guidedlocalsearch");
            Assert.AreEqual(metaheuristics, Metaheuristics.GuidedLocalSearch);

            //ObjectiveTabuSearch
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("ObjectiveTabuSearch");
            Assert.AreEqual(metaheuristics, Metaheuristics.ObjectiveTabuSearch);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("OBJECTIVETABUSEARCH");
            Assert.AreEqual(metaheuristics, Metaheuristics.ObjectiveTabuSearch);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("objectivetabusearch");
            Assert.AreEqual(metaheuristics, Metaheuristics.ObjectiveTabuSearch);

            //SimulatedAnnealing
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("SimulatedAnnealing");
            Assert.AreEqual(metaheuristics, Metaheuristics.SimulatedAnnealing);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("SIMULATEDANNEALING");
            Assert.AreEqual(metaheuristics, Metaheuristics.SimulatedAnnealing);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("simulatedannealing");
            Assert.AreEqual(metaheuristics, Metaheuristics.SimulatedAnnealing);

            //TabuSearch
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("TabuSearch");
            Assert.AreEqual(metaheuristics, Metaheuristics.TabuSearch);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("TABUSEARCH");
            Assert.AreEqual(metaheuristics, Metaheuristics.TabuSearch);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("tabusearch");
            Assert.AreEqual(metaheuristics, Metaheuristics.TabuSearch);

            //DEFAULT
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("");
            Assert.AreEqual(metaheuristics, Metaheuristics.Automatic);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum(null);
            Assert.AreEqual(metaheuristics, Metaheuristics.Automatic);
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum("ASDF");
            Assert.AreEqual(metaheuristics, Metaheuristics.Automatic);
        }

        public void TestResolvingEnum()
        {
            //AUTOMATIC
            var metaheuristics = LocalSearchStrategyEnum.ResolveEnum(Metaheuristics.Automatic);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

            //GREEDYDESCENT
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum(Metaheuristics.GreedyDescent);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

            //GuidedLocalSearch
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum(Metaheuristics.GuidedLocalSearch);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

            //ObjectiveTabuSearch
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum(Metaheuristics.ObjectiveTabuSearch);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

            //SimulatedAnnealing
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum(Metaheuristics.SimulatedAnnealing);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

            //TabuSearch
            metaheuristics = LocalSearchStrategyEnum.ResolveEnum(Metaheuristics.TabuSearch);
            Assert.AreEqual(metaheuristics, LocalSearchMetaheuristic.Types.Value.Automatic);

        }
    }

}
