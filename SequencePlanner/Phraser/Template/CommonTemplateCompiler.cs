using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class CommonTemplateCompiler : TemplateCompiler
    {
        public CommonTask Compile(Template template)
        {
            return new CommonTask();
        }
    }
}
