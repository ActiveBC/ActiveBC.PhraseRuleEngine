using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens
{
    public sealed class NerToken : IQuantifiableToken
    {
        public string VariableName { get; }
        public string CallChain { get; }
        public IRuleArgumentToken[] Arguments { get; }

        public NerToken(string variableName, string callChain, IRuleArgumentToken[] arguments)
        {
            this.VariableName = variableName;
            this.CallChain = callChain;
            this.Arguments = arguments;
        }

        public override string ToString()
        {
            return
                $"<{this.VariableName} = {this.CallChain}({this.Arguments.Select(argument => argument.ToString()).JoinToString(", ")})>";
        }
    }
}