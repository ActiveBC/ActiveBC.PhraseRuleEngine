using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.InputProcessing;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Walker;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing
{
    internal sealed class RegexProcessor : IInputProcessor
    {
        private readonly RegexAutomaton m_automaton;
        private readonly IRegexAutomatonWalker<RegexAutomaton> m_regexAutomatonWalker;

        public RegexProcessor(RegexAutomaton automaton, IRegexAutomatonWalker<RegexAutomaton> regexAutomatonWalker)
        {
            this.m_automaton = automaton;
            this.m_regexAutomatonWalker = regexAutomatonWalker;
        }

        public RuleMatchResultCollection Match(RuleInput ruleInput, int firstSymbolIndex, IRuleSpaceCache cache)
        {
            return this.m_regexAutomatonWalker.Walk(this.m_automaton, ruleInput, firstSymbolIndex, cache);
        }

        public IEnumerable<string> GetUsedWords()
        {
            return this.m_automaton.GetUsedWords();
        }
    }
}