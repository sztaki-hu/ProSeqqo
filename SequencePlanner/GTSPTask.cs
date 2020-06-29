using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Template;
using System.Collections.Generic;

namespace SequencePlanner
{
    public abstract class GTSPTask
    {
        public static bool DEBUG = false;

        protected bool Built { get; set; }
        protected ORToolsWrapper ORtool { get; set; }
        public List<Position> Solution { get; private set; }
        public List<Position> CleanSolution { get; private set; }
        public GTSPRepresentation GTSP { get; set; }

        public GTSPTask(){}

        public abstract void Templateing(Template template);

        public abstract void Validate();

        public abstract void Build();

        public abstract ORToolsResult Run();
    }
}