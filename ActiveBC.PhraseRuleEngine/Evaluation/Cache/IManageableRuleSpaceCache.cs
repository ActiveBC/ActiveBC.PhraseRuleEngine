namespace ActiveBC.PhraseRuleEngine.Evaluation.Cache
{
    public interface IManageableRuleSpaceCache : IRuleSpaceCache
    {
        void Clear();
    }
}