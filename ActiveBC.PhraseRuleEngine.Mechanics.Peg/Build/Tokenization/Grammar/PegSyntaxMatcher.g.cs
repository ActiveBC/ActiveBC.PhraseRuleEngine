//
// IronMeta PegSyntaxMatcher Parser; Generated 2022-05-12 09:42:07Z UTC
//

using System;
using System.Collections.Generic;
using System.Linq;

using IronMeta.Matcher;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens.Internal;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;

#pragma warning disable 0219
#pragma warning disable 1591

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Grammar
{

    using _PegSyntaxMatcher_Inputs = IEnumerable<char>;
    using _PegSyntaxMatcher_Results = IEnumerable<IToken>;
    using _PegSyntaxMatcher_Item = IronMeta.Matcher.MatchItem<char, IToken>;
    using _PegSyntaxMatcher_Args = IEnumerable<IronMeta.Matcher.MatchItem<char, IToken>>;
    using _PegSyntaxMatcher_Memo = IronMeta.Matcher.MatchState<char, IToken>;
    using _PegSyntaxMatcher_Rule = System.Action<IronMeta.Matcher.MatchState<char, IToken>, int, IEnumerable<IronMeta.Matcher.MatchItem<char, IToken>>>;
    using _PegSyntaxMatcher_Base = IronMeta.Matcher.Matcher<char, IToken>;

    public partial class PegSyntaxMatcher : Matcher<char, IToken>
    {
        public PegSyntaxMatcher()
            : base()
        {
            _setTerminals();
        }

        public PegSyntaxMatcher(bool handle_left_recursion)
            : base(handle_left_recursion)
        {
            _setTerminals();
        }

        void _setTerminals()
        {
            this.Terminals = new HashSet<string>()
            {
                "AnyLiteral",
                "COMMENT",
                "CyrillicLetter",
                "CyrillicWord",
                "EOF",
                "EOL",
                "ExplixitQuantifier",
                "ImplicitEmptyRuleArguments",
                "Infix",
                "Integer",
                "LatinLetter",
                "LatinWord",
                "Literal",
                "LiteralSet",
                "LiteralSetMember",
                "Lookahead",
                "NegativeLookahead",
                "OneOrMoreQuantifier",
                "PositiveLookahead",
                "Prefix",
                "Quantifier",
                "QuantityExact",
                "QuantityMin",
                "QuantityRange",
                "RuleDefaultArgument",
                "SPACING",
                "Suffix",
                "Symbol",
                "WHITESPACE",
                "ZeroOrMoreQuantifier",
                "ZeroOrOneQuantifier",
            };
        }


        public void SPACING(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // OR 1
            int _start_i1 = _index;

            // CALLORVAR EOL
            _PegSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "EOL", _index, EOL, null);

            if (_r2 != null) _index = _r2.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i1; } else goto label1;

            // CALLORVAR WHITESPACE
            _PegSyntaxMatcher_Item _r3;

            _r3 = _MemoCall(_memo, "WHITESPACE", _index, WHITESPACE, null);

            if (_r3 != null) _index = _r3.NextIndex;

        label1: // OR
            int _dummy_i1 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR COMMENT
            _PegSyntaxMatcher_Item _r4;

            _r4 = _MemoCall(_memo, "COMMENT", _index, COMMENT, null);

            if (_r4 != null) _index = _r4.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void WHITESPACE(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // INPUT CLASS
            _ParseInputClass(_memo, ref _index, ' ', '\t');

        }


        public void EOL(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // LITERAL "\r\n"
            _ParseLiteralString(_memo, ref _index, "\r\n");

        }


        public void EOF(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // NOT 0
            int _start_i0 = _index;

            // ANY
            _ParseAny(_memo, ref _index);

            // NOT 0
            var _r0 = _memo.Results.Pop();
            _memo.Results.Push( _r0 == null ? new _PegSyntaxMatcher_Item(_start_i0, _memo.InputEnumerable) : null);
            _index = _start_i0;

        }


        public void COMMENT(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // CALLORVAR m_cSharpSyntaxMatcher.CSharpComment
            _PegSyntaxMatcher_Item _r0;

            _r0 = _MemoCall(_memo, "m_cSharpSyntaxMatcher.CSharpComment", _index, m_cSharpSyntaxMatcher.CSharpComment, null);

            if (_r0 != null) _index = _r0.NextIndex;

        }


        public void Pattern(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // AND 0
            int _start_i0 = _index;

            // AND 1
            int _start_i1 = _index;

            // STAR 2
            int _start_i2 = _index;
            var _res2 = Enumerable.Empty<IToken>();
        label2:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r3;

            _r3 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r3 != null) _index = _r3.NextIndex;

            // STAR 2
            var _r2 = _memo.Results.Pop();
            if (_r2 != null)
            {
                _res2 = _res2.Concat(_r2.Results);
                goto label2;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _res2.Where(_NON_NULL), true));
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR Group
            _PegSyntaxMatcher_Item _r4;

            _r4 = _MemoCall(_memo, "Group", _index, Group, null);

            if (_r4 != null) _index = _r4.NextIndex;

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label0; }

            // STAR 5
            int _start_i5 = _index;
            var _res5 = Enumerable.Empty<IToken>();
        label5:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // STAR 5
            var _r5 = _memo.Results.Pop();
            if (_r5 != null)
            {
                _res5 = _res5.Concat(_r5.Results);
                goto label5;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _res5.Where(_NON_NULL), true));
            }

        label0: // AND
            var _r0_2 = _memo.Results.Pop();
            var _r0_1 = _memo.Results.Pop();

            if (_r0_1 != null && _r0_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i0, _index, _memo.InputEnumerable, _r0_1.Results.Concat(_r0_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i0;
            }

        }


        public void Group(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item branches = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // LITERAL '('
            _ParseLiteralChar(_memo, ref _index, '(');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // AND 5
            int _start_i5 = _index;

            // CALLORVAR Branch
            _PegSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "Branch", _index, Branch, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label5; }

            // STAR 7
            int _start_i7 = _index;
            var _res7 = Enumerable.Empty<IToken>();
        label7:

            // AND 8
            int _start_i8 = _index;

            // LITERAL '|'
            _ParseLiteralChar(_memo, ref _index, '|');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label8; }

            // CALLORVAR Branch
            _PegSyntaxMatcher_Item _r10;

            _r10 = _MemoCall(_memo, "Branch", _index, Branch, null);

            if (_r10 != null) _index = _r10.NextIndex;

        label8: // AND
            var _r8_2 = _memo.Results.Pop();
            var _r8_1 = _memo.Results.Pop();

            if (_r8_1 != null && _r8_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i8, _index, _memo.InputEnumerable, _r8_1.Results.Concat(_r8_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i8;
            }

            // STAR 7
            var _r7 = _memo.Results.Pop();
            if (_r7 != null)
            {
                _res7 = _res7.Concat(_r7.Results);
                goto label7;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i7, _index, _memo.InputEnumerable, _res7.Where(_NON_NULL), true));
            }

        label5: // AND
            var _r5_2 = _memo.Results.Pop();
            var _r5_1 = _memo.Results.Pop();

            if (_r5_1 != null && _r5_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _r5_1.Results.Concat(_r5_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i5;
            }

            // BIND branches
            branches = _memo.Results.Peek();

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // LITERAL ')'
            _ParseLiteralChar(_memo, ref _index, ')');

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new PegGroupToken(branches.Results.Cast<BranchToken>().ToArray()); }, _r0), true) );
            }

        }


        public void Branch(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item items = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // STAR 3
            int _start_i3 = _index;
            var _res3 = Enumerable.Empty<IToken>();
        label3:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r4;

            _r4 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r4 != null) _index = _r4.NextIndex;

            // STAR 3
            var _r3 = _memo.Results.Pop();
            if (_r3 != null)
            {
                _res3 = _res3.Concat(_r3.Results);
                goto label3;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _res3.Where(_NON_NULL), true));
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // AND 6
            int _start_i6 = _index;

            // CALLORVAR BranchItem
            _PegSyntaxMatcher_Item _r7;

            _r7 = _MemoCall(_memo, "BranchItem", _index, BranchItem, null);

            if (_r7 != null) _index = _r7.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label6; }

            // STAR 8
            int _start_i8 = _index;
            var _res8 = Enumerable.Empty<IToken>();
        label8:

            // AND 9
            int _start_i9 = _index;

            // PLUS 10
            int _start_i10 = _index;
            var _res10 = Enumerable.Empty<IToken>();
        label10:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r11;

            _r11 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r11 != null) _index = _r11.NextIndex;

            // PLUS 10
            var _r10 = _memo.Results.Pop();
            if (_r10 != null)
            {
                _res10 = _res10.Concat(_r10.Results);
                goto label10;
            }
            else
            {
                if (_index > _start_i10)
                    _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i10, _index, _memo.InputEnumerable, _res10.Where(_NON_NULL), true));
                else
                    _memo.Results.Push(null);
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label9; }

            // CALLORVAR BranchItem
            _PegSyntaxMatcher_Item _r12;

            _r12 = _MemoCall(_memo, "BranchItem", _index, BranchItem, null);

            if (_r12 != null) _index = _r12.NextIndex;

        label9: // AND
            var _r9_2 = _memo.Results.Pop();
            var _r9_1 = _memo.Results.Pop();

            if (_r9_1 != null && _r9_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i9, _index, _memo.InputEnumerable, _r9_1.Results.Concat(_r9_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i9;
            }

            // STAR 8
            var _r8 = _memo.Results.Pop();
            if (_r8 != null)
            {
                _res8 = _res8.Concat(_r8.Results);
                goto label8;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i8, _index, _memo.InputEnumerable, _res8.Where(_NON_NULL), true));
            }

        label6: // AND
            var _r6_2 = _memo.Results.Pop();
            var _r6_1 = _memo.Results.Pop();

            if (_r6_1 != null && _r6_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i6, _index, _memo.InputEnumerable, _r6_1.Results.Concat(_r6_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i6;
            }

            // BIND items
            items = _memo.Results.Peek();

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // STAR 13
            int _start_i13 = _index;
            var _res13 = Enumerable.Empty<IToken>();
        label13:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r14;

            _r14 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r14 != null) _index = _r14.NextIndex;

            // STAR 13
            var _r13 = _memo.Results.Pop();
            if (_r13 != null)
            {
                _res13 = _res13.Concat(_r13.Results);
                goto label13;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i13, _index, _memo.InputEnumerable, _res13.Where(_NON_NULL), true));
            }

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new BranchToken(items.Results.Cast<BranchItemToken>().ToArray()); }, _r0), true) );
            }

        }


        public void BranchItem(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item lookahead = null;
            _PegSyntaxMatcher_Item quantifiable = null;
            _PegSyntaxMatcher_Item quantifier = null;
            _PegSyntaxMatcher_Item variableName = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // AND 3
            int _start_i3 = _index;

            // CALLORVAR Lookahead
            _PegSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "Lookahead", _index, Lookahead, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _PegSyntaxMatcher_Item(_index, _memo.InputEnumerable)); }

            // BIND lookahead
            lookahead = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // CALLORVAR Quantifiable
            _PegSyntaxMatcher_Item _r8;

            _r8 = _MemoCall(_memo, "Quantifiable", _index, Quantifiable, null);

            if (_r8 != null) _index = _r8.NextIndex;

            // BIND quantifiable
            quantifiable = _memo.Results.Peek();

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // CALLORVAR Quantifier
            _PegSyntaxMatcher_Item _r11;

            _r11 = _MemoCall(_memo, "Quantifier", _index, Quantifier, null);

            if (_r11 != null) _index = _r11.NextIndex;

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _PegSyntaxMatcher_Item(_index, _memo.InputEnumerable)); }

            // BIND quantifier
            quantifier = _memo.Results.Peek();

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR VariableCapture
            _PegSyntaxMatcher_Item _r14;

            _r14 = _MemoCall(_memo, "VariableCapture", _index, VariableCapture, null);

            if (_r14 != null) _index = _r14.NextIndex;

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _PegSyntaxMatcher_Item(_index, _memo.InputEnumerable)); }

            // BIND variableName
            variableName = _memo.Results.Peek();

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new BranchItemToken(
            (IQuantifiableToken) quantifiable.Results.Single(),
            quantifier.Results.SingleOrDefault() as QuantifierToken ?? new QuantifierToken(1, 1),
            (variableName.Results.SingleOrDefault() as CSharpIdentifierToken)?.Value,
            lookahead.Results.SingleOrDefault() as LookaheadToken
        ); }, _r0), true) );
            }

        }


        public void Lookahead(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // CALLORVAR PositiveLookahead
            _PegSyntaxMatcher_Item _r1;

            _r1 = _MemoCall(_memo, "PositiveLookahead", _index, PositiveLookahead, null);

            if (_r1 != null) _index = _r1.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR NegativeLookahead
            _PegSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "NegativeLookahead", _index, NegativeLookahead, null);

            if (_r2 != null) _index = _r2.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void PositiveLookahead(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // LITERAL '&'
            _ParseLiteralChar(_memo, ref _index, '&');

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new LookaheadToken(false); }, _r0), true) );
            }

        }


        public void NegativeLookahead(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // LITERAL '!'
            _ParseLiteralChar(_memo, ref _index, '!');

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new LookaheadToken(true); }, _r0), true) );
            }

        }


        public void VariableCapture(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // AND 0
            int _start_i0 = _index;

            // LITERAL ':'
            _ParseLiteralChar(_memo, ref _index, ':');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label0; }

            // CALLORVAR m_cSharpSyntaxMatcher.CSharpIdentifier
            _PegSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "m_cSharpSyntaxMatcher.CSharpIdentifier", _index, m_cSharpSyntaxMatcher.CSharpIdentifier, null);

            if (_r2 != null) _index = _r2.NextIndex;

        label0: // AND
            var _r0_2 = _memo.Results.Pop();
            var _r0_1 = _memo.Results.Pop();

            if (_r0_1 != null && _r0_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i0, _index, _memo.InputEnumerable, _r0_1.Results.Concat(_r0_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i0;
            }

        }


        public void Quantifiable(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // OR 1
            int _start_i1 = _index;

            // OR 2
            int _start_i2 = _index;

            // OR 3
            int _start_i3 = _index;

            // OR 4
            int _start_i4 = _index;

            // OR 5
            int _start_i5 = _index;

            // OR 6
            int _start_i6 = _index;

            // CALLORVAR AnyLiteral
            _PegSyntaxMatcher_Item _r7;

            _r7 = _MemoCall(_memo, "AnyLiteral", _index, AnyLiteral, null);

            if (_r7 != null) _index = _r7.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i6; } else goto label6;

            // CALLORVAR Infix
            _PegSyntaxMatcher_Item _r8;

            _r8 = _MemoCall(_memo, "Infix", _index, Infix, null);

            if (_r8 != null) _index = _r8.NextIndex;

        label6: // OR
            int _dummy_i6 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i5; } else goto label5;

            // CALLORVAR Suffix
            _PegSyntaxMatcher_Item _r9;

            _r9 = _MemoCall(_memo, "Suffix", _index, Suffix, null);

            if (_r9 != null) _index = _r9.NextIndex;

        label5: // OR
            int _dummy_i5 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i4; } else goto label4;

            // CALLORVAR Prefix
            _PegSyntaxMatcher_Item _r10;

            _r10 = _MemoCall(_memo, "Prefix", _index, Prefix, null);

            if (_r10 != null) _index = _r10.NextIndex;

        label4: // OR
            int _dummy_i4 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i3; } else goto label3;

            // CALLORVAR Literal
            _PegSyntaxMatcher_Item _r11;

            _r11 = _MemoCall(_memo, "Literal", _index, Literal, null);

            if (_r11 != null) _index = _r11.NextIndex;

        label3: // OR
            int _dummy_i3 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i2; } else goto label2;

            // CALLORVAR LiteralSet
            _PegSyntaxMatcher_Item _r12;

            _r12 = _MemoCall(_memo, "LiteralSet", _index, LiteralSet, null);

            if (_r12 != null) _index = _r12.NextIndex;

        label2: // OR
            int _dummy_i2 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i1; } else goto label1;

            // CALLORVAR RuleReference
            _PegSyntaxMatcher_Item _r13;

            _r13 = _MemoCall(_memo, "RuleReference", _index, RuleReference, null);

            if (_r13 != null) _index = _r13.NextIndex;

        label1: // OR
            int _dummy_i1 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR Group
            _PegSyntaxMatcher_Item _r14;

            _r14 = _MemoCall(_memo, "Group", _index, Group, null);

            if (_r14 != null) _index = _r14.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void AnyLiteral(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // LITERAL '.'
            _ParseLiteralChar(_memo, ref _index, '.');

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return AnyLiteralToken.Instance; }, _r0), true) );
            }

        }


        public void Literal(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item literal = null;

            // CALLORVAR Symbol
            _PegSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "Symbol", _index, Symbol, null);

            if (_r2 != null) _index = _r2.NextIndex;

            // BIND literal
            literal = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new LiteralToken(((ContainerToken<string>) literal.Results.Single()).Value); }, _r0), true) );
            }

        }


        public void Prefix(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item prefix = null;

            // AND 1
            int _start_i1 = _index;

            // CALLORVAR Symbol
            _PegSyntaxMatcher_Item _r3;

            _r3 = _MemoCall(_memo, "Symbol", _index, Symbol, null);

            if (_r3 != null) _index = _r3.NextIndex;

            // BIND prefix
            prefix = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // LITERAL '~'
            _ParseLiteralChar(_memo, ref _index, '~');

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new PrefixToken(((ContainerToken<string>) prefix.Results.Single()).Value); }, _r0), true) );
            }

        }


        public void Infix(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item infix = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // LITERAL '~'
            _ParseLiteralChar(_memo, ref _index, '~');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // CALLORVAR Symbol
            _PegSyntaxMatcher_Item _r5;

            _r5 = _MemoCall(_memo, "Symbol", _index, Symbol, null);

            if (_r5 != null) _index = _r5.NextIndex;

            // BIND infix
            infix = _memo.Results.Peek();

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // LITERAL '~'
            _ParseLiteralChar(_memo, ref _index, '~');

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new InfixToken(((ContainerToken<string>) infix.Results.Single()).Value); }, _r0), true) );
            }

        }


        public void Suffix(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item suffix = null;

            // AND 1
            int _start_i1 = _index;

            // LITERAL '~'
            _ParseLiteralChar(_memo, ref _index, '~');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR Symbol
            _PegSyntaxMatcher_Item _r4;

            _r4 = _MemoCall(_memo, "Symbol", _index, Symbol, null);

            if (_r4 != null) _index = _r4.NextIndex;

            // BIND suffix
            suffix = _memo.Results.Peek();

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new SuffixToken(((ContainerToken<string>) suffix.Results.Single()).Value); }, _r0), true) );
            }

        }


        public void LiteralSet(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item negation = null;
            _PegSyntaxMatcher_Item members = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // AND 3
            int _start_i3 = _index;

            // AND 4
            int _start_i4 = _index;

            // AND 5
            int _start_i5 = _index;

            // LITERAL '['
            _ParseLiteralChar(_memo, ref _index, '[');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label5; }

            // LITERAL '^'
            _ParseLiteralChar(_memo, ref _index, '^');

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _PegSyntaxMatcher_Item(_index, _memo.InputEnumerable)); }

            // BIND negation
            negation = _memo.Results.Peek();

        label5: // AND
            var _r5_2 = _memo.Results.Pop();
            var _r5_1 = _memo.Results.Pop();

            if (_r5_1 != null && _r5_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _r5_1.Results.Concat(_r5_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i5;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // STAR 10
            int _start_i10 = _index;
            var _res10 = Enumerable.Empty<IToken>();
        label10:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r11;

            _r11 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r11 != null) _index = _r11.NextIndex;

            // STAR 10
            var _r10 = _memo.Results.Pop();
            if (_r10 != null)
            {
                _res10 = _res10.Concat(_r10.Results);
                goto label10;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i10, _index, _memo.InputEnumerable, _res10.Where(_NON_NULL), true));
            }

        label4: // AND
            var _r4_2 = _memo.Results.Pop();
            var _r4_1 = _memo.Results.Pop();

            if (_r4_1 != null && _r4_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _r4_1.Results.Concat(_r4_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i4;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // AND 13
            int _start_i13 = _index;

            // CALLORVAR LiteralSetMember
            _PegSyntaxMatcher_Item _r14;

            _r14 = _MemoCall(_memo, "LiteralSetMember", _index, LiteralSetMember, null);

            if (_r14 != null) _index = _r14.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label13; }

            // STAR 15
            int _start_i15 = _index;
            var _res15 = Enumerable.Empty<IToken>();
        label15:

            // AND 16
            int _start_i16 = _index;

            // PLUS 17
            int _start_i17 = _index;
            var _res17 = Enumerable.Empty<IToken>();
        label17:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r18;

            _r18 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r18 != null) _index = _r18.NextIndex;

            // PLUS 17
            var _r17 = _memo.Results.Pop();
            if (_r17 != null)
            {
                _res17 = _res17.Concat(_r17.Results);
                goto label17;
            }
            else
            {
                if (_index > _start_i17)
                    _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i17, _index, _memo.InputEnumerable, _res17.Where(_NON_NULL), true));
                else
                    _memo.Results.Push(null);
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label16; }

            // CALLORVAR LiteralSetMember
            _PegSyntaxMatcher_Item _r19;

            _r19 = _MemoCall(_memo, "LiteralSetMember", _index, LiteralSetMember, null);

            if (_r19 != null) _index = _r19.NextIndex;

        label16: // AND
            var _r16_2 = _memo.Results.Pop();
            var _r16_1 = _memo.Results.Pop();

            if (_r16_1 != null && _r16_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i16, _index, _memo.InputEnumerable, _r16_1.Results.Concat(_r16_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i16;
            }

            // STAR 15
            var _r15 = _memo.Results.Pop();
            if (_r15 != null)
            {
                _res15 = _res15.Concat(_r15.Results);
                goto label15;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i15, _index, _memo.InputEnumerable, _res15.Where(_NON_NULL), true));
            }

        label13: // AND
            var _r13_2 = _memo.Results.Pop();
            var _r13_1 = _memo.Results.Pop();

            if (_r13_1 != null && _r13_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i13, _index, _memo.InputEnumerable, _r13_1.Results.Concat(_r13_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i13;
            }

            // BIND members
            members = _memo.Results.Peek();

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // STAR 20
            int _start_i20 = _index;
            var _res20 = Enumerable.Empty<IToken>();
        label20:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r21;

            _r21 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r21 != null) _index = _r21.NextIndex;

            // STAR 20
            var _r20 = _memo.Results.Pop();
            if (_r20 != null)
            {
                _res20 = _res20.Concat(_r20.Results);
                goto label20;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i20, _index, _memo.InputEnumerable, _res20.Where(_NON_NULL), true));
            }

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // LITERAL ']'
            _ParseLiteralChar(_memo, ref _index, ']');

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new LiteralSetToken(
            negation.Inputs.Any(),
            members.Results.Cast<ILiteralSetMemberToken>().ToArray()
        ); }, _r0), true) );
            }

        }


        public void LiteralSetMember(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // OR 1
            int _start_i1 = _index;

            // OR 2
            int _start_i2 = _index;

            // CALLORVAR Infix
            _PegSyntaxMatcher_Item _r3;

            _r3 = _MemoCall(_memo, "Infix", _index, Infix, null);

            if (_r3 != null) _index = _r3.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i2; } else goto label2;

            // CALLORVAR Suffix
            _PegSyntaxMatcher_Item _r4;

            _r4 = _MemoCall(_memo, "Suffix", _index, Suffix, null);

            if (_r4 != null) _index = _r4.NextIndex;

        label2: // OR
            int _dummy_i2 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i1; } else goto label1;

            // CALLORVAR Prefix
            _PegSyntaxMatcher_Item _r5;

            _r5 = _MemoCall(_memo, "Prefix", _index, Prefix, null);

            if (_r5 != null) _index = _r5.NextIndex;

        label1: // OR
            int _dummy_i1 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR Literal
            _PegSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "Literal", _index, Literal, null);

            if (_r6 != null) _index = _r6.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void RuleReference(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item ruleName = null;
            _PegSyntaxMatcher_Item arguments = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // LITERAL '$'
            _ParseLiteralChar(_memo, ref _index, '$');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // CALLORVAR m_cSharpSyntaxMatcher.CSharpNamespace
            _PegSyntaxMatcher_Item _r5;

            _r5 = _MemoCall(_memo, "m_cSharpSyntaxMatcher.CSharpNamespace", _index, m_cSharpSyntaxMatcher.CSharpNamespace, null);

            if (_r5 != null) _index = _r5.NextIndex;

            // BIND ruleName
            ruleName = _memo.Results.Peek();

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR RuleArguments
            _PegSyntaxMatcher_Item _r7;

            _r7 = _MemoCall(_memo, "RuleArguments", _index, RuleArguments, null);

            if (_r7 != null) _index = _r7.NextIndex;

            // BIND arguments
            arguments = _memo.Results.Peek();

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new RuleReferenceToken(
            this.m_namespace,
            ((CSharpTypeNameWithNamespaceToken) ruleName.Results.Single()).Value,
            arguments.Results.Cast<IRuleArgumentToken>().ToArray()
        ); }, _r0), true) );
            }

        }


        public void RuleArguments(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // CALLORVAR ImplicitEmptyRuleArguments
            _PegSyntaxMatcher_Item _r1;

            _r1 = _MemoCall(_memo, "ImplicitEmptyRuleArguments", _index, ImplicitEmptyRuleArguments, null);

            if (_r1 != null) _index = _r1.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR DefinedRuleArguments
            _PegSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "DefinedRuleArguments", _index, DefinedRuleArguments, null);

            if (_r2 != null) _index = _r2.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void ImplicitEmptyRuleArguments(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // NOT 0
            int _start_i0 = _index;

            // LITERAL '('
            _ParseLiteralChar(_memo, ref _index, '(');

            // NOT 0
            var _r0 = _memo.Results.Pop();
            _memo.Results.Push( _r0 == null ? new _PegSyntaxMatcher_Item(_start_i0, _memo.InputEnumerable) : null);
            _index = _start_i0;

        }


        public void DefinedRuleArguments(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // AND 0
            int _start_i0 = _index;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // AND 3
            int _start_i3 = _index;

            // LITERAL '('
            _ParseLiteralChar(_memo, ref _index, '(');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // STAR 5
            int _start_i5 = _index;
            var _res5 = Enumerable.Empty<IToken>();
        label5:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // STAR 5
            var _r5 = _memo.Results.Pop();
            if (_r5 != null)
            {
                _res5 = _res5.Concat(_r5.Results);
                goto label5;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _res5.Where(_NON_NULL), true));
            }

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // AND 8
            int _start_i8 = _index;

            // AND 9
            int _start_i9 = _index;

            // CALLORVAR RuleArgument
            _PegSyntaxMatcher_Item _r10;

            _r10 = _MemoCall(_memo, "RuleArgument", _index, RuleArgument, null);

            if (_r10 != null) _index = _r10.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label9; }

            // STAR 11
            int _start_i11 = _index;
            var _res11 = Enumerable.Empty<IToken>();
        label11:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r12;

            _r12 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r12 != null) _index = _r12.NextIndex;

            // STAR 11
            var _r11 = _memo.Results.Pop();
            if (_r11 != null)
            {
                _res11 = _res11.Concat(_r11.Results);
                goto label11;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i11, _index, _memo.InputEnumerable, _res11.Where(_NON_NULL), true));
            }

        label9: // AND
            var _r9_2 = _memo.Results.Pop();
            var _r9_1 = _memo.Results.Pop();

            if (_r9_1 != null && _r9_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i9, _index, _memo.InputEnumerable, _r9_1.Results.Concat(_r9_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i9;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label8; }

            // STAR 13
            int _start_i13 = _index;
            var _res13 = Enumerable.Empty<IToken>();
        label13:

            // AND 14
            int _start_i14 = _index;

            // AND 15
            int _start_i15 = _index;

            // AND 16
            int _start_i16 = _index;

            // LITERAL ','
            _ParseLiteralChar(_memo, ref _index, ',');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label16; }

            // STAR 18
            int _start_i18 = _index;
            var _res18 = Enumerable.Empty<IToken>();
        label18:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r19;

            _r19 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r19 != null) _index = _r19.NextIndex;

            // STAR 18
            var _r18 = _memo.Results.Pop();
            if (_r18 != null)
            {
                _res18 = _res18.Concat(_r18.Results);
                goto label18;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i18, _index, _memo.InputEnumerable, _res18.Where(_NON_NULL), true));
            }

        label16: // AND
            var _r16_2 = _memo.Results.Pop();
            var _r16_1 = _memo.Results.Pop();

            if (_r16_1 != null && _r16_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i16, _index, _memo.InputEnumerable, _r16_1.Results.Concat(_r16_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i16;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label15; }

            // CALLORVAR RuleArgument
            _PegSyntaxMatcher_Item _r20;

            _r20 = _MemoCall(_memo, "RuleArgument", _index, RuleArgument, null);

            if (_r20 != null) _index = _r20.NextIndex;

        label15: // AND
            var _r15_2 = _memo.Results.Pop();
            var _r15_1 = _memo.Results.Pop();

            if (_r15_1 != null && _r15_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i15, _index, _memo.InputEnumerable, _r15_1.Results.Concat(_r15_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i15;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label14; }

            // STAR 21
            int _start_i21 = _index;
            var _res21 = Enumerable.Empty<IToken>();
        label21:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r22;

            _r22 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r22 != null) _index = _r22.NextIndex;

            // STAR 21
            var _r21 = _memo.Results.Pop();
            if (_r21 != null)
            {
                _res21 = _res21.Concat(_r21.Results);
                goto label21;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i21, _index, _memo.InputEnumerable, _res21.Where(_NON_NULL), true));
            }

        label14: // AND
            var _r14_2 = _memo.Results.Pop();
            var _r14_1 = _memo.Results.Pop();

            if (_r14_1 != null && _r14_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i14, _index, _memo.InputEnumerable, _r14_1.Results.Concat(_r14_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i14;
            }

            // STAR 13
            var _r13 = _memo.Results.Pop();
            if (_r13 != null)
            {
                _res13 = _res13.Concat(_r13.Results);
                goto label13;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i13, _index, _memo.InputEnumerable, _res13.Where(_NON_NULL), true));
            }

        label8: // AND
            var _r8_2 = _memo.Results.Pop();
            var _r8_1 = _memo.Results.Pop();

            if (_r8_1 != null && _r8_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i8, _index, _memo.InputEnumerable, _r8_1.Results.Concat(_r8_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i8;
            }

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _PegSyntaxMatcher_Item(_index, _memo.InputEnumerable)); }

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // STAR 23
            int _start_i23 = _index;
            var _res23 = Enumerable.Empty<IToken>();
        label23:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r24;

            _r24 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r24 != null) _index = _r24.NextIndex;

            // STAR 23
            var _r23 = _memo.Results.Pop();
            if (_r23 != null)
            {
                _res23 = _res23.Concat(_r23.Results);
                goto label23;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i23, _index, _memo.InputEnumerable, _res23.Where(_NON_NULL), true));
            }

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label0; }

            // LITERAL ')'
            _ParseLiteralChar(_memo, ref _index, ')');

        label0: // AND
            var _r0_2 = _memo.Results.Pop();
            var _r0_1 = _memo.Results.Pop();

            if (_r0_1 != null && _r0_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i0, _index, _memo.InputEnumerable, _r0_1.Results.Concat(_r0_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i0;
            }

        }


        public void RuleArgument(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // CALLORVAR RuleDefaultArgument
            _PegSyntaxMatcher_Item _r1;

            _r1 = _MemoCall(_memo, "RuleDefaultArgument", _index, RuleDefaultArgument, null);

            if (_r1 != null) _index = _r1.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR RuleObjectPropertyArgument
            _PegSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "RuleObjectPropertyArgument", _index, RuleObjectPropertyArgument, null);

            if (_r2 != null) _index = _r2.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void RuleObjectPropertyArgument(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item memberAccess = null;

            // CALLORVAR m_cSharpSyntaxMatcher.CSharpChainedMemberAccess
            _PegSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "m_cSharpSyntaxMatcher.CSharpChainedMemberAccess", _index, m_cSharpSyntaxMatcher.CSharpChainedMemberAccess, null);

            if (_r2 != null) _index = _r2.NextIndex;

            // BIND memberAccess
            memberAccess = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new RuleChainedMemberAccessArgumentToken(((CSharpChainedMemberAccessToken) memberAccess.Results.Single()).Value); }, _r0), true) );
            }

        }


        public void RuleDefaultArgument(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // LITERAL "default"
            _ParseLiteralString(_memo, ref _index, "default");

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return RuleDefaultArgumentToken.Instance; }, _r0), true) );
            }

        }


        public void Quantifier(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // OR 1
            int _start_i1 = _index;

            // OR 2
            int _start_i2 = _index;

            // CALLORVAR ZeroOrOneQuantifier
            _PegSyntaxMatcher_Item _r3;

            _r3 = _MemoCall(_memo, "ZeroOrOneQuantifier", _index, ZeroOrOneQuantifier, null);

            if (_r3 != null) _index = _r3.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i2; } else goto label2;

            // CALLORVAR ZeroOrMoreQuantifier
            _PegSyntaxMatcher_Item _r4;

            _r4 = _MemoCall(_memo, "ZeroOrMoreQuantifier", _index, ZeroOrMoreQuantifier, null);

            if (_r4 != null) _index = _r4.NextIndex;

        label2: // OR
            int _dummy_i2 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i1; } else goto label1;

            // CALLORVAR OneOrMoreQuantifier
            _PegSyntaxMatcher_Item _r5;

            _r5 = _MemoCall(_memo, "OneOrMoreQuantifier", _index, OneOrMoreQuantifier, null);

            if (_r5 != null) _index = _r5.NextIndex;

        label1: // OR
            int _dummy_i1 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR ExplixitQuantifier
            _PegSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "ExplixitQuantifier", _index, ExplixitQuantifier, null);

            if (_r6 != null) _index = _r6.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void ZeroOrOneQuantifier(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // LITERAL '?'
            _ParseLiteralChar(_memo, ref _index, '?');

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new QuantifierToken(0, 1); }, _r0), true) );
            }

        }


        public void ZeroOrMoreQuantifier(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // LITERAL '*'
            _ParseLiteralChar(_memo, ref _index, '*');

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new QuantifierToken(0, null); }, _r0), true) );
            }

        }


        public void OneOrMoreQuantifier(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // LITERAL '+'
            _ParseLiteralChar(_memo, ref _index, '+');

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new QuantifierToken(1, null); }, _r0), true) );
            }

        }


        public void ExplixitQuantifier(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // AND 0
            int _start_i0 = _index;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // AND 3
            int _start_i3 = _index;

            // LITERAL '{'
            _ParseLiteralChar(_memo, ref _index, '{');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // STAR 5
            int _start_i5 = _index;
            var _res5 = Enumerable.Empty<IToken>();
        label5:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // STAR 5
            var _r5 = _memo.Results.Pop();
            if (_r5 != null)
            {
                _res5 = _res5.Concat(_r5.Results);
                goto label5;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _res5.Where(_NON_NULL), true));
            }

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // OR 7
            int _start_i7 = _index;

            // OR 8
            int _start_i8 = _index;

            // CALLORVAR QuantityRange
            _PegSyntaxMatcher_Item _r9;

            _r9 = _MemoCall(_memo, "QuantityRange", _index, QuantityRange, null);

            if (_r9 != null) _index = _r9.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i8; } else goto label8;

            // CALLORVAR QuantityMin
            _PegSyntaxMatcher_Item _r10;

            _r10 = _MemoCall(_memo, "QuantityMin", _index, QuantityMin, null);

            if (_r10 != null) _index = _r10.NextIndex;

        label8: // OR
            int _dummy_i8 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i7; } else goto label7;

            // CALLORVAR QuantityExact
            _PegSyntaxMatcher_Item _r11;

            _r11 = _MemoCall(_memo, "QuantityExact", _index, QuantityExact, null);

            if (_r11 != null) _index = _r11.NextIndex;

        label7: // OR
            int _dummy_i7 = _index; // no-op for label

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // STAR 12
            int _start_i12 = _index;
            var _res12 = Enumerable.Empty<IToken>();
        label12:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r13;

            _r13 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r13 != null) _index = _r13.NextIndex;

            // STAR 12
            var _r12 = _memo.Results.Pop();
            if (_r12 != null)
            {
                _res12 = _res12.Concat(_r12.Results);
                goto label12;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i12, _index, _memo.InputEnumerable, _res12.Where(_NON_NULL), true));
            }

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label0; }

            // LITERAL '}'
            _ParseLiteralChar(_memo, ref _index, '}');

        label0: // AND
            var _r0_2 = _memo.Results.Pop();
            var _r0_1 = _memo.Results.Pop();

            if (_r0_1 != null && _r0_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i0, _index, _memo.InputEnumerable, _r0_1.Results.Concat(_r0_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i0;
            }

        }


        public void QuantityRange(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item min = null;
            _PegSyntaxMatcher_Item max = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // AND 3
            int _start_i3 = _index;

            // AND 4
            int _start_i4 = _index;

            // CALLORVAR Integer
            _PegSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "Integer", _index, Integer, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // BIND min
            min = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // STAR 7
            int _start_i7 = _index;
            var _res7 = Enumerable.Empty<IToken>();
        label7:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r8;

            _r8 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r8 != null) _index = _r8.NextIndex;

            // STAR 7
            var _r7 = _memo.Results.Pop();
            if (_r7 != null)
            {
                _res7 = _res7.Concat(_r7.Results);
                goto label7;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i7, _index, _memo.InputEnumerable, _res7.Where(_NON_NULL), true));
            }

        label4: // AND
            var _r4_2 = _memo.Results.Pop();
            var _r4_1 = _memo.Results.Pop();

            if (_r4_1 != null && _r4_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _r4_1.Results.Concat(_r4_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i4;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // LITERAL ','
            _ParseLiteralChar(_memo, ref _index, ',');

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // STAR 10
            int _start_i10 = _index;
            var _res10 = Enumerable.Empty<IToken>();
        label10:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r11;

            _r11 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r11 != null) _index = _r11.NextIndex;

            // STAR 10
            var _r10 = _memo.Results.Pop();
            if (_r10 != null)
            {
                _res10 = _res10.Concat(_r10.Results);
                goto label10;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i10, _index, _memo.InputEnumerable, _res10.Where(_NON_NULL), true));
            }

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR Integer
            _PegSyntaxMatcher_Item _r13;

            _r13 = _MemoCall(_memo, "Integer", _index, Integer, null);

            if (_r13 != null) _index = _r13.NextIndex;

            // BIND max
            max = _memo.Results.Peek();

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new QuantifierToken(
            ((ContainerToken<int>) min.Results.Single()).Value,
            ((ContainerToken<int>) max.Results.Single()).Value
        ); }, _r0), true) );
            }

        }


        public void QuantityMin(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item min = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // CALLORVAR Integer
            _PegSyntaxMatcher_Item _r4;

            _r4 = _MemoCall(_memo, "Integer", _index, Integer, null);

            if (_r4 != null) _index = _r4.NextIndex;

            // BIND min
            min = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // STAR 5
            int _start_i5 = _index;
            var _res5 = Enumerable.Empty<IToken>();
        label5:

            // CALLORVAR SPACING
            _PegSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // STAR 5
            var _r5 = _memo.Results.Pop();
            if (_r5 != null)
            {
                _res5 = _res5.Concat(_r5.Results);
                goto label5;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _res5.Where(_NON_NULL), true));
            }

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // LITERAL ','
            _ParseLiteralChar(_memo, ref _index, ',');

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new QuantifierToken(((ContainerToken<int>) min.Results.Single()).Value, null); }, _r0), true) );
            }

        }


        public void QuantityExact(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item quantity = null;

            // CALLORVAR Integer
            _PegSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "Integer", _index, Integer, null);

            if (_r2 != null) _index = _r2.NextIndex;

            // BIND quantity
            quantity = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { int value = ((ContainerToken<int>) quantity.Results.Single()).Value;

        return new QuantifierToken(value, value); }, _r0), true) );
            }

        }


        public void Symbol(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // CALLORVAR CyrillicWord
            _PegSyntaxMatcher_Item _r1;

            _r1 = _MemoCall(_memo, "CyrillicWord", _index, CyrillicWord, null);

            if (_r1 != null) _index = _r1.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR LatinWord
            _PegSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "LatinWord", _index, LatinWord, null);

            if (_r2 != null) _index = _r2.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void CyrillicWord(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item value = null;

            // AND 2
            int _start_i2 = _index;

            // CALLORVAR CyrillicLetter
            _PegSyntaxMatcher_Item _r3;

            _r3 = _MemoCall(_memo, "CyrillicLetter", _index, CyrillicLetter, null);

            if (_r3 != null) _index = _r3.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // STAR 4
            int _start_i4 = _index;
            var _res4 = Enumerable.Empty<IToken>();
        label4:

            // OR 5
            int _start_i5 = _index;

            // OR 6
            int _start_i6 = _index;

            // CALLORVAR CyrillicLetter
            _PegSyntaxMatcher_Item _r7;

            _r7 = _MemoCall(_memo, "CyrillicLetter", _index, CyrillicLetter, null);

            if (_r7 != null) _index = _r7.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i6; } else goto label6;

            // LITERAL '-'
            _ParseLiteralChar(_memo, ref _index, '-');

        label6: // OR
            int _dummy_i6 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i5; } else goto label5;

            // CALLORVAR Integer
            _PegSyntaxMatcher_Item _r9;

            _r9 = _MemoCall(_memo, "Integer", _index, Integer, null);

            if (_r9 != null) _index = _r9.NextIndex;

        label5: // OR
            int _dummy_i5 = _index; // no-op for label

            // STAR 4
            var _r4 = _memo.Results.Pop();
            if (_r4 != null)
            {
                _res4 = _res4.Concat(_r4.Results);
                goto label4;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _res4.Where(_NON_NULL), true));
            }

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // BIND value
            value = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new ContainerToken<string>(value.Inputs.JoinCharsToString()); }, _r0), true) );
            }

        }


        public void CyrillicLetter(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item value = null;

            // INPUT CLASS
            _ParseInputClass(_memo, ref _index, '\u0430', '\u0431', '\u0432', '\u0433', '\u0434', '\u0435', '\u0436', '\u0437', '\u0438', '\u0439', '\u043a', '\u043b', '\u043c', '\u043d', '\u043e', '\u043f', '\u0440', '\u0441', '\u0442', '\u0443', '\u0444', '\u0445', '\u0446', '\u0447', '\u0448', '\u0449', '\u044a', '\u044b', '\u044c', '\u044d', '\u044e', '\u044f', '\u0410', '\u0411', '\u0412', '\u0413', '\u0414', '\u0415', '\u0416', '\u0417', '\u0418', '\u0419', '\u041a', '\u041b', '\u041c', '\u041d', '\u041e', '\u041f', '\u0420', '\u0421', '\u0422', '\u0423', '\u0424', '\u0425', '\u0426', '\u0427', '\u0428', '\u0429', '\u042a', '\u042b', '\u042c', '\u042d', '\u042e', '\u042f', 'ё', 'Ё');

            // BIND value
            value = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new ContainerToken<char>(value.Inputs.Single()); }, _r0), true) );
            }

        }


        public void LatinWord(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item value = null;

            // AND 2
            int _start_i2 = _index;

            // CALLORVAR LatinLetter
            _PegSyntaxMatcher_Item _r3;

            _r3 = _MemoCall(_memo, "LatinLetter", _index, LatinLetter, null);

            if (_r3 != null) _index = _r3.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // STAR 4
            int _start_i4 = _index;
            var _res4 = Enumerable.Empty<IToken>();
        label4:

            // OR 5
            int _start_i5 = _index;

            // OR 6
            int _start_i6 = _index;

            // CALLORVAR LatinLetter
            _PegSyntaxMatcher_Item _r7;

            _r7 = _MemoCall(_memo, "LatinLetter", _index, LatinLetter, null);

            if (_r7 != null) _index = _r7.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i6; } else goto label6;

            // LITERAL '-'
            _ParseLiteralChar(_memo, ref _index, '-');

        label6: // OR
            int _dummy_i6 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i5; } else goto label5;

            // CALLORVAR Integer
            _PegSyntaxMatcher_Item _r9;

            _r9 = _MemoCall(_memo, "Integer", _index, Integer, null);

            if (_r9 != null) _index = _r9.NextIndex;

        label5: // OR
            int _dummy_i5 = _index; // no-op for label

            // STAR 4
            var _r4 = _memo.Results.Pop();
            if (_r4 != null)
            {
                _res4 = _res4.Concat(_r4.Results);
                goto label4;
            }
            else
            {
                _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _res4.Where(_NON_NULL), true));
            }

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // BIND value
            value = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new ContainerToken<string>(value.Inputs.JoinCharsToString()); }, _r0), true) );
            }

        }


        public void LatinLetter(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item value = null;

            // INPUT CLASS
            _ParseInputClass(_memo, ref _index, '\u0061', '\u0062', '\u0063', '\u0064', '\u0065', '\u0066', '\u0067', '\u0068', '\u0069', '\u006a', '\u006b', '\u006c', '\u006d', '\u006e', '\u006f', '\u0070', '\u0071', '\u0072', '\u0073', '\u0074', '\u0075', '\u0076', '\u0077', '\u0078', '\u0079', '\u007a', '\u0041', '\u0042', '\u0043', '\u0044', '\u0045', '\u0046', '\u0047', '\u0048', '\u0049', '\u004a', '\u004b', '\u004c', '\u004d', '\u004e', '\u004f', '\u0050', '\u0051', '\u0052', '\u0053', '\u0054', '\u0055', '\u0056', '\u0057', '\u0058', '\u0059', '\u005a');

            // BIND value
            value = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new ContainerToken<char>(value.Inputs.Single()); }, _r0), true) );
            }

        }


        public void Integer(_PegSyntaxMatcher_Memo _memo, int _index, _PegSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _PegSyntaxMatcher_Item value = null;

            // PLUS 2
            int _start_i2 = _index;
            var _res2 = Enumerable.Empty<IToken>();
        label2:

            // INPUT CLASS
            _ParseInputClass(_memo, ref _index, '\u0030', '\u0031', '\u0032', '\u0033', '\u0034', '\u0035', '\u0036', '\u0037', '\u0038', '\u0039');

            // PLUS 2
            var _r2 = _memo.Results.Pop();
            if (_r2 != null)
            {
                _res2 = _res2.Concat(_r2.Results);
                goto label2;
            }
            else
            {
                if (_index > _start_i2)
                    _memo.Results.Push(new _PegSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _res2.Where(_NON_NULL), true));
                else
                    _memo.Results.Push(null);
            }

            // BIND value
            value = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _PegSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new ContainerToken<int>(Int32.Parse(value.Inputs.JoinCharsToString())); }, _r0), true) );
            }

        }


    } // class PegSyntaxMatcher

} // namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Grammar

