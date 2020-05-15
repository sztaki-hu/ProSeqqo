using SequencePlanner.GTSP;
using System;
using Xunit;

namespace SequenceUnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void GraphRepresentation()
        {
            Process p = new Process();
            Process p2 = new Process();

            Alternative a = new Alternative();
            Alternative a2 = new Alternative();
            Alternative b = new Alternative();
            Alternative b2 = new Alternative();
            Alternative c = new Alternative();
            Alternative c2 = new Alternative();

            Task t = new Task();
            Task t2 = new Task();

            Position pos = new Position();
            Position pos2 = new Position();
            GraphRepresentation graph = new GraphRepresentation();

            graph.addProcess(p);
            graph.addProcess(p2);
            graph.addAlternative(p, new Alternative[] { a, b, c });
            graph.addAlternative(p2, new Alternative[] { a2, b2, c2 });
            graph.addTask(a, t);
            graph.addTask(a2, t2);
            graph.addPosition(t, pos);
            graph.addPosition(t2, pos2);
            graph.createEdgesVirtual();

            graph.WriteGraph();

        }
    }
}
