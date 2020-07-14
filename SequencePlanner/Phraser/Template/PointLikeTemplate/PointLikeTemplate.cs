using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class PointLikeTemplate : CommonTemplate
    {
        public List<ProcessHierarchyOptionValue> ProcessHierarchy { get; set; }
        public List<PrecedenceOptionValue> ProcessPrecedence { get; set; }
        public List<PrecedenceOptionValue> PositionPrecedence { get; set; }
        public CommonTask Task { get; set; }

        public PointLikeTemplate(OptionSet optionSet, bool validate = true) : base(optionSet)
        {
            Validation = validate;
            OptionSet = optionSet;
            Fill(optionSet);
            if (Validation)
                Validate();
            else
                Console.WriteLine("Warning: Template not validated!");
        }

        public new IAbstractTask Compile()
        {
            Task = (CommonTask) base.Compile();
            PointLikeTemplateCompiler compiler = new PointLikeTemplateCompiler();
            return compiler.Compile(this, Task);
        }

        public new void Validate()
        {
            PointLikeTemplateValidator validator = new PointLikeTemplateValidator();
            if (!validator.Validate(this))
                Console.WriteLine("LikeLike Template validation Error!");
        }

        private void Fill(OptionSet optionSet)
        {
            try
            {
                if (optionSet != null)
                {
                    ProcessHierarchy = ((ProcessHierarchy)optionSet.FindOption("ProcessHierarchy")).Value;
                    ProcessPrecedence = ((ProcessPrecedence)optionSet.FindOption("ProcessPrecedence")).Value;
                    PositionPrecedence = ((PositionPrecedence)optionSet.FindOption("PositionPrecedence")).Value;
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
            string tmp = "\nPointLikeTemplate details:";

            tmp += "\n\tProcessHierarchy: " + ProcessHierarchy?.ToString();
            tmp += "\n\tProcessPrecedence: " + ProcessPrecedence?.ToString();
            tmp += "\n\tPositionPrecedence: " + PositionPrecedence?.ToString();
            tmp += "\n\n: ";
            return tmp;
        }
    }
}
