using SequencePlanner.Phraser.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class Template : ITemplate
    {
        public OptionSet OptionSet { get; set; }

        public Template()
        {
            OptionSet = new OptionSet();
        }

        public void Read(string file)
        {
            string[] lines = File.ReadAllLines(@file);
            SequencerOptionPhraser phraser = new SequencerOptionPhraser();
            List<string> linesList = phraser.ReadFile(lines);
            OptionSet.FillValues(linesList);
            OptionSet.Validate();
        }

        public void Write(string file)
        {
            throw new NotImplementedException();
        }

        public SequencerTask Compile()
        {
            Validate();
            return TemplateCompiler.Compile(this);
        }

        private void Validate()
        {

        }
    }
}