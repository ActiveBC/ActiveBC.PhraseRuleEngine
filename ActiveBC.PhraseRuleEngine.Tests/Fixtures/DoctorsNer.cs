using System;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

namespace ActiveBC.PhraseRuleEngine.Tests.Fixtures
{
    internal static class DoctorsNer
    {
        public static readonly RuleSetContainer Instance = new RuleSetContainer(
            @"
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

string Doctor = peg#($Therapeutic:d_0|$Chirurgeon:d_1|$Oculist:d_2|$Otolaryngologist:d_3|$Urologist:d_4|$WomanGynecologist:d_5|$ManGynecologist:d_6|$Gynecologist:d_7|$Neurologist:d_8|$Traumatologist:d_9|$Dermatologist:d_10|$Gastroenterologist:d_11|$Cardiologist:d_12|$Endocrinologist:d_13|$TeethCleaning:d_14|$DentistTherapist:d_15|$DentistSurgeon:d_16|$DentistOrthodontist:d_17|$Dentist:d_18)# { return Pick.OneOf(d_0, d_1, d_2, d_3, d_4, d_5, d_6, d_7, d_8, d_9, d_10, d_11, d_12, d_13, d_14, d_15, d_16, d_17, d_18); }
string Therapeutic = peg#(терапевт~)# { return ""терапевту""; }
string Chirurgeon = peg#(хирург~)# { return ""хирургу""; }
string Oculist = peg#([окулист~ глазн~ офтальмолог~])# { return ""офтальмологу""; }
string Otolaryngologist = peg#([отоларинголог~ лор~ ухогорл~ горлонос~ горлунос~]|ухо горло нос|ухо горло носу|горл~ нос~)# { return ""отоларингологу""; }
string Urologist = peg#(уролог~)# { return ""урологу""; }
string WomanGynecologist = peg#(женщин~ гинеколог~|гинеколог~ женщин~|[женщине-гинекологу женщина-гинеколог гинеколог-женщина гинекологу-женщине])# { return ""женщине гинекологу""; }
string ManGynecologist = peg#(мужчин~ гинеколог~|гинеколог~ мужчин~|[мужчине-гинекологу мужчина-гинеколог гинеколог-мужчина гинекологу-мужчине])# { return ""мужчине гинекологу""; }
string Gynecologist = peg#([гинеколог~ женск~])# { return ""гинекологу""; }
string Neurologist = peg#(невролог~)# { return ""неврологу""; }
string Traumatologist = peg#(травматолог~)# { return ""травматологу""; }
string Dermatologist = peg#([кожн~ дерматолог~])# { return ""дерматологу""; }
string Gastroenterologist = peg#(гастроэнтеролог~)# { return ""гастроэнтерологу""; }
string Cardiologist = peg#([кардиолог~ сердечн~])# { return ""кардиологу""; }
string Endocrinologist = peg#(эндокринолог~)# { return ""эндокринологу""; }
string TeethCleaning = peg#(на? гигиеническ~? [очистк~ чистк~] зубов?|[почисти~ очисти~] зубы|зубы [почисти~ очисти~])# { return ""специалисту по гигиенической чистке зубов""; }
string DentistTherapist = peg#(стоматолог~ терапевт~|[стоматолог-терапевт стоматолог-терапевту стоматологу-терапевту])# { return ""стоматологу-терапевту""; }
string DentistSurgeon = peg#(стоматолог~ хирург~|[стоматолог-хирург стоматолог-хирургу стоматологу-хирургу])# { return ""стоматологу-хирургу""; }
string DentistOrthodontist = peg#(стоматолог~? ортодонт~|[стоматолог-ортодонт стоматолог-ортодонту стоматологу-ортодонту])# { return ""стоматологу-ортодонту""; }
string Dentist = peg#([стоматолог~ дантист~ зубно~])# { return ""стоматологу""; }",
            new RuleSetToken(
                new []
                {
                    new UsingToken("ActiveBC.PhraseRuleEngine.Tests.Helpers"),
                },
                new []
                {
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Doctor",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Therapeutic", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_0",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Chirurgeon", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_1",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Oculist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_2",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Otolaryngologist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_3",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Urologist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_4",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "WomanGynecologist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_5",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "ManGynecologist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_6",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Gynecologist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_7",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Neurologist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_8",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Traumatologist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_9",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Dermatologist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_10",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Gastroenterologist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_11",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Cardiologist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_12",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Endocrinologist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_13",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "TeethCleaning", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_14",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "DentistTherapist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_15",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "DentistSurgeon", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_16",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "DentistOrthodontist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_17",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Dentist", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "d_18",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken(@"{ return Pick.OneOf(d_0, d_1, d_2, d_3, d_4, d_5, d_6, d_7, d_8, d_9, d_10, d_11, d_12, d_13, d_14, d_15, d_16, d_17, d_18); }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Therapeutic",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new PrefixToken("терапевт"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken(@"{ return ""терапевту""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Chirurgeon",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new PrefixToken("хирург"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken(@"{ return ""хирургу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Oculist",
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
                                                    new PrefixToken("окулист"),
                                                    new PrefixToken("глазн"),
                                                    new PrefixToken("офтальмолог"),
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
                        new BodyBasedProjectionToken(@"{ return ""офтальмологу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Otolaryngologist",
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
                                                    new PrefixToken("отоларинголог"),
                                                    new PrefixToken("лор"),
                                                    new PrefixToken("ухогорл"),
                                                    new PrefixToken("горлонос"),
                                                    new PrefixToken("горлунос"),
                                                }
                                            ),
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
                                            new LiteralToken("ухо"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("горло"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("нос"),
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
                                            new LiteralToken("ухо"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("горло"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("носу"),
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
                                            new PrefixToken("горл"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new PrefixToken("нос"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken(@"{ return ""отоларингологу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Urologist",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new PrefixToken("уролог"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken(@"{ return ""урологу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "WomanGynecologist",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new PrefixToken("женщин"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new PrefixToken("гинеколог"),
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
                                            new PrefixToken("гинеколог"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new PrefixToken("женщин"),
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
                                            new LiteralSetToken(
                                                false,
                                                new ILiteralSetMemberToken[]
                                                {
                                                    new LiteralToken("женщине-гинекологу"),
                                                    new LiteralToken("женщина-гинеколог"),
                                                    new LiteralToken("гинеколог-женщина"),
                                                    new LiteralToken("гинекологу-женщине"),
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
                        new BodyBasedProjectionToken(@"{ return ""женщине гинекологу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "ManGynecologist",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new PrefixToken("мужчин"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new PrefixToken("гинеколог"),
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
                                            new PrefixToken("гинеколог"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new PrefixToken("мужчин"),
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
                                            new LiteralSetToken(
                                                false,
                                                new ILiteralSetMemberToken[]
                                                {
                                                    new LiteralToken("мужчине-гинекологу"),
                                                    new LiteralToken("мужчина-гинеколог"),
                                                    new LiteralToken("гинеколог-мужчина"),
                                                    new LiteralToken("гинекологу-мужчине"),
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
                        new BodyBasedProjectionToken(@"{ return ""мужчине гинекологу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Gynecologist",
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
                                                    new PrefixToken("гинеколог"),
                                                    new PrefixToken("женск"),
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
                        new BodyBasedProjectionToken(@"{ return ""гинекологу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Neurologist",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new PrefixToken("невролог"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken(@"{ return ""неврологу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Traumatologist",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new PrefixToken("травматолог"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken(@"{ return ""травматологу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Dermatologist",
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
                                                    new PrefixToken("кожн"),
                                                    new PrefixToken("дерматолог"),
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
                        new BodyBasedProjectionToken(@"{ return ""дерматологу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Gastroenterologist",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new PrefixToken("гастроэнтеролог"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken(@"{ return ""гастроэнтерологу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Cardiologist",
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
                                                    new PrefixToken("кардиолог"),
                                                    new PrefixToken("сердечн"),
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
                        new BodyBasedProjectionToken(@"{ return ""кардиологу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Endocrinologist",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new PrefixToken("эндокринолог"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken(@"{ return ""эндокринологу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "TeethCleaning",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("на"),
                                            new QuantifierToken(0, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new PrefixToken("гигиеническ"),
                                            new QuantifierToken(0, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralSetToken(
                                                false,
                                                new ILiteralSetMemberToken[]
                                                {
                                                    new PrefixToken("очистк"),
                                                    new PrefixToken("чистк"),
                                                }
                                            ),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("зубов"),
                                            new QuantifierToken(0, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralSetToken(
                                                false,
                                                new ILiteralSetMemberToken[]
                                                {
                                                    new PrefixToken("почисти"),
                                                    new PrefixToken("очисти"),
                                                }
                                            ),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("зубы"),
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
                                            new LiteralToken("зубы"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralSetToken(
                                                false,
                                                new ILiteralSetMemberToken[]
                                                {
                                                    new PrefixToken("почисти"),
                                                    new PrefixToken("очисти"),
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
                        new BodyBasedProjectionToken(@"{ return ""специалисту по гигиенической чистке зубов""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "DentistTherapist",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new PrefixToken("стоматолог"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new PrefixToken("терапевт"),
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
                                            new LiteralSetToken(
                                                false,
                                                new ILiteralSetMemberToken[]
                                                {
                                                    new LiteralToken("стоматолог-терапевт"),
                                                    new LiteralToken("стоматолог-терапевту"),
                                                    new LiteralToken("стоматологу-терапевту"),
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
                        new BodyBasedProjectionToken(@"{ return ""стоматологу-терапевту""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "DentistSurgeon",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new PrefixToken("стоматолог"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new PrefixToken("хирург"),
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
                                            new LiteralSetToken(
                                                false,
                                                new ILiteralSetMemberToken[]
                                                {
                                                    new LiteralToken("стоматолог-хирург"),
                                                    new LiteralToken("стоматолог-хирургу"),
                                                    new LiteralToken("стоматологу-хирургу"),
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
                        new BodyBasedProjectionToken(@"{ return ""стоматологу-хирургу""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "DentistOrthodontist",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new PrefixToken("стоматолог"),
                                            new QuantifierToken(0, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new PrefixToken("ортодонт"),
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
                                            new LiteralSetToken(
                                                false,
                                                new ILiteralSetMemberToken[]
                                                {
                                                    new LiteralToken("стоматолог-ортодонт"),
                                                    new LiteralToken("стоматолог-ортодонту"),
                                                    new LiteralToken("стоматологу-ортодонту"),
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
                        new BodyBasedProjectionToken(@"{ return ""стоматологу-ортодонту""; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                        "Dentist",
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
                                                    new PrefixToken("стоматолог"),
                                                    new PrefixToken("дантист"),
                                                    new PrefixToken("зубно"),
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
                        new BodyBasedProjectionToken(@"{ return ""стоматологу""; }")
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