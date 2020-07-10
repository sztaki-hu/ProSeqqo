using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class PointLikeTemplate : Template
    {
        public List<ProcessHierarchyOptionValue> ProcessHierarchy { get; set; }
        public List<PrecedenceOptionValue> ProcessPrecedence { get; set; }
        public List<PrecedenceOptionValue> PositionPrecedence { get; set; }


        public PointLikeTask Parse(OptionSet optionSet, bool validate = true)
        {
            Fill(optionSet);
            if (validate)
                Validate();
            else
                Console.WriteLine("Warning: Template not validated!");

            return Compile();
        }

        public PointLikeTask Compile()
        {
            PointLikeTemplateCompiler compiler = new PointLikeTemplateCompiler();
            return compiler.Compile(this);
        }

        public void Validate()
        {
            PointLikeTemplateValidator validator = new PointLikeTemplateValidator();
            if (!validator.Validate(this))
                Console.WriteLine("LikeLike Template validation Error!");
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
    }
}
