using SequencePlanner.GeneralModels;
using SequencePlanner.GTSPTask.Task.General;

namespace SequencerTest.Units
{
    public class BuildByCodeAPI
    {

    //    public BuildTask()
    //    {
    //        var task = new GeneralTask();
    //        task.Validate = true;
    //        task.Cyclic = true;
    //        task.StartDepot = new Motion();
    //        task.FinishDepot = new Motion();
    //        task.UseMIPprecedenceSolver = true;
    //        task.UseShortcutInAlternatives = true;
    //        task.BidirectionalMotionDefault = true;

    //        task.DistanceFunction = new EucledianDistanceFunction(List<Motion> motions);
    //        task.DistanceFunction = new MaxDistanceFunction(List <Motion> motions);
    //        task.DistanceFunction = new ManhattanDistanceFunction(List <Motion> motions);
    //        task.DistanceFunction = new MatrixDistanceFunction(new ConfigMatrix  );
    //        task.DistanceFunction = new TrapezoidTimeDistanceFunction(new double[], new double[]);
    //        task.DistanceFunction = new TrapezoidTimeWithTimeBreakerDistanceFunction(new double [], new double[]);
    //        task.DistanceFunction.FunctionName
    //        task.DistanceFunction.CostMultiplyer
    //        task.DistanceFunction.IdlePenalty
    //        task.DistanceFunction.OverrideCosts = new List<>();

            
    //        var resource = new Resource(int ResourceID);
    //        task.ResourceChangeover = new NoResourceChangeover();
    //        task.ResourceChangeover = new ConstantResourceChangeover(double cost, new AddResourceChangeoverFunction());
    //        task.ResourceChangeover = new ConstantResourceChangeover(List<Resource> resources, double cost, new AddResourceChangeoverFunction());
    //        task.ResourceChangeover = new ConstantResourceChangeover(double cost, new MaxResourceChangeoverFunction());
    //        task.ResourceChangeover = new ConstantResourceChangeover(List<Resource> resources, double cost, new MaxResourceChangeoverFunction());
    //        task.ResourceChangeover = new MatrixResourceChangeover(int [] header, double[][],  new AddResourceChangeoverFunction());
    //        task.ResourceChangeover = new MatrixResourceChangeover(List<Resource>, double[][], new MaxResourceChangeoverFunction());

    //        task.SolverSettings = new SolverSettings(LocalSearchStrategy strategy, int timelimit);

    //        task.Configs = new List<Config>();
    //        task.Motions = new List<Motion>();
    //        task.ProcessPrecedences = new List<Precedence<Process>>();
    //        task.MotionPrecedences = new List<Precedence<Motion>>();


    //        var c = new Config(int userID, double[], string name, Resource resource, );
    //        var c = new Config(int userID, List<double>, string name, Resource resource, );
    //        var motionC = new Motion(int userID, Config c, string name, bool bidirectional, double length);
    //        var motionL = new Motion(int userID, Config c1, Config c2, string name, bool bidirectional, double length);
    //        var motionLL = new Motion(int userID, List < Config > configs, string name, bool bidirectional, double length);

    //        var proc = new Process(int userID);
    //        var alternative = new Alternatice(int userID);
    //        var task = new Task(int userID);
    //        var hierarchy = new HierarchyRecord(Process p, Alternative a, Task t, List < Motion > motions);
    //        var hierarchy2 = new HierarchyRecord(Process p, Alternative a, Task t, Motion motion);
    //        var hierarchy2 = new HierarchyRecord(int ProcessUserID, int AlternativeID, int TaskID, Motion motion);

    //        task.Hierarchy = new Hierarchy(List < HierarchyRecord > records);

    //        task.ProcessPrecedences = List<ProcessPrecedence>();
    //        task.MotionPrecedences = List<MotionPrecedence>();
    //        task.OverrideCost = List<Cost<Config>>();

    //        var procPrec = new Precedence<Process>(Process before, Process after);
    //        var motionPrec = new Precedence<Motion>(Motion before, Motion after);
    //        //var costOverride = new Cost<Motion>(Motion a, Motion b, double cost, bool bidirectional);
    //        var configOverride = new Cost<Config>(Config a, Config b, double cost, bool bidirectional);
    //        var disjointSet = disjointSet(List < Motion > motions);
    //    }
    }
}