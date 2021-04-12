using Newtonsoft.Json;
using SequencePlanner.GTSPTask.Serialization.SerializationObject;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using SequencePlanner.OR_Tools;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.Task
{
    public class BaseTaskSerializationObject
    {
        [JsonProperty(Order = 1)]
        public string TaskType { get; set; }
        [JsonProperty(Order = 2)]
        public int Dimension { get; set; }
        [JsonProperty(Order = 3)]
        public bool CyclicSequence { get; set; }
        [JsonProperty(Order = 4)]
        public int StartDepot { get; set; }
        [JsonProperty(Order = 5)]
        public int FinishDepot { get; set; }
        [JsonProperty(Order = 6)]
        public int TimeLimit { get; set; }
        public bool UseMIPprecedenceSolver { get; set; }
        public bool Validate { get; set; }
        public List<PositionSerializationObject> PositionList { get; set; }
        public DistanceFunctionSerializationObject DistanceFunction {get;set;}
        public ResourceFunctionSerializationObject ResourceFunction {get;set;}
        public StrictEdgeWeightSetSerializationObject StrictUserEdgeWeights { get; set; }
        public string LocalSearchStrategy { get; set; }
        public bool BidirectionLineDefault { get; set; }
        public bool UseLineLengthInWeight { get; set; }
        public bool UseResourceInLineLength { get; set; }

        public BaseTaskSerializationObject()
        {
        }
        public BaseTaskSerializationObject(List<string> seqString)
        {
        }
        public BaseTaskSerializationObject(BaseTask baseTask)
        {
            Dimension = baseTask.Dimension;
            CyclicSequence = baseTask.CyclicSequence;
            BidirectionLineDefault = Line.BIDIRECTIONAL_DEFAULT;
            //WeightMultipier = baseTask.WeightMultipier;
            if (baseTask.StartDepot != null)
                StartDepot = baseTask.StartDepot.UserID;
            else
                StartDepot = -1;
            if (baseTask.FinishDepot != null)
                FinishDepot = baseTask.FinishDepot.UserID;
            else
                FinishDepot = -1;
            TimeLimit = baseTask.TimeLimit;
            PositionList = new List<PositionSerializationObject>();
            foreach (var item in baseTask.PositionMatrix.Positions)
            {
                PositionList.Add(new PositionSerializationObject(item.Out)); ///TODO: Rapid FIX change it!
            }
            DistanceFunction = new DistanceFunctionSerializationObject(baseTask.PositionMatrix);
            ResourceFunction = new ResourceFunctionSerializationObject(baseTask.PositionMatrix);
            StrictUserEdgeWeights = new StrictEdgeWeightSetSerializationObject(baseTask.PositionMatrix.StrictUserEdgeWeights);
            UseResourceInLineLength = false;
            UseLineLengthInWeight = false;
            LocalSearchStrategy = baseTask.LocalSearchStrategy.ToString();
            Validate = baseTask.Validate;
        }

        public string ToSEQShort()
        {
            //string separator = ";";
            string newline = "\n";
            string seq = "";
            seq += "#Export @ " + DateTime.UtcNow + newline;
            seq += "TaskType: "+ TaskType + newline;
            seq += "Validate: "+ Validate + newline;
            seq += "Dimension: " + Dimension + newline;
            seq += "CyclicSequence: " + CyclicSequence + newline;
            if(StartDepot!=-1)
                seq += "StartDepot: " + StartDepot + newline;
            if(FinishDepot!=-1)
                seq += "FinishDepot: " + FinishDepot + newline;
            seq += "TimeLimit: " + TimeLimit + newline;
            seq += "TimeLimit: " + TimeLimit + newline;
            seq += "LocalSearchStrategy: " + LocalSearchStrategy + newline;
            seq += "BidirectionLineDefault: " + BidirectionLineDefault + newline;
            seq += "UseResourceInLineLength: " + UseResourceInLineLength + newline;
            seq += "UseLineLengthInWeight: " + UseLineLengthInWeight + newline;
            seq += DistanceFunction.ToSEQShort();
            seq += ResourceFunction.ToSEQ();
            return seq;
        }

        public string ToSEQLong()
        {
            string newline = "\n";
            string seq = "";
            seq += DistanceFunction.ToSEQLong();
            if (DistanceFunction.Function != "Matrix")
            {
                seq += "PositionList:" + newline;
                foreach (var item in PositionList)
                {
                    seq += item.ToSEQ();
                }
            }
            if (StrictUserEdgeWeights != null && StrictUserEdgeWeights.Weights.Count > 0)
            {
                seq += "StrictEdgeWeights: " + newline;
                seq += StrictUserEdgeWeights.ToSEQ();

            }
            return seq;
        }

        public void ToBaseTask(BaseTask task)
        {
            //TaskType
            task.Dimension = Dimension;
            task.Validate = Validate;
            task.CyclicSequence = CyclicSequence;
            task.PositionMatrix = new PositionMatrix();
            Line.BIDIRECTIONAL_DEFAULT = BidirectionLineDefault;
            task.PositionMatrix.UseResourceInLineLength = UseResourceInLineLength;
            task.PositionMatrix.UseLineLengthInWeight = UseLineLengthInWeight;
            foreach (var pos in PositionList)
            {
                var newPosition = pos.ToPosition();
                task.PositionMatrix.Positions.Add(new GTSPNode(newPosition));
                if (newPosition.UserID == StartDepot)
                    task.StartDepot = newPosition;
                if (newPosition.UserID == FinishDepot)
                    task.FinishDepot = newPosition;
            }
            if ((StartDepot != null && StartDepot != -1) && task.StartDepot == null)
                SeqLogger.Error("StartDepot not exist position!", nameof(BaseTaskSerializationObject));
            if ((FinishDepot != null && FinishDepot != -1) && task.FinishDepot == null)
                SeqLogger.Error("FinishDepot not exist as position!", nameof(BaseTaskSerializationObject));
            task.TimeLimit = TimeLimit;
            task.PositionMatrix.StrictUserEdgeWeights = StrictUserEdgeWeights.ToStrictEdgeWeightSet(task.PositionMatrix.Positions);

            task.PositionMatrix.DistanceFunction = DistanceFunction.ToDistanceFunction();
            task.PositionMatrix.ResourceFunction = ResourceFunction.ToResourceFunction();
            task.UseMIPprecedenceSolver = UseMIPprecedenceSolver;
            task.LocalSearchStrategy = LocalSearchStrategyEnum.ResolveEnum(LocalSearchStrategy);
        }
        public void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            TaskType = tokenizer.GetStringByHeader("TaskType");
            Validate = tokenizer.GetBoolByHeader("Validate");
            Dimension = tokenizer.GetIntByHeader("Dimension");
            CyclicSequence = tokenizer.GetBoolByHeader("CyclicSequence");
            StartDepot = tokenizer.GetIntByHeader("StartDepot");
            FinishDepot = tokenizer.GetIntByHeader("FinishDepot");
            TimeLimit = tokenizer.GetIntByHeader("TimeLimit");
            PositionList = tokenizer.GetPositionListByHeader("PositionList");
            UseMIPprecedenceSolver = tokenizer.GetBoolByHeader("UseMIPprecedenceSolver");
            BidirectionLineDefault = tokenizer.GetBoolByHeader("BidirectionLineDefault");
            UseResourceInLineLength = tokenizer.GetBoolByHeader("UseResourceInLineLength");
            UseLineLengthInWeight = tokenizer.GetBoolByHeader("UseLineLengthInWeight");
            Line.BIDIRECTIONAL_DEFAULT = BidirectionLineDefault;
            DistanceFunction = new DistanceFunctionSerializationObject();
            DistanceFunction.FillBySEQTokens(tokenizer);
            if ((PositionList == null || PositionList.Count == 0) & DistanceFunction.Function == "Matrix")
                PositionList = DistanceFunction.DistanceMatrix.PositionList;
            ResourceFunction = new ResourceFunctionSerializationObject();
            ResourceFunction.FillBySEQTokens(tokenizer);
            LocalSearchStrategy = tokenizer.GetStringByHeader("LocalSearchStrategy");
            StrictUserEdgeWeights = tokenizer.GetStrictEdgeWeightSet("StrictEdgeWeights");
        }
    }
}