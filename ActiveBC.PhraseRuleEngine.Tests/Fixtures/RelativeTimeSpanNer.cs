using System;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

namespace ActiveBC.PhraseRuleEngine.Tests.Fixtures
{
    // problematic cases:
    // "... ровно" ("ровно" is not a part of the grammar)
    // "через минуту" ("минуту" should be treated same way as "час")
    // "два две" (shouldn't be parsed in HoursAndMinutes_WithoutWords; but "два ноль две" should be)
    // "десять пятнадцать" (shouldn't be parsed in HoursAndMinutes_WithoutWords; but "десять ноль две" and "два пятндадцать" should be)
    internal static class RelativeTimeSpanNer
    {
        public static readonly RuleSetContainer Instance = new RuleSetContainer(
@"using System;

// root expression
TimeSpan Expression = peg#($BeforeOrAfter:multiplier $TimeSpan:timeSpan)# {
    return timeSpan.Multiply(multiplier);
}

// direction modifier
int BeforeOrAfter = peg#($Before:before|$After:after)# { return (before ?? 0) + (after ?? 0); }
int Before = peg#(за)# { return -1; }
int After = peg#(через)# { return 1; }

// alternatives of possible relative time phrase
TimeSpan TimeSpan = peg#($HoursExact:hoursExact|$HoursAndMinutes_WithWords:hmWithWords|$HoursAndMinutes_WithoutWords:hmNoWords|$HoursOnly_WithWord:h|$MinutesOnly_WithWord:m)# { return (hoursExact ?? hmWithWords ?? hmNoWords ?? h ?? m).Value; }

// different patterns for relative time phrase
TimeSpan HoursExact = peg#($HoursNumber_HalfAnHour:special|$HoursExact_WithHalf:common)# { return (special ?? common).Value; }
TimeSpan HoursExact_WithHalf = peg#($Doubles_Halves_After_0_12:hours $Word_Hour__S_Nom_S_Gen_P_Gen)# { return TimeSpan.FromHours(hours); }
TimeSpan HoursAndMinutes_WithWords = peg#($HoursNumber_WithWord:hours $MinutesNumber_WithWord:minutes)# { return hours.Add(minutes); }
TimeSpan HoursAndMinutes_WithoutWords = peg#($HoursNumber_WithoutExtraWord:hours $MinutesNumber_WithoutWord:minutes)# { return hours.Add(minutes); }
TimeSpan HoursOnly_WithWord = peg#($HoursNumber_WithWord:hours)# { return hours; }
TimeSpan MinutesOnly_WithWord = peg#($MinutesNumber_WithWord:minutes)# { return minutes; }

// minutes helpers
TimeSpan MinutesNumber_WithWord = peg#($MinutesNumber_WithoutWord:number $Word_Minute__S_Nom_S_Gen_P_Gen)# { return number; }
TimeSpan MinutesNumber_WithoutWord = peg#($Integer_0? $Integers_0_9__F_Acc:n_0_9|$Integers_10_19:n_10_19|$Integers_20_59__F_Acc:n_20_59)# { return TimeSpan.FromMinutes((n_0_9 ?? n_10_19 ?? n_20_59).Value); }

// hours helpers
TimeSpan HoursNumber_WithWord = peg#($HoursNumber_WithoutWord:common $Word_Hour__S_Nom_S_Gen_P_Gen|$HoursNumber_SingleHour:special)# { return (common ?? special).Value; }
TimeSpan HoursNumber_WithoutWord = peg#($Integers_0_12__M_Acc:hours)# { return TimeSpan.FromHours(hours); }
TimeSpan HoursNumber_WithoutExtraWord = peg#($HoursNumber_WithoutWord:hours|$HoursNumber_SingleHour:hour)# { return (hours ?? hour).Value; }
TimeSpan HoursNumber_SingleHour = peg#(час)# { return TimeSpan.FromHours(1); }
TimeSpan HoursNumber_HalfAnHour = peg#(полчаса)# { return TimeSpan.FromMinutes(30); }

// numeric helpers
// legend:
// if the word doesn't distinguish significant (in the context of current PEG-grammar) grammatical categories
// (such as declension, grammatical gender or grammatical number),
// the respective rule is not being marked with additional suffix, otherwise it is:
// ""F"" and ""M"" stand for feminine and masculine grammatical genders respectively
// ""S"" and ""P"" stand for singular and plural grammatical numbers respectively
// ""Nom"" and ""Acc"" stand for nominative and accusative cases respectively
int Integers_0_9__F_Acc = peg#($Integer_0:n_0|$Integers_1_9__F_Acc:n_1_9)# { return (n_0 ?? n_1_9).Value; }
int Integers_1_9__F_Acc = peg#($Integer_1__F_Acc:n_1|$Integer_2__F_Acc:n_2|$Integers_3_9:n_3_9)# { return (n_1 ?? n_2 ?? n_3_9).Value; }
int Integers_20_59__F_Acc = peg#($Integers_Tens_20_50:tens $Integers_1_9__F_Acc?:ones)# { return tens + (ones ?? 0); }
int Integers_0_9__M_Acc = peg#($Integer_0:n_0|$Integer_1__M_Acc:n_1|$Integer_2__M_Acc:n_2|$Integers_3_9:n_3_9)# { return (n_0 ?? n_1 ?? n_2 ?? n_3_9).Value; }
int Integers_0_12__M_Acc = peg#($Integers_0_9__M_Acc:n_0_9|$Integer_10:n_10|$Integer_11:n_11|$Integer_12:n_12)# { return (n_0_9 ?? n_10 ?? n_11 ?? n_12).Value; }
int Integers_3_9 = peg#($Integer_3:n_3|$Integer_4:n_4|$Integer_5:n_5|$Integer_6:n_6|$Integer_7:n_7|$Integer_8:n_8|$Integer_9:n_9)# { return (n_3 ?? n_4 ?? n_5 ?? n_6 ?? n_7 ?? n_8 ?? n_9).Value; }
int Integers_10_19 = peg#($Integer_10:n_10|$Integer_11:n_11|$Integer_12:n_12|$Integer_13:n_13|$Integer_14:n_14|$Integer_15:n_15|$Integer_16:n_16|$Integer_17:n_17|$Integer_18:n_18|$Integer_19:n_19)# { return (n_10 ?? n_11 ?? n_12 ?? n_13 ?? n_14 ?? n_15 ?? n_16 ?? n_17 ?? n_18 ?? n_19).Value; }

int Integer_0 = peg#(ноль)# { return 0; }
int Integer_1__M_Acc = peg#(один)# { return 1; }
int Integer_1__F_Acc = peg#(одну)# { return 1; }
int Integer_2__M_Acc = peg#(два)# { return 2; }
int Integer_2__F_Acc = peg#(две)# { return 2; }
int Integer_3 = peg#(три)# { return 3; }
int Integer_4 = peg#(четыре)# { return 4; }
int Integer_5 = peg#(пять)# { return 5; }
int Integer_6 = peg#(шесть)# { return 6; }
int Integer_7 = peg#(семь)# { return 7; }
int Integer_8 = peg#(восемь)# { return 8; }
int Integer_9 = peg#(девять)# { return 9; }
int Integer_10 = peg#(десять)# { return 10; }
int Integer_11 = peg#(одиннадцать)# { return 11; }
int Integer_12 = peg#(двенадцать)# { return 12; }
int Integer_13 = peg#(тринадцать)# { return 13; }
int Integer_14 = peg#(четырнадцать)# { return 14; }
int Integer_15 = peg#(пятнадцать)# { return 15; }
int Integer_16 = peg#(шестнадцать)# { return 16; }
int Integer_17 = peg#(семьнадцать)# { return 17; }
int Integer_18 = peg#(восемьнадцать)# { return 18; }
int Integer_19 = peg#(девятнадцать)# { return 19; }

int Integers_Tens_20_50 = peg#($Integers_Tens_20:n_20|$Integers_Tens_30:n_30|$Integers_Tens_40:n_40|$Integers_Tens_50:n_50)# { return (n_20 ?? n_30 ?? n_40 ?? n_50).Value; }
int Integers_Tens_20 = peg#(двадцать)# { return 20; }
int Integers_Tens_30 = peg#(тридцать)# { return 30; }
int Integers_Tens_40 = peg#(сорок)# { return 40; }
int Integers_Tens_50 = peg#(пятьдесят)# { return 50; }

double Doubles_Halves_After_0_12 = peg#($Doubles_Halves_After_0_Special:n_0|$Doubles_Halves_After_1_Special:n_1|$Doubles_Halves_After_0_12_Common:n_0_12)# { return (n_0 ?? n_1 ?? n_0_12).Value; }
double Doubles_Halves_After_0_Special = peg#(пол)# { return 0.5; }
double Doubles_Halves_After_1_Special = peg#(полтора)# { return 1.5; }
double Doubles_Halves_After_0_12_Common = peg#($Integers_0_12__M_Acc:n с половиной)# { return n + 0.5; }

// lexical helpers
void Word_Minute__S_Nom_S_Gen_P_Gen = peg#([минута минуты минут])# {}
void Word_Hour__S_Nom_S_Gen_P_Gen = peg#([час часа часов])# {}",
            new RuleSetToken(
                new []
                {
                    new UsingToken("System"),
                },
                new []
                {
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "Expression",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "BeforeOrAfter", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "multiplier",
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "TimeSpan", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "timeSpan",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken(@"{
    return timeSpan.Multiply(multiplier);
}")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "BeforeOrAfter",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Before", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "before",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "After", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "after",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return (before ?? 0) + (after ?? 0); }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Before",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("за"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return -1; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "After",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("через"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 1; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "TimeSpan",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "HoursExact", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "hoursExact",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "HoursAndMinutes_WithWords", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "hmWithWords",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "HoursAndMinutes_WithoutWords", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "hmNoWords",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "HoursOnly_WithWord", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "h",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "MinutesOnly_WithWord", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "m",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return (hoursExact ?? hmWithWords ?? hmNoWords ?? h ?? m).Value; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "HoursExact",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "HoursNumber_HalfAnHour", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "special",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "HoursExact_WithHalf", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "common",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return (special ?? common).Value; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "HoursExact_WithHalf",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Doubles_Halves_After_0_12", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "hours",
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Word_Hour__S_Nom_S_Gen_P_Gen", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return TimeSpan.FromHours(hours); }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "HoursAndMinutes_WithWords",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "HoursNumber_WithWord", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "hours",
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "MinutesNumber_WithWord", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "minutes",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return hours.Add(minutes); }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "HoursAndMinutes_WithoutWords",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "HoursNumber_WithoutExtraWord", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "hours",
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "MinutesNumber_WithoutWord", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "minutes",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return hours.Add(minutes); }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "HoursOnly_WithWord",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "HoursNumber_WithWord", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "hours",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return hours; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "MinutesOnly_WithWord",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "MinutesNumber_WithWord", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "minutes",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return minutes; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "MinutesNumber_WithWord",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "MinutesNumber_WithoutWord", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "number",
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Word_Minute__S_Nom_S_Gen_P_Gen", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return number; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "MinutesNumber_WithoutWord",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_0", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(0, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_0_9__F_Acc", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_0_9",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_10_19", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_10_19",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_20_59__F_Acc", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_20_59",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return TimeSpan.FromMinutes((n_0_9 ?? n_10_19 ?? n_20_59).Value); }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "HoursNumber_WithWord",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "HoursNumber_WithoutWord", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "common",
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Word_Hour__S_Nom_S_Gen_P_Gen", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "HoursNumber_SingleHour", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "special",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return (common ?? special).Value; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "HoursNumber_WithoutWord",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_0_12__M_Acc", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "hours",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return TimeSpan.FromHours(hours); }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "HoursNumber_WithoutExtraWord",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "HoursNumber_WithoutWord", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "hours",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "HoursNumber_SingleHour", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "hour",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return (hours ?? hour).Value; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "HoursNumber_SingleHour",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("час"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return TimeSpan.FromHours(1); }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("TimeSpan"), Array.Empty<ICSharpTypeToken>()),
                        "HoursNumber_HalfAnHour",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("полчаса"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return TimeSpan.FromMinutes(30); }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integers_0_9__F_Acc",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_0", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_0",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_1_9__F_Acc", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_1_9",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return (n_0 ?? n_1_9).Value; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integers_1_9__F_Acc",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_1__F_Acc", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_1",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_2__F_Acc", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_2",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_3_9", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_3_9",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return (n_1 ?? n_2 ?? n_3_9).Value; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integers_20_59__F_Acc",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_Tens_20_50", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "tens",
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_1_9__F_Acc", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(0, 1),
                                            "ones",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return tens + (ones ?? 0); }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integers_0_9__M_Acc",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_0", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_0",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_1__M_Acc", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_1",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_2__M_Acc", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_2",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_3_9", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_3_9",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return (n_0 ?? n_1 ?? n_2 ?? n_3_9).Value; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integers_0_12__M_Acc",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_0_9__M_Acc", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_0_9",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_10", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_10",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_11", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_11",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_12", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_12",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return (n_0_9 ?? n_10 ?? n_11 ?? n_12).Value; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integers_3_9",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_3", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_3",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_4", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_4",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_5", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_5",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_6", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_6",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_7", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_7",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_8", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_8",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_9", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_9",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return (n_3 ?? n_4 ?? n_5 ?? n_6 ?? n_7 ?? n_8 ?? n_9).Value; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integers_10_19",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_10", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_10",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_11", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_11",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_12", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_12",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_13", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_13",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_14", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_14",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_15", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_15",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_16", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_16",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_17", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_17",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_18", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_18",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integer_19", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_19",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken(
                            "{ return (n_10 ?? n_11 ?? n_12 ?? n_13 ?? n_14 ?? n_15 ?? n_16 ?? n_17 ?? n_18 ?? n_19).Value; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_0",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("ноль"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 0; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_1__M_Acc",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("один"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 1; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_1__F_Acc",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("одну"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 1; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_2__M_Acc",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("два"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 2; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_2__F_Acc",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("две"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 2; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_3",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("три"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 3; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_4",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("четыре"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 4; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_5",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("пять"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 5; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_6",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("шесть"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 6; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_7",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("семь"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 7; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_8",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("восемь"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 8; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_9",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("девять"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 9; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_10",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("десять"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 10; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_11",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("одиннадцать"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 11; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_12",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("двенадцать"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 12; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_13",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("тринадцать"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 13; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_14",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("четырнадцать"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 14; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_15",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("пятнадцать"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 15; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_16",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("шестнадцать"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 16; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_17",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("семьнадцать"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 17; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_18",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("восемьнадцать"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 18; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integer_19",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("девятнадцать"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 19; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integers_Tens_20_50",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_Tens_20", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_20",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_Tens_30", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_30",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_Tens_40", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_40",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_Tens_50", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_50",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return (n_20 ?? n_30 ?? n_40 ?? n_50).Value; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integers_Tens_20",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("двадцать"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 20; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integers_Tens_30",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("тридцать"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 30; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integers_Tens_40",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("сорок"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 40; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Integers_Tens_50",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("пятьдесят"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 50; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("double"), Array.Empty<ICSharpTypeToken>()),
                        "Doubles_Halves_After_0_12",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Doubles_Halves_After_0_Special", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_0",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Doubles_Halves_After_1_Special", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_1",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Doubles_Halves_After_0_12_Common", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_0_12",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return (n_0 ?? n_1 ?? n_0_12).Value; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("double"), Array.Empty<ICSharpTypeToken>()),
                        "Doubles_Halves_After_0_Special",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("пол"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 0.5; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("double"), Array.Empty<ICSharpTypeToken>()),
                        "Doubles_Halves_After_1_Special",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("полтора"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 1.5; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("double"), Array.Empty<ICSharpTypeToken>()),
                        "Doubles_Halves_After_0_12_Common",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Integers_0_12__M_Acc", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n",
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("с"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("половиной"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return n + 0.5; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                        "Word_Minute__S_Nom_S_Gen_P_Gen",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralSetToken(
                                                false,
                                                new ILiteralSetMemberToken[]
                                                {
                                                    new LiteralToken("минута"),
                                                    new LiteralToken("минуты"),
                                                    new LiteralToken("минут"),
                                                }
                                            ),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        VoidProjectionToken.Instance
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                        "Word_Hour__S_Nom_S_Gen_P_Gen",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralSetToken(
                                                false,
                                                new ILiteralSetMemberToken[]
                                                {
                                                    new LiteralToken("час"),
                                                    new LiteralToken("часа"),
                                                    new LiteralToken("часов"),
                                                }
                                            ),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        VoidProjectionToken.Instance
                    ),
                }
            ),
            new []
            {
                NerEnvironment.Mechanics.Peg,
            }
        );
    }
}