using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens
{
    public sealed class RuleReferenceToken : IQuantifiableToken
    {
        public string? DeclaredInNamespace { get; }
        public string RuleName { get; }
        public IRuleArgumentToken[] Arguments { get; }

        public RuleReferenceToken(
            string? declaredInNamespace,
            string ruleName,
            IRuleArgumentToken[] arguments
        )
        {
            this.DeclaredInNamespace = declaredInNamespace;
            this.RuleName = ruleName;
            this.Arguments = arguments;
        }

        public override string ToString()
        {
            return $"${this.RuleName}{(this.Arguments.Length == 0 ? string.Empty : $"({this.Arguments.Select(argument => argument.ToString()).JoinToString(", ")})")}";
        }

        public string GetRuleSpaceKey()
        {
            // todo remove "self" support
            string ruleName = this.RuleName.RemoveFromStart("self.");

            // if reference is fully-qualified
            if (ruleName.Contains("."))
            {
                return ruleName;
            }

            // if we can't add namespace
            if (this.DeclaredInNamespace is null)
            {
                return ruleName;
            }

            // self namespace not supposed to be a part of rule space
            if (this.DeclaredInNamespace == "self")
            {
                return ruleName;
            }

            return $"{this.DeclaredInNamespace}.{ruleName}";
        }
    }
}