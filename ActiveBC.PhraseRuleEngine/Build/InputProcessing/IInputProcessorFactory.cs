using ActiveBC.PhraseRuleEngine.Build.InputProcessing.Models;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.InputProcessing;

namespace ActiveBC.PhraseRuleEngine.Build.InputProcessing
{
    public interface IInputProcessorFactory
    {
        IInputProcessor Create(IPatternToken patternToken, IRuleSpace ruleSpace);

        RuleCapturedVariables ExtractOwnCapturedVariables(
            IPatternToken patternToken,
            IRuleDescriptionProvider ruleDescriptionProvider
        );
    }
}