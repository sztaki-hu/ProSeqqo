using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class PointLikeTemplateCompiler: TemplateCompiler
    {
        public PointLikeTask Compile(PointLikeTemplate template)
        {
            return new PointLikeTask();
        }
    }
}