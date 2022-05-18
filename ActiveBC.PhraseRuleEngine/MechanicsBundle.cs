using System;
using ActiveBC.PhraseRuleEngine.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Build.Tokenization;

namespace ActiveBC.PhraseRuleEngine
{
    /// <summary>
    /// This class is used to pass the configuration available input processor to rule space factory.
    /// </summary>
    public sealed class MechanicsBundle
    {
        public string Key { get; }
        public IPatternTokenizer Tokenizer { get; }
        public IInputProcessorFactory Factory { get; }
        public Type TokenType { get; }

        public MechanicsBundle(
            string key,
            IPatternTokenizer tokenizer,
            IInputProcessorFactory factory,
            Type tokenType
        )
        {
            this.Key = key;
            this.Tokenizer = tokenizer;
            this.Factory = factory;
            this.TokenType = tokenType;
        }
    }
}