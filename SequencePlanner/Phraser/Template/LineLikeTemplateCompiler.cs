using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class LineLikeTemplateCompiler: TemplateCompiler
    {
        public LineLikeTask Compile(LineLikeTemplate template)
        {
            return new LineLikeTask();
        }
    }
}
