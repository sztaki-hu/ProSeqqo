﻿using SequencePlanner.GTSP;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using SequencePlanner.OR_Tools;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SequencePlanner.GTSPTask.Task.LineLike
{
    public class LineLikeTask: BaseTask
    {
        public List<Line> Lines { get; set; }
        public List<Contour> Contours { get; set; }
        public double ContourPenalty { get; set; }
        public List<GTSPPrecedenceConstraint> LinePrecedences { get; set; }
        public List<GTSPPrecedenceConstraint> ContourPrecedences { get; set; }
        private LineLikeGTSPRepresentation GTSPRepresentation { get; set; }
        private int MAX_SEQUENCING_ID = 0;

        public LineLikeTask():base()
        {
            Lines = new List<Line>();
            Contours = new List<Contour>();
            LinePrecedences = new List<GTSPPrecedenceConstraint>();
            ContourPrecedences = new List<GTSPPrecedenceConstraint>();
        }

        public LineTaskResult RunModel()
        {
            SeqLogger.Info("RunModel started!", nameof(LineLikeTask));
            SeqLogger.Indent++;
            GenerateModel();
            var orToolsParam = new ORToolsTask()
            {
                TimeLimit = TimeLimit,
                GTSPRepresentation = GTSPRepresentation
            };
            if (UseMIPprecedenceSolver)
                GTSPRepresentation.InitialRoutes = CreateInitialRout();
            var orTools = new ORToolsSequencerWrapper(orToolsParam);
            orTools.Build();
            var result = ResolveSolution(orTools.Solve());
            SeqLogger.Indent--;
            SeqLogger.Info("RunModel finished!", nameof(LineLikeTask));
            result.ToString();
            return result;
        }

        protected long[][] CreateInitialRout()
        {

            var ORPreSolver = new ORToolsLineLikePreSolverWrapper(new ORToolsLineLikePreSolverTask()
            {
                NumberOfNodes = Lines.Count,
                DisjointConstraints = GTSPRepresentation.DisjointConstraints,
                OrderPrecedenceConstraints = GTSPRepresentation.PrecedenceConstraints,
                StartDepot = GTSPRepresentation.StartDepot
            });
            var result = ORPreSolver.Solve();
            if (result.Count > 0)
            {
                long[][] initialSolution = new long[1][];
                initialSolution[0] = new long[result.Count];
                for (int i = 0; i < result.Count; i++)
                {
                    initialSolution[0][i] = result[i];
                }
                return initialSolution;
            }
            return null;
        }

        private void GenerateModel()
        {
            SeqLogger.Info("Generate model started!", nameof(LineLikeTask));
            SeqLogger.Indent++;
            AddVirtualStart(); //Handle Cyclic sequence with start/finish depots
            foreach (var line in Lines)
            {
                line.SequencingID = MAX_SEQUENCING_ID++;
            }

            GTSPRepresentation = new LineLikeGTSPRepresentation()
            {
                DisjointConstraints = CreateDisjointConstraints(),
                PrecedenceConstraints = CreatePrecedenceConstraints(),
                Matrix = CreateGTSPMatrix(),
                StartDepot = startSeqID
            };
            GTSPRepresentation.RoundedMatrix = ScaleUpWeights(GTSPRepresentation.Matrix);
            SeqLogger.Indent--;
            SeqLogger.Info("Generate model finished!", nameof(LineLikeTask));
        }

        public override void ValidateModel()
        {
            var validator = new LineLikeTaskValidator();
            validator.Validate(this);
        }

        public async Task<LineTaskResult> RunModelAsync(int taskID, CancellationToken cancellationToken)
        {
            System.Console.WriteLine(taskID);
            return RunModel();
        }

        private List<GTSPPrecedenceConstraint> CreatePrecedenceConstraints()
        {
            List<GTSPPrecedenceConstraint> precedenceConstraints = new List<GTSPPrecedenceConstraint>();
            if (LinePrecedences != null)
            {
                foreach (var precedence in LinePrecedences)
                {
                    //Find original line and reverse too!
                    var beforeList = FindLineByUserID(precedence.Before.UserID);
                    var afterList = FindLineByUserID(precedence.After.UserID);
                    if (beforeList != null && afterList != null)
                    {
                        foreach (var before in beforeList)
                        {
                            foreach (var after in afterList)
                            {
                                precedenceConstraints.Add(new GTSPPrecedenceConstraint(before, after));
                            }
                        }
                    }
                }
            }
            foreach (var precedence in ContourPrecedences)
            {
                foreach (var beforeLine in ((Contour)precedence.Before).Lines)
                {
                    foreach (var afterLine in ((Contour)precedence.After).Lines)
                    {
                        precedenceConstraints.Add(new GTSPPrecedenceConstraint()
                        {
                            Before = beforeLine,
                            After = afterLine
                        }); ;
                    }
                }
            }
            SeqLogger.Info("Order precedences created!", nameof(LineLikeTask));
            return precedenceConstraints;
        }

        private List<GTSPDisjointConstraint> CreateDisjointConstraints()
        {
            List<GTSPDisjointConstraint> disjointConstraints = new List<GTSPDisjointConstraint>();

            foreach (var contour in Contours)
            {
                var lineNumber = contour.Lines.Count;
                for (int i = 0; i < lineNumber; i++)
                {
                    var line = contour.Lines[i];
                    var constraint = new GTSPDisjointConstraint();
                    constraint.Add(line);
                    if (line.Bidirectional)
                    {
                        Line newLine = line.Copy();
                        newLine.Name += "_Reverse";
                        newLine.NodeA = line.NodeB;
                        newLine.NodeB = line.NodeA;
                        newLine.SequencingID = MAX_SEQUENCING_ID++;
                        Lines.Add(newLine);
                        contour.Lines.Add(newLine);
                        constraint.Add(newLine);
                    }
                    disjointConstraints.Add(constraint);
                }
            }
            if (virtualStart != null)
            {
                var startContraint = new GTSPDisjointConstraint();
                startContraint.Add(virtualStart);
                disjointConstraints.Add(startContraint);
            }
            SeqLogger.Info("Disjoint precedences created!", nameof(LineLikeTask));
            return disjointConstraints;
        }

        private double[,] CreateGTSPMatrix()
        {
            double[,] matrix = new double[Lines.Count,Lines.Count];

            foreach (var lineFrom in Lines)
            {
                foreach (var lineTo in Lines)
                {
                    double weight;
                    if (lineFrom.GlobalID != lineTo.GlobalID)
                    {
                        weight = CalculateWeight(lineFrom, lineTo);
                    }
                    else
                    {
                        weight = 0.0;
                    }
                    matrix[lineFrom.SequencingID, lineTo.SequencingID] = weight;
                }
            }
            SetWeightsForVirtualStart(matrix); //Handle Cyclic sequence with start/finish depots with override of matrix
            SeqLogger.Info("GTSP matrix created!", nameof(LineLikeTask));
            return matrix;
        }

        private Line virtualStart;
        private int startSeqID;
        private void AddVirtualStart()
        {
            Line line = new Line()
            {
                Name = "VirtualStartLine",
                Virtual = true,
                ResourceID = -1,
                Bidirectional = false
            };
            if (!CyclicSequence)
            {
                if (StartDepot != null && FinishDepot != null)
                {
                    line.NodeA = FinishDepot;
                    line.NodeB = StartDepot;
                }
                if (StartDepot != null && FinishDepot == null)
                {
                    line.NodeA = StartDepot;
                    line.NodeB = StartDepot;
                }
                if (StartDepot == null && FinishDepot != null)
                {
                    line.NodeA = FinishDepot;
                    line.NodeB = FinishDepot;
                }
                if (StartDepot == null && FinishDepot == null)
                {
                    line.NodeA = new Position() { Virtual = true };
                    line.NodeB = new Position() { Virtual = true };
                }
            }
            else
            {
                line.NodeA = StartDepot;
                line.NodeB = StartDepot;
            }
            Lines.Add(line);
            virtualStart = line;
            SeqLogger.Info("Virtual start and finish handled!", nameof(LineLikeTask));
        }

        private void SetWeightsForVirtualStart(double[,] matrix)
        {
                startSeqID = virtualStart.SequencingID;
                for (int i = 0; i < Lines.Count; i++)
                {
                    if (startSeqID != i)
                    {
                        if (StartDepot != null && FinishDepot != null) //We have StartDepot and FinishDepot
                        {
                            //matrix[virtualStart.SequencingID, i] = 0.0;
                            //matrix[i, virtualStart.SequencingID] = 0.0;
                        }
                        if (StartDepot != null && FinishDepot == null && !CyclicSequence) // Only StartDepot and not cyclic
                        {
                            matrix[i, virtualStart.SequencingID] = 0.0;
                            //matrix[virtualStart.SequencingID, i] = 0.0; //Calculated real value in createGTSPMatrix()
                        }
                        if (StartDepot == null && FinishDepot != null) //Only FinisDepot
                        {
                            matrix[virtualStart.SequencingID, i] = 0.0;
                            //matrix[i, virtualStart.SequencingID] = 0.0; //Calculated real value in createGTSPMatrix()
                        }
                        if (StartDepot == null && FinishDepot == null) //No StartDepot and FinishDepotGiven
                        {
                            //Original is good in this case calculated in createGTSPMatrix()
                        }
                        if (StartDepot != null && FinishDepot == null && CyclicSequence) //Only StartDepot and Cyclic
                        {
                            //Original is good in this case calculated in createGTSPMatrix()
                        }
                    }
                    else
                    {
                        matrix[i, i] = 0.0; // self containing loop 
                    }
                }
        }

        private LineTaskResult ResolveSolution(TaskResult result)
        {
            LineTaskResult taskResult = new LineTaskResult(result);
            foreach (var raw in taskResult.SolutionRaw)
            {
                var find = false;
                foreach (var line in Lines)
                {
                    if(line.SequencingID == raw)
                    {
                        if (!line.Virtual)
                        {
                            taskResult.Result.Add(line.UserID);
                            taskResult.ResultWithVirtual.Add(line.UserID);
                            taskResult.LineResult.Add(line);
                        }
                        else
                        {
                            taskResult.ResultWithVirtual.Add(line.UserID);
                        }
                        find = true;
                        break;
                    }
                }
                if (!find)
                    throw new SequencerException("Result of OR-Tools can not be resolved, no line found with the SequenceID: "+raw);
            }

            for (int i = 1; i < taskResult.LineResult.Count; i++)
            {
                taskResult.Costs.Add(GTSPRepresentation.Matrix[taskResult.LineResult[i - 1].SequencingID, taskResult.LineResult[i].SequencingID]);
                taskResult.CostBetweenLines += taskResult.Costs[i - 1];
                if (taskResult.Costs[i-1]>0 && !taskResult.LineResult[i - 1].Virtual && !taskResult.LineResult[i].Virtual)
                {
                    taskResult.CostBetweenLinesNoPenalty -= ContourPenalty;
                    taskResult.Penalty += ContourPenalty;
                }
            }
            taskResult.CostSum = taskResult.CostBetweenLines;
            taskResult.CostSumNoPenalty += taskResult.CostBetweenLinesNoPenalty;
            taskResult.CostBetweenLinesNoPenalty += taskResult.CostBetweenLines;

            foreach (var line in taskResult.LineResult)
            {
                if (!line.Virtual) {
                    taskResult.CostOfLines += PositionMatrix.DistanceFunction.ComputeDistance(line.NodeA, line.NodeB);
                    line.Length = PositionMatrix.DistanceFunction.ComputeDistance(line.NodeA, line.NodeB);
                }
            }
            taskResult.CostSum += taskResult.CostOfLines;
            taskResult.CostSumNoPenalty += taskResult.CostSum;
            SeqLogger.Info("Solution resolved!", nameof(LineLikeTask));
            return taskResult;
        }

        private double CalculateWeight(Line lineFrom, Line lineTo)
        {
            if (lineFrom.NodeB.Virtual || lineTo.NodeA.Virtual)
                return 0.0;
            double weight = PositionMatrix.DistanceFunction.ComputeDistance(lineFrom.NodeB, lineTo.NodeA);
            weight = PositionMatrix.ResourceFunction.ComputeResourceCost(lineFrom.NodeB, lineTo.NodeB, weight);
            if (weight > 0)
            {
                weight += ContourPenalty;
            }
            //if (lineFrom.Contour.ID != lineTo.Contour.ID)
            //    weight += ContourPenalty;
            return weight;
        }

        private List<Line> FindLineByUserID(int UserID)
        {
            List<Line> tmp = new List<Line>(); ;
            foreach (var line in Lines)
            {
                if (line.UserID == UserID)
                    tmp.Add(line);
            }
            return tmp;
        }
    }
}