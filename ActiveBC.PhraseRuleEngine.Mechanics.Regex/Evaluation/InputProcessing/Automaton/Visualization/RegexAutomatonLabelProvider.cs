using System;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.States;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.Transitions;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Payload;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Label;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.TerminalDetectors;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Visualization
{
    internal sealed class RegexAutomatonLabelProvider : ILabelProvider<RegexAutomatonState, RegexAutomatonTransition>
    {
        public static readonly ILabelProvider<RegexAutomatonState, RegexAutomatonTransition> Instance = new RegexAutomatonLabelProvider();

        public string GetLabel(RegexAutomatonState vertex)
        {
            return $"[{vertex.Id}]";
        }

        public string GetLabel(RegexAutomatonTransition edge)
        {
            string label = edge.Payload switch
            {
                EpsilonPayload => "ε",
                MarkerPayload payload => $"「{payload.Marker}」",
                NerPayload payload => FormatNerPayload(payload),
                RuleReferencePayload payload => FormatReferencePayload(payload),
                TerminalPayload payload => FormatTerminalPayload(payload),
                VariableCapturePayload payload => $":{payload.VariableName}",
                _ => throw new ArgumentOutOfRangeException(nameof(edge))
            };

            return $"{label} ({edge.Id})";
        }

        private string FormatNerPayload(NerPayload payload)
        {
            return $"{FormatRule(payload.RuleKey, payload.RuleArguments)} (extract)";
        }

        private string FormatReferencePayload(RuleReferencePayload payload)
        {
            return $"{FormatRule(payload.RuleKey, payload.RuleArguments)} (forward)";
        }

        private static string FormatRule(string ruleKey, IRuleArgumentToken[] ruleArguments)
        {
            string formattedPayload = ruleKey;

            if (ruleArguments.Length > 0)
            {
                formattedPayload = $"{formattedPayload}({ruleArguments.Select(FormatRuleArgument).JoinToString(", ")})";
            }

            return formattedPayload;

            string FormatRuleArgument(IRuleArgumentToken ruleArgument)
            {
                return ruleArgument switch
                {
                    RuleChainedMemberAccessArgumentToken token => token.ToString(),
                    RuleDefaultArgumentToken => "default",
                    _ => throw new ArgumentOutOfRangeException(nameof(ruleArgument))
                };
            }
        }

        private string FormatTerminalPayload(TerminalPayload payload)
        {
            return payload.TerminalDetector switch
            {
                AnyLiteralDetector => ".",
                LiteralDetector detector => FormatLiteral(detector.Literal),
                PrefixDetector detector => FormatPrefix(detector.Prefix),
                InfixDetector detector => FormatInfix(detector.Infix),
                SuffixDetector detector => FormatSuffix(detector.Suffix),
                LiteralSetDetector detector => $"[{(detector.IsNegative ? "^" : "")}{detector.Members.Select(FormatLiteralSetMember).JoinToString(" ")}]",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private string FormatLiteralSetMember((LiteralSetDetector.MemberType Type, string Value) member)
        {
            Func<string, string> formatter = member.Type switch
            {
                LiteralSetDetector.MemberType.Literal => FormatLiteral,
                LiteralSetDetector.MemberType.Prefix => FormatPrefix,
                LiteralSetDetector.MemberType.Infix => FormatInfix,
                LiteralSetDetector.MemberType.Suffix => FormatSuffix,
                _ => throw new ArgumentOutOfRangeException()
            };

            return formatter(member.Value);
        }

        private static string FormatLiteral(string literal)
        {
            return literal;
        }

        private static string FormatPrefix(string prefix)
        {
            return $"{prefix}~";
        }

        private static string FormatInfix(string infix)
        {
            return $"~{infix}~";
        }

        private static string FormatSuffix(string suffix)
        {
            return $"~{suffix}";
        }
    }
}