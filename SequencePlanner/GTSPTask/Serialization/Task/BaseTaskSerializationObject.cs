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
        public List<PositionSerializationObject> PositionList { get; set; }
        public DistanceFunctionSerializationObject DistanceFunction {get;set;}
        public ResourceFunctionSerializationObject ResourceFunction {get;set;}
        public string LocalSearchStrategy { get; set; }

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
            LocalSearchStrategy = baseTask.LocalSearchStrategy.ToString();
        }

        public string ToSEQShort()
        {
            //string separator = ";";
            string newline = "\n";
            string seq = "";
            seq += "#Export @ " + DateTime.UtcNow + newline;
            seq += "TaskType: "+ TaskType + newline;
            seq += "Dimension: " + Dimension + newline;
            seq += "CyclicSequence: " + CyclicSequence + newline;
            if(StartDepot!=-1)
                seq += "StartDepot: " + StartDepot + newline;
            if(FinishDepot!=-1)
                seq += "FinishDepot: " + FinishDepot + newline;
            seq += "TimeLimit: " + TimeLimit + newline;
            seq += "TimeLimit: " + TimeLimit + newline;
            seq += "LocalSearchStrategy: " + LocalSearchStrategy + newline;
            seq += DistanceFunction.ToSEQShort();
            seq += ResourceFunction.ToSEQ();
            return seq;
        }

        public string ToSEQLong()
        {
            string newline = "\n";
            string seq = "";
            seq += DistanceFunction.ToSEQLong();
            if (DistanceFunction.Function != "MatrixDistance")
            {
                seq += "PositionList:" + newline;
                foreach (var item in PositionList)
                {
                    seq += item.ToSEQ();
                }
            }
            return seq;
        }

        public void ToBaseTask(BaseTask task)
        {
            //TaskType
            task.Dimension = Dimension;
            task.CyclicSequence = CyclicSequence;
            task.PositionMatrix = new PositionMatrix();
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
            task.PositionMatrix.DistanceFunction = DistanceFunction.ToDistanceFunction(task.PositionMatrix.Positions);
            task.PositionMatrix.ResourceFunction = ResourceFunction.ToResourceFunction();
            task.UseMIPprecedenceSolver = UseMIPprecedenceSolver;
            task.LocalSearchStrategy = LocalSearchStrategyEnum.ResolveEnum(LocalSearchStrategy);
        }
        public void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            TaskType = TokenConverter.GetStringByHeader("TaskType", tokenizer);
            Dimension = TokenConverter.GetIntByHeader("Dimension", tokenizer);
            CyclicSequence = TokenConverter.GetBoolByHeader("CyclicSequence", tokenizer);
            StartDepot = TokenConverter.GetIntByHeader("StartDepot", tokenizer);
            FinishDepot = TokenConverter.GetIntByHeader("FinishDepot", tokenizer);
            TimeLimit = TokenConverter.GetIntByHeader("TimeLimit", tokenizer);
            PositionList = TokenConverter.GetPositionListByHeader("PositionList", tokenizer);
            UseMIPprecedenceSolver = TokenConverter.GetBoolByHeader("UseMIPprecedenceSolver", tokenizer);
            DistanceFunction = new DistanceFunctionSerializationObject();
            DistanceFunction.FillBySEQTokens(tokenizer);
            if ((PositionList == null || PositionList.Count == 0) & DistanceFunction.Function == "MatrixDistance")
                PositionList = DistanceFunction.DistanceMatrix.PositionList;
            ResourceFunction = new ResourceFunctionSerializationObject();
            ResourceFunction.FillBySEQTokens(tokenizer);
            LocalSearchStrategy = TokenConverter.GetStringByHeader("LocalSearchStrategy", tokenizer);
        }
    }
}