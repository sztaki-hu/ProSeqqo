using SequencePlanner.GTSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class CommonTemplateCompiler
    {
        private CommonTask Task { get; set; }
        private CommonTemplate Template { get; set; }

        public IAbstractTask Compile(IAbstractTemplate template)
        {
            Template = (CommonTemplate)template;
            Task = new CommonTask();
            Fill();
            PositionList();
            PositionMatrix();
            FindStartAndFinishDepot();
            SetDistanceFunction();
            return Task;
        }

        private void Fill()
        {
            Task.TaskType = Template.TaskType;
            Task.EdgeWeightSource = Template.EdgeWeightSource ;
            Task.Dimension = Template.Dimension;
            Task.TimeLimit = Template.TimeLimit;
            Task.CyclicSequence = Template.CyclicSequence;
            Task.WeightMultiplier = Template.WeightMultiplier;
            Task.PositionMatrix = Template.PositionMatrix;
        }

        private void PositionList()
        {
            Task.PositionList = new List<Position>();
            if (Template.PositionList != null)
            {
                foreach (var item in Template.PositionList)
                {
                    Task.PositionList.Add(new Position(item.ID, item.Name, item.Position));
                }
            }
        }

        private void PositionMatrix()
        {
            if (Template.PositionMatrix != null)
            {
                Template.PositionList = new List<Options.Values.PositionOptionValue>();
                for (int i = 0; i < Template.PositionMatrix.ID.Count; i++)
                {
                    string name;
                    if (Template.PositionMatrix.Name.Count > 0)
                    {
                        name = Template.PositionMatrix.Name[i];
                    }
                    else
                    {
                        name = "Position_" + i;
                    }
                    Template.PositionList.Add(new Options.Values.PositionOptionValue() { ID = Template.PositionMatrix.ID[i], Name = name, Dim = 0, Position = new List<double>() });
                    Task.PositionList.Add(new Position() { UID = Template.PositionMatrix.ID[i], Name = name, Configuration = new List<double>() });
                }
            }
        }

        private void FindStartAndFinishDepot()
        {
            if (Template.StartDepotID != -1)
            {
                bool find = false;
                foreach (var pos in Task.PositionList)
                {
                    if (pos.GID == Template.StartDepotID)
                    {
                        Task.StartDepot = pos;
                        find = true;
                    }
                }
                if (!find)
                    Console.WriteLine("CommonTemplateCompiler: StartDepot not found by ID!");
            }

            if (Template.FinishDepotID != -1)
            {
                bool find = false;
                foreach (var pos in Task.PositionList)
                {
                    if (pos.GID == Template.FinishDepotID)
                    {
                        Task.FinishDepot = pos;
                        find = true;
                    }
                }
                if (!find)
                    Console.WriteLine("CommonTemplateCompiler: FinishDepot not found by ID!");
            }
        }

        private void SetDistanceFunction()
        {
            if (Template.DistanceFunction == Options.Values.DistanceFunctionEnum.Trapezoid_Time || Template.DistanceFunction == Options.Values.DistanceFunctionEnum.Trapezoid_Time_WithTieBreaker)
            {
                var trapParam = new TrapezoidParams(Template.TrapezoidParamsAcceleration.ToArray(), Template.TrapezoidParamsSpeed.ToArray());
                Task.DistanceFunction = new EdgeWeightCalculator(Template.DistanceFunction, trapParam);
            }
            else
            {
                if (Template.DistanceFunction == Options.Values.DistanceFunctionEnum.FullMatrix)
                {
                    Task.DistanceFunction = new EdgeWeightCalculator(Template.DistanceFunction, Template.PositionMatrix);
                }
                else
                {
                    Task.DistanceFunction = new EdgeWeightCalculator(Template.DistanceFunction);
                }
            }
        }
    }
}