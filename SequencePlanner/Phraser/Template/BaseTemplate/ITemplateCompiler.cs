namespace SequencePlanner.Phraser.Template
{
    public interface ITemplateCompiler
    {
        IAbstractTask Compile(IAbstractTemplate template);
    }
}