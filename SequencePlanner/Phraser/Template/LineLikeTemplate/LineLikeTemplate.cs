using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class LineLikeTemplate : CommonTemplate
    {
        public List<LineListOptionValue> LineList { get; set; }
        public List<PrecedenceOptionValue> LinePrecedence { get; set; }
        public List<PrecedenceOptionValue> ContourPrecedence { get; set; }
        public int ContourPenalty { get; set; }
        public bool BidirectionLineDefault { get; set; }
        private CommonTask CommonTask { get; set; }


        public LineLikeTemplate(OptionSet optionSet, bool validate = true): base(optionSet, validate)
        {
            OptionSet = optionSet;
            Fill(OptionSet);
            if (validate)
                Validate();
            else
                Console.WriteLine("Warning: Template not validated!");
        }
        
        public new IAbstractTask Compile()
        {
            CommonTask = (CommonTask) base.Compile();
            if (CommonTask.StartDepot != null)
                StartDepotID = CommonTask.StartDepot.UID;
            else
                StartDepotID = -1;
            return new LineLikeTask(this, CommonTask);
        }

        public new void Validate()
        {
            LineLikeTemplateValidator validator = new LineLikeTemplateValidator();
            if (!validator.Validate(this))
                    Console.WriteLine("LikeLike Template validation Error!");
        }
        
        private void Fill(OptionSet optionSet)
        {
            try
            {
                if (optionSet != null)
                {
                    LineList = ((LineList)optionSet.FindOption("LineList")).Value;
                    LinePrecedence = ((LinePrecedence)optionSet.FindOption("LinePrecedence")).Value;
                    ContourPrecedence = ((ContourPrecedence)optionSet.FindOption("ContourPrecedence")).Value;
                    ContourPenalty = ((ContourPenalty)optionSet.FindOption("ContourPenalty")).Value;
                    BidirectionLineDefault = ((BidirectionLineDefault)optionSet.FindOption("BidirectionLineDefault")).Value;
                }
                else
                {
                    Console.WriteLine("Template:Validate failed, no optionSet");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Template:Validate failed " + e.Message);
            }
        }

        public override string ToString()
        {
            string tmp = "\nLineLike Template details:";
            tmp += "\n\tLineList: " + LineList?.ToString();
            tmp += "\n\tLinePrecedence: " + LinePrecedence?.ToString();
            tmp += "\n\tContourPrecedence: " + ContourPrecedence?.ToString();
            tmp += "\n\tContourPenalty: " + ContourPenalty.ToString();
            tmp += "\n\n: ";
            return tmp;
        }
    }
}