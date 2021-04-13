using System.Collections.Generic;
using SequencePlanner.Helper;

namespace SequencePlanner.Model
{
    public class GTSPPrecedenceConstraintList
    {
        public List<BaseNode> Before { get; set; }
        public List<BaseNode> After { get; set; }


        public GTSPPrecedenceConstraintList()
        {
            Before = new List<BaseNode>();
            After = new List<BaseNode>();
        }
        public GTSPPrecedenceConstraintList(List<BaseNode> before, List<BaseNode> after)
        {
            Before = before;
            After = after;
        }


        public void Validate()
        {
            if (Before == null)
                throw new SeqException("GTSPPrecedenceConstraint.Before is null!");
            if (After == null)
                throw new SeqException("GTSPPrecedenceConstraint.After is null!");
        }
        public override string ToString()
        {
            var before = "";
            var after = "";
            foreach (var item in Before)
            {
                before += item + ", ";
            }
            before = before.Remove(before.Length - 2);
            foreach (var item in After)
            {
                after += item + ", ";
            }
            after = after.Remove(after.Length - 2);
            return "Precedence constraint before: [" + before + "] after: [" + after + "]";
        }
    }
}