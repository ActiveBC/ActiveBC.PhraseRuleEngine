//
// IronMeta RuleSetSyntaxMatcher Parser; Generated 2022-05-12 09:45:31Z UTC
//

using System;
using System.Collections.Generic;
using System.Linq;

using IronMeta.Matcher;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens.Internal;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;

#pragma warning disable 0219
#pragma warning disable 1591

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Grammar
{

    using _RuleSetSyntaxMatcher_Inputs = IEnumerable<char>;
    using _RuleSetSyntaxMatcher_Results = IEnumerable<IToken>;
    using _RuleSetSyntaxMatcher_Item = IronMeta.Matcher.MatchItem<char, IToken>;
    using _RuleSetSyntaxMatcher_Args = IEnumerable<IronMeta.Matcher.MatchItem<char, IToken>>;
    using _RuleSetSyntaxMatcher_Memo = IronMeta.Matcher.MatchState<char, IToken>;
    using _RuleSetSyntaxMatcher_Rule = System.Action<IronMeta.Matcher.MatchState<char, IToken>, int, IEnumerable<IronMeta.Matcher.MatchItem<char, IToken>>>;
    using _RuleSetSyntaxMatcher_Base = IronMeta.Matcher.Matcher<char, IToken>;

    public partial class RuleSetSyntaxMatcher : Matcher<char, IToken>
    {
        public RuleSetSyntaxMatcher()
            : base()
        {
            _setTerminals();
        }

        public RuleSetSyntaxMatcher(bool handle_left_recursion)
            : base(handle_left_recursion)
        {
            _setTerminals();
        }

        void _setTerminals()
        {
            this.Terminals = new HashSet<string>()
            {
                "COMMENT",
                "EOF",
                "EOL",
                "ImplicitEmptyRuleParameters",
                "RawPattern",
                "RuleName",
                "SPACING",
                "WHITESPACE",
            };
        }


        public void SPACING(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // OR 1
            int _start_i1 = _index;

            // CALLORVAR EOL
            _RuleSetSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "EOL", _index, EOL, null);

            if (_r2 != null) _index = _r2.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i1; } else goto label1;

            // CALLORVAR WHITESPACE
            _RuleSetSyntaxMatcher_Item _r3;

            _r3 = _MemoCall(_memo, "WHITESPACE", _index, WHITESPACE, null);

            if (_r3 != null) _index = _r3.NextIndex;

        label1: // OR
            int _dummy_i1 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR COMMENT
            _RuleSetSyntaxMatcher_Item _r4;

            _r4 = _MemoCall(_memo, "COMMENT", _index, COMMENT, null);

            if (_r4 != null) _index = _r4.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void WHITESPACE(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // INPUT CLASS
            _ParseInputClass(_memo, ref _index, ' ', '\t');

        }


        public void EOL(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // LITERAL "\r\n"
            _ParseLiteralString(_memo, ref _index, "\r\n");

        }


        public void EOF(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // NOT 0
            int _start_i0 = _index;

            // ANY
            _ParseAny(_memo, ref _index);

            // NOT 0
            var _r0 = _memo.Results.Pop();
            _memo.Results.Push( _r0 == null ? new _RuleSetSyntaxMatcher_Item(_start_i0, _memo.InputEnumerable) : null);
            _index = _start_i0;

        }


        public void COMMENT(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // CALLORVAR m_cSharpSyntaxMatcher.CSharpComment
            _RuleSetSyntaxMatcher_Item _r0;

            _r0 = _MemoCall(_memo, "m_cSharpSyntaxMatcher.CSharpComment", _index, m_cSharpSyntaxMatcher.CSharpComment, null);

            if (_r0 != null) _index = _r0.NextIndex;

        }


        public void RuleSet(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _RuleSetSyntaxMatcher_Item usings = null;
            _RuleSetSyntaxMatcher_Item rules = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // STAR 4
            int _start_i4 = _index;
            var _res4 = Enumerable.Empty<IToken>();
        label4:

            // AND 5
            int _start_i5 = _index;

            // AND 6
            int _start_i6 = _index;

            // STAR 7
            int _start_i7 = _index;
            var _res7 = Enumerable.Empty<IToken>();
        label7:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r8;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i7, _index, _memo.InputEnumerable, _res7.Where(_NON_NULL), true));
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label6; }

            // CALLORVAR Using
            _RuleSetSyntaxMatcher_Item _r9;

            _r9 = _MemoCall(_memo, "Using", _index, Using, null);

            if (_r9 != null) _index = _r9.NextIndex;

        label6: // AND
            var _r6_2 = _memo.Results.Pop();
            var _r6_1 = _memo.Results.Pop();

            if (_r6_1 != null && _r6_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i6, _index, _memo.InputEnumerable, _r6_1.Results.Concat(_r6_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i6;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label5; }

            // STAR 10
            int _start_i10 = _index;
            var _res10 = Enumerable.Empty<IToken>();
        label10:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r11;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i10, _index, _memo.InputEnumerable, _res10.Where(_NON_NULL), true));
            }

        label5: // AND
            var _r5_2 = _memo.Results.Pop();
            var _r5_1 = _memo.Results.Pop();

            if (_r5_1 != null && _r5_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _r5_1.Results.Concat(_r5_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i5;
            }

            // STAR 4
            var _r4 = _memo.Results.Pop();
            if (_r4 != null)
            {
                _res4 = _res4.Concat(_r4.Results);
                goto label4;
            }
            else
            {
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _res4.Where(_NON_NULL), true));
            }

            // BIND usings
            usings = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // STAR 13
            int _start_i13 = _index;
            var _res13 = Enumerable.Empty<IToken>();
        label13:

            // AND 14
            int _start_i14 = _index;

            // AND 15
            int _start_i15 = _index;

            // STAR 16
            int _start_i16 = _index;
            var _res16 = Enumerable.Empty<IToken>();
        label16:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r17;

            _r17 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r17 != null) _index = _r17.NextIndex;

            // STAR 16
            var _r16 = _memo.Results.Pop();
            if (_r16 != null)
            {
                _res16 = _res16.Concat(_r16.Results);
                goto label16;
            }
            else
            {
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i16, _index, _memo.InputEnumerable, _res16.Where(_NON_NULL), true));
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label15; }

            // CALLORVAR Rule
            _RuleSetSyntaxMatcher_Item _r18;

            _r18 = _MemoCall(_memo, "Rule", _index, Rule, null);

            if (_r18 != null) _index = _r18.NextIndex;

        label15: // AND
            var _r15_2 = _memo.Results.Pop();
            var _r15_1 = _memo.Results.Pop();

            if (_r15_1 != null && _r15_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i15, _index, _memo.InputEnumerable, _r15_1.Results.Concat(_r15_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i15;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label14; }

            // STAR 19
            int _start_i19 = _index;
            var _res19 = Enumerable.Empty<IToken>();
        label19:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r20;

            _r20 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r20 != null) _index = _r20.NextIndex;

            // STAR 19
            var _r19 = _memo.Results.Pop();
            if (_r19 != null)
            {
                _res19 = _res19.Concat(_r19.Results);
                goto label19;
            }
            else
            {
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i19, _index, _memo.InputEnumerable, _res19.Where(_NON_NULL), true));
            }

        label14: // AND
            var _r14_2 = _memo.Results.Pop();
            var _r14_1 = _memo.Results.Pop();

            if (_r14_1 != null && _r14_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i14, _index, _memo.InputEnumerable, _r14_1.Results.Concat(_r14_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i13, _index, _memo.InputEnumerable, _res13.Where(_NON_NULL), true));
            }

            // BIND rules
            rules = _memo.Results.Peek();

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR EOF
            _RuleSetSyntaxMatcher_Item _r21;

            _r21 = _MemoCall(_memo, "EOF", _index, EOF, null);

            if (_r21 != null) _index = _r21.NextIndex;

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new RuleSetToken(
            usings.Results.Cast<UsingToken>().ToArray(),
            rules.Results.Cast<RuleToken>().ToArray()
        ); }, _r0), true) );
            }

        }


        public void Using(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _RuleSetSyntaxMatcher_Item value = null;

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

            // LITERAL "using"
            _ParseLiteralString(_memo, ref _index, "using");

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label5; }

            // PLUS 7
            int _start_i7 = _index;
            var _res7 = Enumerable.Empty<IToken>();
        label7:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r8;

            _r8 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r8 != null) _index = _r8.NextIndex;

            // PLUS 7
            var _r7 = _memo.Results.Pop();
            if (_r7 != null)
            {
                _res7 = _res7.Concat(_r7.Results);
                goto label7;
            }
            else
            {
                if (_index > _start_i7)
                    _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i7, _index, _memo.InputEnumerable, _res7.Where(_NON_NULL), true));
                else
                    _memo.Results.Push(null);
            }

        label5: // AND
            var _r5_2 = _memo.Results.Pop();
            var _r5_1 = _memo.Results.Pop();

            if (_r5_1 != null && _r5_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _r5_1.Results.Concat(_r5_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i5;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // CALLORVAR m_cSharpSyntaxMatcher.CSharpNamespace
            _RuleSetSyntaxMatcher_Item _r10;

            _r10 = _MemoCall(_memo, "m_cSharpSyntaxMatcher.CSharpNamespace", _index, m_cSharpSyntaxMatcher.CSharpNamespace, null);

            if (_r10 != null) _index = _r10.NextIndex;

            // BIND value
            value = _memo.Results.Peek();

        label4: // AND
            var _r4_2 = _memo.Results.Pop();
            var _r4_1 = _memo.Results.Pop();

            if (_r4_1 != null && _r4_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _r4_1.Results.Concat(_r4_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i4;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // STAR 11
            int _start_i11 = _index;
            var _res11 = Enumerable.Empty<IToken>();
        label11:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r12;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i11, _index, _memo.InputEnumerable, _res11.Where(_NON_NULL), true));
            }

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // LITERAL ';'
            _ParseLiteralChar(_memo, ref _index, ';');

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // PLUS 14
            int _start_i14 = _index;
            var _res14 = Enumerable.Empty<IToken>();
        label14:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r15;

            _r15 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r15 != null) _index = _r15.NextIndex;

            // PLUS 14
            var _r14 = _memo.Results.Pop();
            if (_r14 != null)
            {
                _res14 = _res14.Concat(_r14.Results);
                goto label14;
            }
            else
            {
                if (_index > _start_i14)
                    _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i14, _index, _memo.InputEnumerable, _res14.Where(_NON_NULL), true));
                else
                    _memo.Results.Push(null);
            }

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new UsingToken(((CSharpTypeNameWithNamespaceToken) value.Results.Single()).Value); }, _r0), true) );
            }

        }


        public void Rule(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _RuleSetSyntaxMatcher_Item type = null;
            _RuleSetSyntaxMatcher_Item name = null;
            _RuleSetSyntaxMatcher_Item parameters = null;
            _RuleSetSyntaxMatcher_Item pattern = null;
            _RuleSetSyntaxMatcher_Item projection = null;

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

            // AND 6
            int _start_i6 = _index;

            // AND 7
            int _start_i7 = _index;

            // AND 8
            int _start_i8 = _index;

            // AND 9
            int _start_i9 = _index;

            // CALLORVAR m_cSharpSyntaxMatcher.CSharpType
            _RuleSetSyntaxMatcher_Item _r11;

            _r11 = _MemoCall(_memo, "m_cSharpSyntaxMatcher.CSharpType", _index, m_cSharpSyntaxMatcher.CSharpType, null);

            if (_r11 != null) _index = _r11.NextIndex;

            // BIND type
            type = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label9; }

            // PLUS 12
            int _start_i12 = _index;
            var _res12 = Enumerable.Empty<IToken>();
        label12:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r13;

            _r13 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r13 != null) _index = _r13.NextIndex;

            // PLUS 12
            var _r12 = _memo.Results.Pop();
            if (_r12 != null)
            {
                _res12 = _res12.Concat(_r12.Results);
                goto label12;
            }
            else
            {
                if (_index > _start_i12)
                    _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i12, _index, _memo.InputEnumerable, _res12.Where(_NON_NULL), true));
                else
                    _memo.Results.Push(null);
            }

        label9: // AND
            var _r9_2 = _memo.Results.Pop();
            var _r9_1 = _memo.Results.Pop();

            if (_r9_1 != null && _r9_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i9, _index, _memo.InputEnumerable, _r9_1.Results.Concat(_r9_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i9;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label8; }

            // CALLORVAR RuleName
            _RuleSetSyntaxMatcher_Item _r15;

            _r15 = _MemoCall(_memo, "RuleName", _index, RuleName, null);

            if (_r15 != null) _index = _r15.NextIndex;

            // BIND name
            name = _memo.Results.Peek();

        label8: // AND
            var _r8_2 = _memo.Results.Pop();
            var _r8_1 = _memo.Results.Pop();

            if (_r8_1 != null && _r8_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i8, _index, _memo.InputEnumerable, _r8_1.Results.Concat(_r8_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i8;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label7; }

            // CALLORVAR RuleParameters
            _RuleSetSyntaxMatcher_Item _r17;

            _r17 = _MemoCall(_memo, "RuleParameters", _index, RuleParameters, null);

            if (_r17 != null) _index = _r17.NextIndex;

            // BIND parameters
            parameters = _memo.Results.Peek();

        label7: // AND
            var _r7_2 = _memo.Results.Pop();
            var _r7_1 = _memo.Results.Pop();

            if (_r7_1 != null && _r7_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i7, _index, _memo.InputEnumerable, _r7_1.Results.Concat(_r7_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i7;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label6; }

            // STAR 18
            int _start_i18 = _index;
            var _res18 = Enumerable.Empty<IToken>();
        label18:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r19;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i18, _index, _memo.InputEnumerable, _res18.Where(_NON_NULL), true));
            }

        label6: // AND
            var _r6_2 = _memo.Results.Pop();
            var _r6_1 = _memo.Results.Pop();

            if (_r6_1 != null && _r6_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i6, _index, _memo.InputEnumerable, _r6_1.Results.Concat(_r6_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i6;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label5; }

            // LITERAL '='
            _ParseLiteralChar(_memo, ref _index, '=');

        label5: // AND
            var _r5_2 = _memo.Results.Pop();
            var _r5_1 = _memo.Results.Pop();

            if (_r5_1 != null && _r5_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _r5_1.Results.Concat(_r5_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i5;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // STAR 21
            int _start_i21 = _index;
            var _res21 = Enumerable.Empty<IToken>();
        label21:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r22;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i21, _index, _memo.InputEnumerable, _res21.Where(_NON_NULL), true));
            }

        label4: // AND
            var _r4_2 = _memo.Results.Pop();
            var _r4_1 = _memo.Results.Pop();

            if (_r4_1 != null && _r4_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _r4_1.Results.Concat(_r4_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i4;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // CALLORVAR Pattern
            _RuleSetSyntaxMatcher_Item _r24;

            _r24 = _MemoCall(_memo, "Pattern", _index, Pattern, null);

            if (_r24 != null) _index = _r24.NextIndex;

            // BIND pattern
            pattern = _memo.Results.Peek();

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // STAR 25
            int _start_i25 = _index;
            var _res25 = Enumerable.Empty<IToken>();
        label25:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r26;

            _r26 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r26 != null) _index = _r26.NextIndex;

            // STAR 25
            var _r25 = _memo.Results.Pop();
            if (_r25 != null)
            {
                _res25 = _res25.Concat(_r25.Results);
                goto label25;
            }
            else
            {
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i25, _index, _memo.InputEnumerable, _res25.Where(_NON_NULL), true));
            }

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR Projection
            _RuleSetSyntaxMatcher_Item _r28;

            _r28 = _MemoCall(_memo, "Projection", _index, Projection, null);

            if (_r28 != null) _index = _r28.NextIndex;

            // BIND projection
            projection = _memo.Results.Peek();

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { (string Key, string RawPattern) patternData = ((ContainerToken<(string Key, string RawPattern)>) pattern.Results.Single()).Value;

        return new RuleToken(
            this.m_namespace,
            (ICSharpTypeToken) type.Results.Single(),
            ((CSharpIdentifierToken) name.Results.Single()).Value,
            parameters.Results.Cast<CSharpParameterToken>().ToArray(),
            patternData.Key,
            this.m_patternParsers[patternData.Key].Tokenize(patternData.RawPattern, this.m_namespace, this.m_caseSensitive),
            (IProjectionToken) projection.Results.Single()
        ); }, _r0), true) );
            }

        }


        public void RuleName(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // CALLORVAR m_cSharpSyntaxMatcher.CSharpIdentifier
            _RuleSetSyntaxMatcher_Item _r0;

            _r0 = _MemoCall(_memo, "m_cSharpSyntaxMatcher.CSharpIdentifier", _index, m_cSharpSyntaxMatcher.CSharpIdentifier, null);

            if (_r0 != null) _index = _r0.NextIndex;

        }


        public void RuleParameters(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // CALLORVAR ImplicitEmptyRuleParameters
            _RuleSetSyntaxMatcher_Item _r1;

            _r1 = _MemoCall(_memo, "ImplicitEmptyRuleParameters", _index, ImplicitEmptyRuleParameters, null);

            if (_r1 != null) _index = _r1.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR DefinedRuleParameters
            _RuleSetSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "DefinedRuleParameters", _index, DefinedRuleParameters, null);

            if (_r2 != null) _index = _r2.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void ImplicitEmptyRuleParameters(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // NOT 0
            int _start_i0 = _index;

            // LITERAL '('
            _ParseLiteralChar(_memo, ref _index, '(');

            // NOT 0
            var _r0 = _memo.Results.Pop();
            _memo.Results.Push( _r0 == null ? new _RuleSetSyntaxMatcher_Item(_start_i0, _memo.InputEnumerable) : null);
            _index = _start_i0;

        }


        public void DefinedRuleParameters(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
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
            _RuleSetSyntaxMatcher_Item _r6;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _res5.Where(_NON_NULL), true));
            }

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
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

            // CALLORVAR RuleParameter
            _RuleSetSyntaxMatcher_Item _r10;

            _r10 = _MemoCall(_memo, "RuleParameter", _index, RuleParameter, null);

            if (_r10 != null) _index = _r10.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label9; }

            // STAR 11
            int _start_i11 = _index;
            var _res11 = Enumerable.Empty<IToken>();
        label11:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r12;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i11, _index, _memo.InputEnumerable, _res11.Where(_NON_NULL), true));
            }

        label9: // AND
            var _r9_2 = _memo.Results.Pop();
            var _r9_1 = _memo.Results.Pop();

            if (_r9_1 != null && _r9_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i9, _index, _memo.InputEnumerable, _r9_1.Results.Concat(_r9_2.Results).Where(_NON_NULL), true) );
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
            _RuleSetSyntaxMatcher_Item _r19;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i18, _index, _memo.InputEnumerable, _res18.Where(_NON_NULL), true));
            }

        label16: // AND
            var _r16_2 = _memo.Results.Pop();
            var _r16_1 = _memo.Results.Pop();

            if (_r16_1 != null && _r16_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i16, _index, _memo.InputEnumerable, _r16_1.Results.Concat(_r16_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i16;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label15; }

            // CALLORVAR RuleParameter
            _RuleSetSyntaxMatcher_Item _r20;

            _r20 = _MemoCall(_memo, "RuleParameter", _index, RuleParameter, null);

            if (_r20 != null) _index = _r20.NextIndex;

        label15: // AND
            var _r15_2 = _memo.Results.Pop();
            var _r15_1 = _memo.Results.Pop();

            if (_r15_1 != null && _r15_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i15, _index, _memo.InputEnumerable, _r15_1.Results.Concat(_r15_2.Results).Where(_NON_NULL), true) );
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
            _RuleSetSyntaxMatcher_Item _r22;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i21, _index, _memo.InputEnumerable, _res21.Where(_NON_NULL), true));
            }

        label14: // AND
            var _r14_2 = _memo.Results.Pop();
            var _r14_1 = _memo.Results.Pop();

            if (_r14_1 != null && _r14_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i14, _index, _memo.InputEnumerable, _r14_1.Results.Concat(_r14_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i13, _index, _memo.InputEnumerable, _res13.Where(_NON_NULL), true));
            }

        label8: // AND
            var _r8_2 = _memo.Results.Pop();
            var _r8_1 = _memo.Results.Pop();

            if (_r8_1 != null && _r8_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i8, _index, _memo.InputEnumerable, _r8_1.Results.Concat(_r8_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i8;
            }

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_index, _memo.InputEnumerable)); }

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
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
            _RuleSetSyntaxMatcher_Item _r24;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i23, _index, _memo.InputEnumerable, _res23.Where(_NON_NULL), true));
            }

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i0, _index, _memo.InputEnumerable, _r0_1.Results.Concat(_r0_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i0;
            }

        }


        public void RuleParameter(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _RuleSetSyntaxMatcher_Item type = null;
            _RuleSetSyntaxMatcher_Item name = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // CALLORVAR m_cSharpSyntaxMatcher.CSharpType
            _RuleSetSyntaxMatcher_Item _r4;

            _r4 = _MemoCall(_memo, "m_cSharpSyntaxMatcher.CSharpType", _index, m_cSharpSyntaxMatcher.CSharpType, null);

            if (_r4 != null) _index = _r4.NextIndex;

            // BIND type
            type = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // PLUS 5
            int _start_i5 = _index;
            var _res5 = Enumerable.Empty<IToken>();
        label5:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // PLUS 5
            var _r5 = _memo.Results.Pop();
            if (_r5 != null)
            {
                _res5 = _res5.Concat(_r5.Results);
                goto label5;
            }
            else
            {
                if (_index > _start_i5)
                    _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _res5.Where(_NON_NULL), true));
                else
                    _memo.Results.Push(null);
            }

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR m_cSharpSyntaxMatcher.CSharpIdentifier
            _RuleSetSyntaxMatcher_Item _r8;

            _r8 = _MemoCall(_memo, "m_cSharpSyntaxMatcher.CSharpIdentifier", _index, m_cSharpSyntaxMatcher.CSharpIdentifier, null);

            if (_r8 != null) _index = _r8.NextIndex;

            // BIND name
            name = _memo.Results.Peek();

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new CSharpParameterToken(
            (ICSharpTypeToken) type.Results.Single(),
            ((CSharpIdentifierToken) name.Results.Single()).Value
        ); }, _r0), true) );
            }

        }


        public void Pattern(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _RuleSetSyntaxMatcher_Item patternKey = null;
            _RuleSetSyntaxMatcher_Item rawPattern = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // AND 3
            int _start_i3 = _index;

            // AND 4
            int _start_i4 = _index;

            // CALLORVAR m_cSharpSyntaxMatcher.CSharpIdentifier
            _RuleSetSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "m_cSharpSyntaxMatcher.CSharpIdentifier", _index, m_cSharpSyntaxMatcher.CSharpIdentifier, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // BIND patternKey
            patternKey = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // STAR 7
            int _start_i7 = _index;
            var _res7 = Enumerable.Empty<IToken>();
        label7:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r8;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i7, _index, _memo.InputEnumerable, _res7.Where(_NON_NULL), true));
            }

        label4: // AND
            var _r4_2 = _memo.Results.Pop();
            var _r4_1 = _memo.Results.Pop();

            if (_r4_1 != null && _r4_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _r4_1.Results.Concat(_r4_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i4;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // LITERAL '#'
            _ParseLiteralChar(_memo, ref _index, '#');

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // CALLORVAR RawPattern
            _RuleSetSyntaxMatcher_Item _r11;

            _r11 = _MemoCall(_memo, "RawPattern", _index, RawPattern, null);

            if (_r11 != null) _index = _r11.NextIndex;

            // BIND rawPattern
            rawPattern = _memo.Results.Peek();

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // LITERAL '#'
            _ParseLiteralChar(_memo, ref _index, '#');

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new ContainerToken<(string Key, string RawPattern)>((patternKey.Inputs.JoinCharsToString(), rawPattern.Inputs.JoinCharsToString())); }, _r0), true) );
            }

        }


        public void RawPattern(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // STAR 0
            int _start_i0 = _index;
            var _res0 = Enumerable.Empty<IToken>();
        label0:

            // AND 1
            int _start_i1 = _index;

            // NOT 2
            int _start_i2 = _index;

            // LITERAL '#'
            _ParseLiteralChar(_memo, ref _index, '#');

            // NOT 2
            var _r2 = _memo.Results.Pop();
            _memo.Results.Push( _r2 == null ? new _RuleSetSyntaxMatcher_Item(_start_i2, _memo.InputEnumerable) : null);
            _index = _start_i2;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // ANY
            _ParseAny(_memo, ref _index);

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // STAR 0
            var _r0 = _memo.Results.Pop();
            if (_r0 != null)
            {
                _res0 = _res0.Concat(_r0.Results);
                goto label0;
            }
            else
            {
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i0, _index, _memo.InputEnumerable, _res0.Where(_NON_NULL), true));
            }

        }


        public void Projection(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // CALLORVAR VoidProjection
            _RuleSetSyntaxMatcher_Item _r1;

            _r1 = _MemoCall(_memo, "VoidProjection", _index, VoidProjection, null);

            if (_r1 != null) _index = _r1.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR BodyBasedProjection
            _RuleSetSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "BodyBasedProjection", _index, BodyBasedProjection, null);

            if (_r2 != null) _index = _r2.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void VoidProjection(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // STAR 3
            int _start_i3 = _index;
            var _res3 = Enumerable.Empty<IToken>();
        label3:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r4;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _res3.Where(_NON_NULL), true));
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // CALLORVAR m_cSharpSyntaxMatcher.CSharpEmptyMethodBody
            _RuleSetSyntaxMatcher_Item _r5;

            _r5 = _MemoCall(_memo, "m_cSharpSyntaxMatcher.CSharpEmptyMethodBody", _index, m_cSharpSyntaxMatcher.CSharpEmptyMethodBody, null);

            if (_r5 != null) _index = _r5.NextIndex;

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // STAR 6
            int _start_i6 = _index;
            var _res6 = Enumerable.Empty<IToken>();
        label6:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r7;

            _r7 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r7 != null) _index = _r7.NextIndex;

            // STAR 6
            var _r6 = _memo.Results.Pop();
            if (_r6 != null)
            {
                _res6 = _res6.Concat(_r6.Results);
                goto label6;
            }
            else
            {
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i6, _index, _memo.InputEnumerable, _res6.Where(_NON_NULL), true));
            }

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return VoidProjectionToken.Instance; }, _r0), true) );
            }

        }


        public void BodyBasedProjection(_RuleSetSyntaxMatcher_Memo _memo, int _index, _RuleSetSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _RuleSetSyntaxMatcher_Item body = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // STAR 3
            int _start_i3 = _index;
            var _res3 = Enumerable.Empty<IToken>();
        label3:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r4;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _res3.Where(_NON_NULL), true));
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // CALLORVAR m_cSharpSyntaxMatcher.CSharpMethodBody
            _RuleSetSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "m_cSharpSyntaxMatcher.CSharpMethodBody", _index, m_cSharpSyntaxMatcher.CSharpMethodBody, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // BIND body
            body = _memo.Results.Peek();

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // STAR 7
            int _start_i7 = _index;
            var _res7 = Enumerable.Empty<IToken>();
        label7:

            // CALLORVAR SPACING
            _RuleSetSyntaxMatcher_Item _r8;

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
                _memo.Results.Push(new _RuleSetSyntaxMatcher_Item(_start_i7, _index, _memo.InputEnumerable, _res7.Where(_NON_NULL), true));
            }

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push( new _RuleSetSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new BodyBasedProjectionToken(body.Inputs.JoinCharsToString()); }, _r0), true) );
            }

        }


    } // class RuleSetSyntaxMatcher

} // namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Grammar

