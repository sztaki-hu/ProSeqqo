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
            GTSPRepresentation gtsp = new GTSPRepresentation();

            gtsp.AddProcess(p);
            gtsp.AddProcess(p2);
            gtsp.AddAlternative(p, new Alternative[] { a, b, c });
            gtsp.AddAlternative(p2, new Alternative[] { a2, b2, c2 });
            gtsp.AddTask(a, t);
            gtsp.AddTask(a2, t2);
            gtsp.AddPosition(t, pos);
            gtsp.AddPosition(t2, pos2);

        }
    }
}
