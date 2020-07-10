using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class LineLikeTemplate : CommonTemplate
    {
        private SeqOptionSet optionSet;
        private CommonTask commonTask;

        public LineLikeTemplate(SeqOptionSet optionSet, CommonTask commonTask)
        {
            this.optionSet = optionSet;
            this.commonTask = commonTask;
        }

        public LineLikeTemplate()
        {
        }

        public List<LineListOptionValue> LineList { get; set; }
        public List<PrecedenceOptionValue> LinePrecedence { get; set; }
        public List<PrecedenceOptionValue> ContourPrecedence { get; set; }
        public int ContourPenalty { get; set; }

        internal LineLikeTask Parse(SeqOptionSet optionSet, CommonTask commonTask)
        {
            throw new NotImplementedException();
        }

        public new LineLikeTask Parse(OptionSet optionSet, bool validate = true)
        {
            Fill(optionSet);
            if (validate)
                Validate();
            else
                Console.WriteLine("Warning: Template not validated!");

            return Compile();
        }

        public new LineLikeTask Compile()
        {
            LineLikeTemplateCompiler compiler = new LineLikeTemplateCompiler();
            return compiler.Compile(this);
        }

        public new void Validate()
        {
            LineLikeTemplateValidator validator = new LineLikeTemplateValidator();
            if (!validator.Validate(this))
                    Console.WriteLine("LikeLike Template validation Error!");
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
    }
}
