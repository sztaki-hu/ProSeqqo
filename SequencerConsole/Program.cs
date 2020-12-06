
namespace SequencerConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            CommandLineProcessor.CLI(args);
        }
    }
}


    //static void Main(string[] args)
    //{
    //    Console.WriteLine("Hello World!");
    //    var s = new ConsoleSpinner();
    //    ColorfulAnimation();

    //    while (true)
    //    {
    //        Thread.Sleep(100); // simulate some work being done
    //        s.UpdateProgress();
    //    }
    //}

    //static void ColorfulAnimation()
    //{
    //    for (int i = 0; i < 5; i++)
    //    {
    //        for (int j = 0; j < 30; j++)
    //        {
    //            Console.Clear();

    //            // steam
    //            Console.Write("       . . . . o o o o o o", Color.LightGray);
    //            for (int s = 0; s < j / 2; s++)
    //            {
    //                Console.Write(" o", Color.LightGray);
    //            }
    //            Console.WriteLine();

    //            var margin = "".PadLeft(j);
    //            Console.WriteLine(margin + "                _____      o", Color.LightGray);
    //            Console.WriteLine(margin + "       ____====  ]OO|_n_n__][.", Color.DeepSkyBlue);
    //            Console.WriteLine(margin + "      [________]_|__|________)< ", Color.DeepSkyBlue);
    //            Console.WriteLine(margin + "       oo    oo  'oo OOOO-| oo\\_", Color.Blue);
    //            Console.WriteLine("   +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+", Color.Silver);

    //            Thread.Sleep(200);
    //        }
    //    }
    //}

    //static void ColorfulAnimation2()
    //{

    //    for (int i = 0; i < 5; i++)
    //    {
    //        var margin = "";
    //        for (int j = 0; j < 30; j++)
    //        {
    //            Console.Clear();

    //            // steam

    //            Console.WriteLine();

    //            margin += "=";

    //            Console.WriteLine(margin + ">");
    //            Console.WriteLine("   +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+", Color.Silver);

    //            //Thread.Sleep(200);
    //        }
    //    }
    //}
    // }
    //}


//    Position A;
//    Position B;
//    Position C;
//    Position D;
//    Line line;
//    Line line2;
//    Contour contour;
//    Contour contour2;
//    List<Position> positionList;
//    PositionMatrix matrix;
//    List<Line> lines;
//    List<Contour> contours;
//    List<GTSPPrecedenceConstraint> linePrecedences;
//    List<GTSPPrecedenceConstraint> contourPrecedences;

//    A = new Position()
//    {
//        Name = "A",
//                UserID = 1,
//                ResourceID = 1,
//                Virtual = false,
//                Vector = new double[] { 1, 2, 3 }
//            };

//    B = new Position()
//    {
//        Name = "B",
//                UserID = 2,
//                ResourceID = 1,
//                Virtual = false,
//                Vector = new double[] { 1, 2, 3 }
//            };

//    C = new Position()
//    {
//        Name = "C",
//                UserID = 3,
//                ResourceID = 1,
//                Virtual = false,
//                Vector = new double[] { 1, 2, 3 }
//            };

//    D = new Position()
//    {
//        Name = "D",
//                UserID = 4,
//                ResourceID = 1,
//                Virtual = false,
//                Vector = new double[] { 1, 2, 3 }
//            };

//    contour = new Contour();
//    contour2 = new Contour();

//    line = new Line()
//    {
//        NodeA = A,
//                NodeB = B
//            };

//    line2 = new Line()
//    {
//        NodeA = C,
//                NodeB = D
//            };

//    contour.Lines.Add(line);
//            contour2.Lines.Add(line2);

//            positionList = new List<Position>() { A, B, C, D
//};
//matrix = new PositionMatrix()
//{
//    Positions = positionList,
//    Matrix = new double[,] { { 1, 2, 3, 4 }, { 2, 1, 3, 4 }, { 2, 1, 3, 4 }, { 2, 1, 3, 4 } },
//    DistanceFunction = new EuclidianDistanceFunction(),
//    ResourceFunction = new ConstantResourceFunction(1, new AddResourceDistanceLinkFunction())
//};
//lines = new List<Line>() { line, line2 };
//contours = new List<Contour>() { contour, contour2 };
//linePrecedences = new List<GTSPPrecedenceConstraint>() { new GTSPPrecedenceConstraint(line, line2) };
//contourPrecedences = new List<GTSPPrecedenceConstraint>() { new GTSPPrecedenceConstraint(contour, contour2) };


//LineLikeTask task = new LineLikeTask()
//{
//    Dimension = 3,
//    TimeLimit = 5000,
//    CyclicSequence = true,
//    StartDepot = A,
//    //FinishDepot = finish,
//    WeightMultipier = 10,
//    ContourPenalty = 1,
//    LinePrecedences = linePrecedences,
//    ContourPrecedences = contourPrecedences,
//    Contours = contours,
//    Lines = lines,
//    PositionMatrix = matrix
//};
//task.ValidateModel();
////task.GenerateModel();
//task.RunModel();
//        }