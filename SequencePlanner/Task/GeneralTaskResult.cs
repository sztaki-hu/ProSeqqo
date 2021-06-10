﻿using SequencePlanner.Helper;
using SequencePlanner.Model;
using SequencePlanner.Model.Hierarchy;
using System;
using System.Collections.Generic;

namespace SequencePlanner.Task
{
    public class GeneralTaskResult
    {
        public TimeSpan FullTime { get; set; }
        public TimeSpan SolverTime { get; set; }
        public TimeSpan PreSolverTime { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string ErrorMessage { get; set; }
        public List<int> SolutionMotionIDs { get; set; }
        public List<int> SolutionConfigIDs { get; set; }
        public List<Motion> SolutionMotion { get; set; }
        public List<Config> SolutionConfig { get; set; }
        public List<HierarchyRecord> SolutionHierarchy { get; set; }
        public List<double> ConfigCosts { get; set; }
        public List<double> MotionCosts { get; set; }
        public double FullMotionCost { get; set; }
        public double FullConfigCost { get; set; }
        public List<DetailedMotionCost> CostsBetweenMotions { get; set; }
        public List<DetailedConfigCost> CostsBetweenConfigs { get; set; }
        public DetailedConfigCost DetailedConfigCost { get; set; }
        public DetailedMotionCost DetailedMotionCost { get; set; }

    public GeneralTaskResult()
        {
            StatusCode = -1;
            StatusMessage = "Result initalized.";
            ErrorMessage = "No Error";
            FullTime = new TimeSpan();
            SolverTime = new TimeSpan();
            PreSolverTime = new TimeSpan();
            SolutionMotionIDs = new List<int>();
            SolutionConfigIDs = new List<int>();
            SolutionMotion = new List<Motion>();
            SolutionConfig = new List<Config>();
            SolutionHierarchy = new List<HierarchyRecord>();
            MotionCosts = new List<double>();
            ConfigCosts = new List<double>();
            CostsBetweenConfigs = new List<DetailedConfigCost>();
            CostsBetweenMotions = new List<DetailedMotionCost>();
            DetailedConfigCost = new DetailedConfigCost();
            DetailedMotionCost = new DetailedMotionCost();
        }

        public void ToLog(LogLevel logLevel)
        {
            SeqLogger.WriteLog(logLevel, "Status code: " + StatusCode);
            SeqLogger.WriteLog(logLevel, "Status message: " + StatusMessage);
            SeqLogger.WriteLog(logLevel, "Error message: " + ErrorMessage);
            SeqLogger.WriteLog(logLevel, "Full Time:" + FullTime);
            SeqLogger.WriteLog(logLevel, "Solver Time: " + SolverTime);
            SeqLogger.WriteLog(logLevel, "MIP Solver Time: " + PreSolverTime);
            SeqLogger.WriteLog(logLevel, "Full Cost: " + FullMotionCost);
            SeqLogger.WriteLog(logLevel, "Solution MotionIDs:" + SolutionMotionIDs.ToListString());
            SeqLogger.WriteLog(logLevel, "Solution ConfigIDs:" + SolutionConfigIDs.ToListString());
            SeqLogger.WriteLog(logLevel, "Motion Cost: " + MotionCosts.ToListString());
            SeqLogger.WriteLog(logLevel, "Config Cost: " + ConfigCosts.ToListString());
            SeqLogger.WriteLog(logLevel, "Motion Costs: ");
            SeqLogger.Indent++;
            foreach (var c in CostsBetweenMotions)
            {
                SeqLogger.WriteLog(logLevel, "\t" + c.ToString());
            }
            SeqLogger.Indent--;
            SeqLogger.WriteLog(logLevel, "Config Costs: ");
            SeqLogger.Indent++;
            foreach (var c in CostsBetweenConfigs)
            {
                SeqLogger.WriteLog(logLevel, "\t" + c.ToString());
            }
            SeqLogger.Indent--;
            SeqLogger.WriteLog(logLevel, "Solution Hierarchy: ");
            SeqLogger.Indent++;
            foreach (var c in SolutionHierarchy)
            {
                SeqLogger.WriteLog(logLevel, "\t" + c.ToString());
            }
            SeqLogger.WriteLog(logLevel, "Full cost of motions with details: " + DetailedMotionCost.ToString());
            SeqLogger.WriteLog(logLevel, "Full cost of configs with details: " + DetailedConfigCost.ToString());
            SeqLogger.Indent--;
        }

        public static string CSVHeader()
        {
            var sep = ";";
            return "StatusCode" + sep
            + "StatusMessage" + sep
            + "ErrorMessage" + sep
            + "FullTime" + sep
            + "SolverTime" + sep
            + "PreSolverTime" + sep
            + "SumMotionCost" + sep
            + "SumComutedMotionCost" + sep
            + "SumOverrideMotionCost" + sep
            + "SumPenaltyCost" + sep
            + "SumInMotionCost" + sep
            + "SolutionMotionIDs" + sep
            + "SolutionConfigIDs";
        }

        public string ToCSV()
        {
            var sep = ";";
            return StatusCode + sep
            + StatusMessage + sep
            + ErrorMessage + sep
            + FullTime + sep
            + SolverTime + sep
            + PreSolverTime + sep
            + FullMotionCost + sep
            + DetailedMotionCost.DistanceFunctionCost + sep
            + DetailedMotionCost.OverrideCost + sep
            + DetailedMotionCost.Penalty + sep
            + DetailedMotionCost.InMotion + sep
            + SolutionMotionIDs.ToListString() + sep
            + SolutionConfigIDs.ToListString();
        }

        public void CalculateSum()
        {

            DetailedConfigCost = new DetailedConfigCost();
            DetailedMotionCost = new DetailedMotionCost();
            foreach (var motionCost in CostsBetweenMotions)
            {
                DetailedMotionCost.DistanceFunctionCost += motionCost.DistanceFunctionCost;
                DetailedMotionCost.OverrideCost += motionCost.OverrideCost;
                DetailedMotionCost.Penalty += motionCost.Penalty;
                DetailedMotionCost.FinalCost += motionCost.FinalCost;
                DetailedMotionCost.ResourceChangeoverCost += motionCost.ResourceChangeoverCost;
                DetailedMotionCost.InMotion += motionCost.PreviousMotionCost;
            }

            foreach (var configCost in CostsBetweenConfigs)
            {
                DetailedConfigCost.DistanceFunctionCost += configCost.DistanceFunctionCost;
                DetailedConfigCost.OverrideCost += configCost.OverrideCost;
                DetailedConfigCost.Penalty += configCost.Penalty;
                DetailedConfigCost.FinalCost += configCost.FinalCost;
                DetailedConfigCost.ResourceChangeoverCost += configCost.ResourceChangeoverCost;
            }
        }
    }
}
