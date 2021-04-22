﻿using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencePlanner.Helper;
using SequencePlanner.GeneralModels;

namespace SequencePlanner.Function.ResourceFunction
{
    public interface IResourceFunction
    {
        public string FunctionName { get; }
        public IResourceDistanceLinkFunction LinkingFunction { get; set; }

        public double ComputeResourceCost(Config A, Config B, double distance);
        public void Validate();
        public void ToLog(LogLevel level);
    }
}