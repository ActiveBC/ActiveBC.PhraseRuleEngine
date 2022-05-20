Phrase Rule Engine (PRE) is a pattern matching library, which allows including multiple mechanics to detect if the phrase matches the some specific rule.

## Basic terms

**Phrase** is represented as an array of strings, for example phrase "Hello world" is represented by an array, containing two elements: "Hello" and "world".

**Rule** is an abstraction, which allows to specify some constraints and check if the phrase is matching those constraints. Specific logic of matching is handled by specific mechanics.

**Mechanics** is a concept which allows PRE to be agnostic of any specific procedures of matching the phrase with the pattern. Each mechanics provides a specific input processor class.

This repository contains two mechanics:
- PEG, which represents the logic of [Parsing Expression Grammars](https://en.wikipedia.org/wiki/Parsing_expression_grammar)
- Regex, which represents the logic of [Regular Expressions](https://en.wikipedia.org/wiki/Regular_expression)

Please note, that the classic applications of PEG and Regex assume that the input is represented by a string, and the simplest unit of it - a symbol - is a single character. This library works in a paradigm, where the input (a phrase) is an array of strings, and the simplest unit - a symbol - is a word.

**Input Processor** is a part of each mechanics which handles input.

**Projection** is an abstraction, which allows to map input processing result to any other value.

**Rule Space** is a concept of grouping rule matchers in such way, that all the rules are exposed to each other. If the mechanics of rule supports referencing other rules (which is true for both Peg and Regex mechanics in this repository), one rule can be referenced by another.

**Rule Matcher** is an abstraction of "built", ready to use rule.

**Tokenization** is the process of converting string representation of the pattern to its object model. Tokenization is implemented by a specific mechanics.

**Rule match result collection** is an abstraction of grouping match results (as there could be more than one result of matching the phrase with the rule).

**Rule match result** contains all the information about match result (if it is successful).

## Library structure

This repository contains three packages:
- `ActiveBC.PhraseRuleEngine` - core library, which is responsible for all the abstractions such as rule space, rule, rule matcher, etc.
  - unit test for this package can be found here: `ActiveBC.PhraseRuleEngine.Tests`
- `ActiveBC.PhraseRuleEngine.Mechanics.Peg` - implementation of PEG mechanics
  - unit test for this package can be found here: `ActiveBC.PhraseRuleEngine.Mechanics.Peg.Tests`
- `ActiveBC.PhraseRuleEngine.Mechanics.Regex` - implementation of PEG mechanics
  - unit test for this package can be found here: `ActiveBC.PhraseRuleEngine.Mechanics.Regex.Tests`

## Benchmarking

For each of the library components there are benchmarks written with `BenchmarkDotNet`:
- `ActiveBC.PhraseRuleEngine.Benchmarking`
- `ActiveBC.PhraseRuleEngine.Mechanics.Peg.Benchmarking`
- `ActiveBC.PhraseRuleEngine.Mechanics.Regex.Benchmarking`

## Usage examples

```csharp
using System;
using System.Collections.Immutable;
using ActiveBC.PhraseRuleEngine;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.Common;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing.Automaton.Optimization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;

// custom implementation of string interner is used to make it possible to "deintern string"
StringInterner stringInterner = new StringInterner();

// creating rule space factory with two mechanics - "peg" and "regex"
RuleSpaceFactory ruleSpaceFactory = new RuleSpaceFactory(
    new[]
    {
        new MechanicsBundle(
            "peg",
            new LoopBasedPegPatternTokenizer(stringInterner),
            new PegProcessorFactory(
                new CombinedStrategy(
                    new IResultSelectionStrategy[]
                    {
                        new MaxExplicitSymbolsStrategy(),
                        new MaxProgressStrategy(),
                    }
                )
            ),
            typeof(ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens.PegGroupToken)
        ),
        new MechanicsBundle(
            "regex",
            new LoopBasedRegexPatternTokenizer(stringInterner),
            new RegexProcessorFactory(OptimizationLevel.Max),
            typeof(RegexGroupToken)
        )
    }
);

// creating rule space from rules
IRuleSpace ruleSpace = ruleSpaceFactory.CreateWithAliases(
    Array.Empty<RuleSetToken>(),
    new []
    {
        // pattern token for this rule is created manually
        new RuleToken(
            null,
            VoidProjectionToken.ReturnType,
            "is_greeting",
            Array.Empty<CSharpParameterToken>(),
            "regex",
            // this token represents the regex which has string representation "(hello|good morning)" and can be tokenized from that string
            new RegexGroupToken(
                new []
                {
                    new BranchToken(
                        new IBranchItemToken[]
                        {
                            new QuantifiableBranchItemToken(new LiteralToken("hello"), new QuantifierToken(1, 1), null),
                        }
                    ),
                    new BranchToken(
                        new IBranchItemToken[]
                        {
                            new QuantifiableBranchItemToken(new LiteralToken("good"), new QuantifierToken(1, 1), null),
                            new QuantifiableBranchItemToken(new LiteralToken("morning"), new QuantifierToken(1, 1), null),
                        }
                    ),
                }
            ),
            VoidProjectionToken.Instance
        ),
        // pattern token for this rule is created with tokenization procedure from raw string
        new RuleToken(
            null,
            VoidProjectionToken.ReturnType,
            "is_farewell",
            Array.Empty<CSharpParameterToken>(),
            "regex",
            ruleSpaceFactory.PatternTokenizers["regex"].Tokenize("(good? bye|farewell)", null, false),
            VoidProjectionToken.Instance
        )
    },
    ImmutableDictionary<string, IRuleMatcher>.Empty,
    ImmutableDictionary<string, IRuleSpace>.Empty,
    ImmutableDictionary<string, Type>.Empty,
    new LoadedAssembliesProvider()
);

// setting up transient variables
RuleSpaceArguments ruleSpaceArguments = new RuleSpaceArguments(ImmutableDictionary<string, object?>.Empty);
RuleSpaceCache ruleSpaceCache = new RuleSpaceCache();

// testing matchers

// greeting matcher
{
    // getting rule matcher from rule space
    IRuleMatcher greetingMatcher = ruleSpace["is_greeting"];

    // "success1" variable will contain single result
    RuleMatchResultCollection success1 = greetingMatcher.Match(
        new RuleInput(
            new [] {"hello"},
            ruleSpaceArguments
        ),
        0,
        ruleSpaceCache
    );

    // "success2" variable will contain single result
    RuleMatchResultCollection success2 = greetingMatcher.Match(
        new RuleInput(
            new [] {"good", "morning"},
            ruleSpaceArguments
        ),
        0,
        ruleSpaceCache
    );

    // "fail" variable will contain no results
    RuleMatchResultCollection fail = greetingMatcher.Match(
        new RuleInput(
            new [] {"good", "bye"},
            ruleSpaceArguments
        ),
        0,
        ruleSpaceCache
    );
}

// farewell matcher
{
    // getting rule matcher from rule space
    IRuleMatcher farewellMatcher = ruleSpace["is_farewell"];

    // "success1" variable will contain single result
    RuleMatchResultCollection success1 = farewellMatcher.Match(
        new RuleInput(
            new [] {"bye"},
            ruleSpaceArguments
        ),
        0,
        ruleSpaceCache
    );

    // "success2" variable will contain single result
    RuleMatchResultCollection success2 = farewellMatcher.Match(
        new RuleInput(
            new [] {"good", "bye"},
            ruleSpaceArguments
        ),
        0,
        ruleSpaceCache
    );

    // "success3" variable will contain single result
    RuleMatchResultCollection success3 = farewellMatcher.Match(
        new RuleInput(
            new [] {"farewell"},
            ruleSpaceArguments
        ),
        0,
        ruleSpaceCache
    );

    // "fail" variable will contain no results
    RuleMatchResultCollection fail = farewellMatcher.Match(
        new RuleInput(
            new [] {"hello"},
            ruleSpaceArguments
        ),
        0,
        ruleSpaceCache
    );
}
```
