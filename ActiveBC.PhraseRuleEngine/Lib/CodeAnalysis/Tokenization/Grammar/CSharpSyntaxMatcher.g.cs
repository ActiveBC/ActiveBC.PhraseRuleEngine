//
// IronMeta CSharpSyntaxMatcher Parser; Generated 2022-05-12 09:45:31Z UTC
//

using System;
using System.Collections.Generic;
using System.Linq;

using IronMeta.Matcher;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens.Internal;

#pragma warning disable 0219
#pragma warning disable 1591

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Grammar
{

    using _CSharpSyntaxMatcher_Inputs = IEnumerable<char>;
    using _CSharpSyntaxMatcher_Results = IEnumerable<IToken>;
    using _CSharpSyntaxMatcher_Item = IronMeta.Matcher.MatchItem<char, IToken>;
    using _CSharpSyntaxMatcher_Args = IEnumerable<IronMeta.Matcher.MatchItem<char, IToken>>;
    using _CSharpSyntaxMatcher_Memo = IronMeta.Matcher.MatchState<char, IToken>;
    using _CSharpSyntaxMatcher_Rule = System.Action<IronMeta.Matcher.MatchState<char, IToken>, int, IEnumerable<IronMeta.Matcher.MatchItem<char, IToken>>>;
    using _CSharpSyntaxMatcher_Base = IronMeta.Matcher.Matcher<char, IToken>;

    public partial class CSharpSyntaxMatcher : Matcher<char, IToken>
    {
        public CSharpSyntaxMatcher()
            : base()
        {
            _setTerminals();
        }

        public CSharpSyntaxMatcher(bool handle_left_recursion)
            : base(handle_left_recursion)
        {
            _setTerminals();
        }

        void _setTerminals()
        {
            this.Terminals = new HashSet<string>()
            {
                "CSharpChainedIdentifiers",
                "CSharpChainedMemberAccess",
                "CSharpComment",
                "CSharpEmptyCodeBlock",
                "CSharpEmptyMethodBody",
                "CSharpIdentifier",
                "CSharpNamespace",
                "CSharpTypeNameWithNamespace",
                "EOL",
                "LatinLetterOrUnderscore",
                "SPACING",
                "WHITESPACE",
            };
        }


        public void SPACING(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // OR 1
            int _start_i1 = _index;

            // CALLORVAR EOL
            _CSharpSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "EOL", _index, EOL, null);

            if (_r2 != null) _index = _r2.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i1; } else goto label1;

            // CALLORVAR WHITESPACE
            _CSharpSyntaxMatcher_Item _r3;

            _r3 = _MemoCall(_memo, "WHITESPACE", _index, WHITESPACE, null);

            if (_r3 != null) _index = _r3.NextIndex;

        label1: // OR
            int _dummy_i1 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR CSharpComment
            _CSharpSyntaxMatcher_Item _r4;

            _r4 = _MemoCall(_memo, "CSharpComment", _index, CSharpComment, null);

            if (_r4 != null) _index = _r4.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void WHITESPACE(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // INPUT CLASS
            _ParseInputClass(_memo, ref _index, ' ', '\t');

        }


        public void EOL(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // LITERAL "\r\n"
            _ParseLiteralString(_memo, ref _index, "\r\n");

        }


        public void CSharpIdentifier(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _CSharpSyntaxMatcher_Item value = null;

            // AND 2
            int _start_i2 = _index;

            // CALLORVAR LatinLetterOrUnderscore
            _CSharpSyntaxMatcher_Item _r3;

            _r3 = _MemoCall(_memo, "LatinLetterOrUnderscore", _index, LatinLetterOrUnderscore, null);

            if (_r3 != null) _index = _r3.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // STAR 4
            int _start_i4 = _index;
            var _res4 = Enumerable.Empty<IToken>();
        label4:

            // OR 5
            int _start_i5 = _index;

            // CALLORVAR LatinLetterOrUnderscore
            _CSharpSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "LatinLetterOrUnderscore", _index, LatinLetterOrUnderscore, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i5; } else goto label5;

            // INPUT CLASS
            _ParseInputClass(_memo, ref _index, '\u0030', '\u0031', '\u0032', '\u0033', '\u0034', '\u0035', '\u0036', '\u0037', '\u0038', '\u0039');

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
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _res4.Where(_NON_NULL), true));
            }

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new CSharpIdentifierToken(value.Inputs.JoinCharsToString()); }, _r0), true) );
            }

        }


        public void CSharpTypeNameWithNamespace(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _CSharpSyntaxMatcher_Item value = null;

            // CALLORVAR CSharpChainedIdentifiers
            _CSharpSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "CSharpChainedIdentifiers", _index, CSharpChainedIdentifiers, null);

            if (_r2 != null) _index = _r2.NextIndex;

            // BIND value
            value = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new CSharpTypeNameWithNamespaceToken(((ContainerToken<string[]>) value.Results.Single()).Value.JoinToString(".")); }, _r0), true) );
            }

        }


        public void CSharpType(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // CALLORVAR CShaptClassicTypeDefinition
            _CSharpSyntaxMatcher_Item _r1;

            _r1 = _MemoCall(_memo, "CShaptClassicTypeDefinition", _index, CShaptClassicTypeDefinition, null);

            if (_r1 != null) _index = _r1.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // CALLORVAR CShaptTupleDefinition
            _CSharpSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "CShaptTupleDefinition", _index, CShaptTupleDefinition, null);

            if (_r2 != null) _index = _r2.NextIndex;

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void CShaptClassicTypeDefinition(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _CSharpSyntaxMatcher_Item type = null;
            _CSharpSyntaxMatcher_Item arguments = null;

            // AND 1
            int _start_i1 = _index;

            // CALLORVAR CSharpTypeNameWithNamespace
            _CSharpSyntaxMatcher_Item _r3;

            _r3 = _MemoCall(_memo, "CSharpTypeNameWithNamespace", _index, CSharpTypeNameWithNamespace, null);

            if (_r3 != null) _index = _r3.NextIndex;

            // BIND type
            type = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR CSharpTypeArguments
            _CSharpSyntaxMatcher_Item _r6;

            _r6 = _MemoCall(_memo, "CSharpTypeArguments", _index, CSharpTypeArguments, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_index, _memo.InputEnumerable)); }

            // BIND arguments
            arguments = _memo.Results.Peek();

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new ClassicCSharpTypeToken(
            (CSharpTypeNameWithNamespaceToken) type.Results.Single(),
            arguments.Results.Cast<ICSharpTypeToken>().ToArray()
        ); }, _r0), true) );
            }

        }


        public void CSharpNamespace(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // CALLORVAR CSharpTypeNameWithNamespace
            _CSharpSyntaxMatcher_Item _r0;

            _r0 = _MemoCall(_memo, "CSharpTypeNameWithNamespace", _index, CSharpTypeNameWithNamespace, null);

            if (_r0 != null) _index = _r0.NextIndex;

        }


        public void CSharpTypeArguments(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
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

            // AND 4
            int _start_i4 = _index;

            // LITERAL '<'
            _ParseLiteralChar(_memo, ref _index, '<');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // STAR 6
            int _start_i6 = _index;
            var _res6 = Enumerable.Empty<IToken>();
        label6:

            // CALLORVAR SPACING
            _CSharpSyntaxMatcher_Item _r7;

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
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i6, _index, _memo.InputEnumerable, _res6.Where(_NON_NULL), true));
            }

        label4: // AND
            var _r4_2 = _memo.Results.Pop();
            var _r4_1 = _memo.Results.Pop();

            if (_r4_1 != null && _r4_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _r4_1.Results.Concat(_r4_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i4;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // CALLORVAR CSharpType
            _CSharpSyntaxMatcher_Item _r8;

            _r8 = _MemoCall(_memo, "CSharpType", _index, CSharpType, null);

            if (_r8 != null) _index = _r8.NextIndex;

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // STAR 9
            int _start_i9 = _index;
            var _res9 = Enumerable.Empty<IToken>();
        label9:

            // AND 10
            int _start_i10 = _index;

            // AND 11
            int _start_i11 = _index;

            // AND 12
            int _start_i12 = _index;

            // STAR 13
            int _start_i13 = _index;
            var _res13 = Enumerable.Empty<IToken>();
        label13:

            // CALLORVAR SPACING
            _CSharpSyntaxMatcher_Item _r14;

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
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i13, _index, _memo.InputEnumerable, _res13.Where(_NON_NULL), true));
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label12; }

            // LITERAL ','
            _ParseLiteralChar(_memo, ref _index, ',');

        label12: // AND
            var _r12_2 = _memo.Results.Pop();
            var _r12_1 = _memo.Results.Pop();

            if (_r12_1 != null && _r12_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i12, _index, _memo.InputEnumerable, _r12_1.Results.Concat(_r12_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i12;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label11; }

            // STAR 16
            int _start_i16 = _index;
            var _res16 = Enumerable.Empty<IToken>();
        label16:

            // CALLORVAR SPACING
            _CSharpSyntaxMatcher_Item _r17;

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
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i16, _index, _memo.InputEnumerable, _res16.Where(_NON_NULL), true));
            }

        label11: // AND
            var _r11_2 = _memo.Results.Pop();
            var _r11_1 = _memo.Results.Pop();

            if (_r11_1 != null && _r11_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i11, _index, _memo.InputEnumerable, _r11_1.Results.Concat(_r11_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i11;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label10; }

            // CALLORVAR CSharpType
            _CSharpSyntaxMatcher_Item _r18;

            _r18 = _MemoCall(_memo, "CSharpType", _index, CSharpType, null);

            if (_r18 != null) _index = _r18.NextIndex;

        label10: // AND
            var _r10_2 = _memo.Results.Pop();
            var _r10_1 = _memo.Results.Pop();

            if (_r10_1 != null && _r10_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i10, _index, _memo.InputEnumerable, _r10_1.Results.Concat(_r10_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i10;
            }

            // STAR 9
            var _r9 = _memo.Results.Pop();
            if (_r9 != null)
            {
                _res9 = _res9.Concat(_r9.Results);
                goto label9;
            }
            else
            {
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i9, _index, _memo.InputEnumerable, _res9.Where(_NON_NULL), true));
            }

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // STAR 19
            int _start_i19 = _index;
            var _res19 = Enumerable.Empty<IToken>();
        label19:

            // CALLORVAR SPACING
            _CSharpSyntaxMatcher_Item _r20;

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
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i19, _index, _memo.InputEnumerable, _res19.Where(_NON_NULL), true));
            }

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label0; }

            // LITERAL '>'
            _ParseLiteralChar(_memo, ref _index, '>');

        label0: // AND
            var _r0_2 = _memo.Results.Pop();
            var _r0_1 = _memo.Results.Pop();

            if (_r0_1 != null && _r0_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i0, _index, _memo.InputEnumerable, _r0_1.Results.Concat(_r0_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i0;
            }

        }


        public void CShaptTupleDefinition(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _CSharpSyntaxMatcher_Item types = null;

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

            // LITERAL '('
            _ParseLiteralChar(_memo, ref _index, '(');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label6; }

            // STAR 8
            int _start_i8 = _index;
            var _res8 = Enumerable.Empty<IToken>();
        label8:

            // CALLORVAR SPACING
            _CSharpSyntaxMatcher_Item _r9;

            _r9 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r9 != null) _index = _r9.NextIndex;

            // STAR 8
            var _r8 = _memo.Results.Pop();
            if (_r8 != null)
            {
                _res8 = _res8.Concat(_r8.Results);
                goto label8;
            }
            else
            {
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i8, _index, _memo.InputEnumerable, _res8.Where(_NON_NULL), true));
            }

        label6: // AND
            var _r6_2 = _memo.Results.Pop();
            var _r6_1 = _memo.Results.Pop();

            if (_r6_1 != null && _r6_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i6, _index, _memo.InputEnumerable, _r6_1.Results.Concat(_r6_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i6;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label5; }

            // CALLORVAR CSharpTupleItem
            _CSharpSyntaxMatcher_Item _r10;

            _r10 = _MemoCall(_memo, "CSharpTupleItem", _index, CSharpTupleItem, null);

            if (_r10 != null) _index = _r10.NextIndex;

        label5: // AND
            var _r5_2 = _memo.Results.Pop();
            var _r5_1 = _memo.Results.Pop();

            if (_r5_1 != null && _r5_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _r5_1.Results.Concat(_r5_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i5;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // PLUS 11
            int _start_i11 = _index;
            var _res11 = Enumerable.Empty<IToken>();
        label11:

            // AND 12
            int _start_i12 = _index;

            // AND 13
            int _start_i13 = _index;

            // AND 14
            int _start_i14 = _index;

            // STAR 15
            int _start_i15 = _index;
            var _res15 = Enumerable.Empty<IToken>();
        label15:

            // CALLORVAR SPACING
            _CSharpSyntaxMatcher_Item _r16;

            _r16 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r16 != null) _index = _r16.NextIndex;

            // STAR 15
            var _r15 = _memo.Results.Pop();
            if (_r15 != null)
            {
                _res15 = _res15.Concat(_r15.Results);
                goto label15;
            }
            else
            {
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i15, _index, _memo.InputEnumerable, _res15.Where(_NON_NULL), true));
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label14; }

            // LITERAL ','
            _ParseLiteralChar(_memo, ref _index, ',');

        label14: // AND
            var _r14_2 = _memo.Results.Pop();
            var _r14_1 = _memo.Results.Pop();

            if (_r14_1 != null && _r14_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i14, _index, _memo.InputEnumerable, _r14_1.Results.Concat(_r14_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i14;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label13; }

            // STAR 18
            int _start_i18 = _index;
            var _res18 = Enumerable.Empty<IToken>();
        label18:

            // CALLORVAR SPACING
            _CSharpSyntaxMatcher_Item _r19;

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
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i18, _index, _memo.InputEnumerable, _res18.Where(_NON_NULL), true));
            }

        label13: // AND
            var _r13_2 = _memo.Results.Pop();
            var _r13_1 = _memo.Results.Pop();

            if (_r13_1 != null && _r13_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i13, _index, _memo.InputEnumerable, _r13_1.Results.Concat(_r13_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i13;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label12; }

            // CALLORVAR CSharpTupleItem
            _CSharpSyntaxMatcher_Item _r20;

            _r20 = _MemoCall(_memo, "CSharpTupleItem", _index, CSharpTupleItem, null);

            if (_r20 != null) _index = _r20.NextIndex;

        label12: // AND
            var _r12_2 = _memo.Results.Pop();
            var _r12_1 = _memo.Results.Pop();

            if (_r12_1 != null && _r12_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i12, _index, _memo.InputEnumerable, _r12_1.Results.Concat(_r12_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i12;
            }

            // PLUS 11
            var _r11 = _memo.Results.Pop();
            if (_r11 != null)
            {
                _res11 = _res11.Concat(_r11.Results);
                goto label11;
            }
            else
            {
                if (_index > _start_i11)
                    _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i11, _index, _memo.InputEnumerable, _res11.Where(_NON_NULL), true));
                else
                    _memo.Results.Push(null);
            }

        label4: // AND
            var _r4_2 = _memo.Results.Pop();
            var _r4_1 = _memo.Results.Pop();

            if (_r4_1 != null && _r4_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _r4_1.Results.Concat(_r4_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i4;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // STAR 21
            int _start_i21 = _index;
            var _res21 = Enumerable.Empty<IToken>();
        label21:

            // CALLORVAR SPACING
            _CSharpSyntaxMatcher_Item _r22;

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
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i21, _index, _memo.InputEnumerable, _res21.Where(_NON_NULL), true));
            }

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // LITERAL ')'
            _ParseLiteralChar(_memo, ref _index, ')');

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // BIND types
            types = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new TupleCSharpTypeToken(types.Results.Cast<CSharpTupleItemToken>().ToArray()); }, _r0), true) );
            }

        }


        public void CSharpTupleItem(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _CSharpSyntaxMatcher_Item type = null;
            _CSharpSyntaxMatcher_Item propertyName = null;

            // AND 1
            int _start_i1 = _index;

            // CALLORVAR CSharpType
            _CSharpSyntaxMatcher_Item _r3;

            _r3 = _MemoCall(_memo, "CSharpType", _index, CSharpType, null);

            if (_r3 != null) _index = _r3.NextIndex;

            // BIND type
            type = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // AND 6
            int _start_i6 = _index;

            // PLUS 7
            int _start_i7 = _index;
            var _res7 = Enumerable.Empty<IToken>();
        label7:

            // CALLORVAR SPACING
            _CSharpSyntaxMatcher_Item _r8;

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
                    _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i7, _index, _memo.InputEnumerable, _res7.Where(_NON_NULL), true));
                else
                    _memo.Results.Push(null);
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label6; }

            // CALLORVAR CSharpIdentifier
            _CSharpSyntaxMatcher_Item _r9;

            _r9 = _MemoCall(_memo, "CSharpIdentifier", _index, CSharpIdentifier, null);

            if (_r9 != null) _index = _r9.NextIndex;

        label6: // AND
            var _r6_2 = _memo.Results.Pop();
            var _r6_1 = _memo.Results.Pop();

            if (_r6_1 != null && _r6_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i6, _index, _memo.InputEnumerable, _r6_1.Results.Concat(_r6_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i6;
            }

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_index, _memo.InputEnumerable)); }

            // BIND propertyName
            propertyName = _memo.Results.Peek();

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new CSharpTupleItemToken(
            (ICSharpTypeToken) type.Results.Single(),
            (CSharpIdentifierToken) propertyName.Results.FirstOrDefault()
        ); }, _r0), true) );
            }

        }


        public void CSharpChainedMemberAccess(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _CSharpSyntaxMatcher_Item value = null;

            // CALLORVAR CSharpChainedIdentifiers
            _CSharpSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "CSharpChainedIdentifiers", _index, CSharpChainedIdentifiers, null);

            if (_r2 != null) _index = _r2.NextIndex;

            // BIND value
            value = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new CSharpChainedMemberAccessToken(((ContainerToken<string[]>) value.Results.Single()).Value); }, _r0), true) );
            }

        }


        public void CSharpEmptyMethodBody(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _CSharpSyntaxMatcher_Item value = null;

            // CALLORVAR CSharpEmptyCodeBlock
            _CSharpSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "CSharpEmptyCodeBlock", _index, CSharpEmptyCodeBlock, null);

            if (_r2 != null) _index = _r2.NextIndex;

            // BIND value
            value = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new ContainerToken<string>(value.Inputs.JoinCharsToString()); }, _r0), true) );
            }

        }


        public void CSharpMethodBody(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _CSharpSyntaxMatcher_Item value = null;

            // CALLORVAR CSharpCodeBlock
            _CSharpSyntaxMatcher_Item _r2;

            _r2 = _MemoCall(_memo, "CSharpCodeBlock", _index, CSharpCodeBlock, null);

            if (_r2 != null) _index = _r2.NextIndex;

            // BIND value
            value = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new ContainerToken<string>(value.Inputs.JoinCharsToString()); }, _r0), true) );
            }

        }


        public void LatinLetterOrUnderscore(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _CSharpSyntaxMatcher_Item value = null;

            // INPUT CLASS
            _ParseInputClass(_memo, ref _index, '\u0061', '\u0062', '\u0063', '\u0064', '\u0065', '\u0066', '\u0067', '\u0068', '\u0069', '\u006a', '\u006b', '\u006c', '\u006d', '\u006e', '\u006f', '\u0070', '\u0071', '\u0072', '\u0073', '\u0074', '\u0075', '\u0076', '\u0077', '\u0078', '\u0079', '\u007a', '\u0041', '\u0042', '\u0043', '\u0044', '\u0045', '\u0046', '\u0047', '\u0048', '\u0049', '\u004a', '\u004b', '\u004c', '\u004d', '\u004e', '\u004f', '\u0050', '\u0051', '\u0052', '\u0053', '\u0054', '\u0055', '\u0056', '\u0057', '\u0058', '\u0059', '\u005a', '_');

            // BIND value
            value = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new ContainerToken<char>(value.Inputs.First()); }, _r0), true) );
            }

        }


        public void CSharpEmptyCodeBlock(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // AND 0
            int _start_i0 = _index;

            // AND 1
            int _start_i1 = _index;

            // LITERAL '{'
            _ParseLiteralChar(_memo, ref _index, '{');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // STAR 3
            int _start_i3 = _index;
            var _res3 = Enumerable.Empty<IToken>();
        label3:

            // CALLORVAR SPACING
            _CSharpSyntaxMatcher_Item _r4;

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
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _res3.Where(_NON_NULL), true));
            }

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i0, _index, _memo.InputEnumerable, _r0_1.Results.Concat(_r0_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i0;
            }

        }


        public void CSharpCodeBlock(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // AND 0
            int _start_i0 = _index;

            // AND 1
            int _start_i1 = _index;

            // LITERAL '{'
            _ParseLiteralChar(_memo, ref _index, '{');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // STAR 3
            int _start_i3 = _index;
            var _res3 = Enumerable.Empty<IToken>();
        label3:

            // AND 4
            int _start_i4 = _index;

            // NOT 5
            int _start_i5 = _index;

            // LITERAL '}'
            _ParseLiteralChar(_memo, ref _index, '}');

            // NOT 5
            var _r5 = _memo.Results.Pop();
            _memo.Results.Push( _r5 == null ? new _CSharpSyntaxMatcher_Item(_start_i5, _memo.InputEnumerable) : null);
            _index = _start_i5;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // OR 7
            int _start_i7 = _index;

            // OR 8
            int _start_i8 = _index;

            // OR 9
            int _start_i9 = _index;

            // CALLORVAR EOL
            _CSharpSyntaxMatcher_Item _r10;

            _r10 = _MemoCall(_memo, "EOL", _index, EOL, null);

            if (_r10 != null) _index = _r10.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i9; } else goto label9;

            // CALLORVAR CSharpComment
            _CSharpSyntaxMatcher_Item _r11;

            _r11 = _MemoCall(_memo, "CSharpComment", _index, CSharpComment, null);

            if (_r11 != null) _index = _r11.NextIndex;

        label9: // OR
            int _dummy_i9 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i8; } else goto label8;

            // CALLORVAR CSharpCodeBlock
            _CSharpSyntaxMatcher_Item _r12;

            _r12 = _MemoCall(_memo, "CSharpCodeBlock", _index, CSharpCodeBlock, null);

            if (_r12 != null) _index = _r12.NextIndex;

        label8: // OR
            int _dummy_i8 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i7; } else goto label7;

            // ANY
            _ParseAny(_memo, ref _index);

        label7: // OR
            int _dummy_i7 = _index; // no-op for label

        label4: // AND
            var _r4_2 = _memo.Results.Pop();
            var _r4_1 = _memo.Results.Pop();

            if (_r4_1 != null && _r4_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _r4_1.Results.Concat(_r4_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i4;
            }

            // STAR 3
            var _r3 = _memo.Results.Pop();
            if (_r3 != null)
            {
                _res3 = _res3.Concat(_r3.Results);
                goto label3;
            }
            else
            {
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i3, _index, _memo.InputEnumerable, _res3.Where(_NON_NULL), true));
            }

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i0, _index, _memo.InputEnumerable, _r0_1.Results.Concat(_r0_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i0;
            }

        }


        public void CSharpComment(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            // OR 0
            int _start_i0 = _index;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // LITERAL "//"
            _ParseLiteralString(_memo, ref _index, "//");

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // STAR 4
            int _start_i4 = _index;
            var _res4 = Enumerable.Empty<IToken>();
        label4:

            // AND 5
            int _start_i5 = _index;

            // NOT 6
            int _start_i6 = _index;

            // CALLORVAR EOL
            _CSharpSyntaxMatcher_Item _r7;

            _r7 = _MemoCall(_memo, "EOL", _index, EOL, null);

            if (_r7 != null) _index = _r7.NextIndex;

            // NOT 6
            var _r6 = _memo.Results.Pop();
            _memo.Results.Push( _r6 == null ? new _CSharpSyntaxMatcher_Item(_start_i6, _memo.InputEnumerable) : null);
            _index = _start_i6;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label5; }

            // ANY
            _ParseAny(_memo, ref _index);

        label5: // AND
            var _r5_2 = _memo.Results.Pop();
            var _r5_1 = _memo.Results.Pop();

            if (_r5_1 != null && _r5_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _r5_1.Results.Concat(_r5_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _res4.Where(_NON_NULL), true));
            }

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR EOL
            _CSharpSyntaxMatcher_Item _r9;

            _r9 = _MemoCall(_memo, "EOL", _index, EOL, null);

            if (_r9 != null) _index = _r9.NextIndex;

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // AND 10
            int _start_i10 = _index;

            // AND 11
            int _start_i11 = _index;

            // LITERAL "/*"
            _ParseLiteralString(_memo, ref _index, "/*");

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label11; }

            // STAR 13
            int _start_i13 = _index;
            var _res13 = Enumerable.Empty<IToken>();
        label13:

            // AND 14
            int _start_i14 = _index;

            // NOT 15
            int _start_i15 = _index;

            // LITERAL "*/"
            _ParseLiteralString(_memo, ref _index, "*/");

            // NOT 15
            var _r15 = _memo.Results.Pop();
            _memo.Results.Push( _r15 == null ? new _CSharpSyntaxMatcher_Item(_start_i15, _memo.InputEnumerable) : null);
            _index = _start_i15;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label14; }

            // OR 17
            int _start_i17 = _index;

            // CALLORVAR EOL
            _CSharpSyntaxMatcher_Item _r18;

            _r18 = _MemoCall(_memo, "EOL", _index, EOL, null);

            if (_r18 != null) _index = _r18.NextIndex;

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i17; } else goto label17;

            // ANY
            _ParseAny(_memo, ref _index);

        label17: // OR
            int _dummy_i17 = _index; // no-op for label

        label14: // AND
            var _r14_2 = _memo.Results.Pop();
            var _r14_1 = _memo.Results.Pop();

            if (_r14_1 != null && _r14_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i14, _index, _memo.InputEnumerable, _r14_1.Results.Concat(_r14_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i13, _index, _memo.InputEnumerable, _res13.Where(_NON_NULL), true));
            }

        label11: // AND
            var _r11_2 = _memo.Results.Pop();
            var _r11_1 = _memo.Results.Pop();

            if (_r11_1 != null && _r11_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i11, _index, _memo.InputEnumerable, _r11_1.Results.Concat(_r11_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i11;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label10; }

            // LITERAL "*/"
            _ParseLiteralString(_memo, ref _index, "*/");

        label10: // AND
            var _r10_2 = _memo.Results.Pop();
            var _r10_1 = _memo.Results.Pop();

            if (_r10_1 != null && _r10_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i10, _index, _memo.InputEnumerable, _r10_1.Results.Concat(_r10_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i10;
            }

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


        public void CSharpChainedIdentifiers(_CSharpSyntaxMatcher_Memo _memo, int _index, _CSharpSyntaxMatcher_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _CSharpSyntaxMatcher_Item chain = null;

            // AND 2
            int _start_i2 = _index;

            // CALLORVAR CSharpIdentifier
            _CSharpSyntaxMatcher_Item _r3;

            _r3 = _MemoCall(_memo, "CSharpIdentifier", _index, CSharpIdentifier, null);

            if (_r3 != null) _index = _r3.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // STAR 4
            int _start_i4 = _index;
            var _res4 = Enumerable.Empty<IToken>();
        label4:

            // AND 5
            int _start_i5 = _index;

            // AND 6
            int _start_i6 = _index;

            // AND 7
            int _start_i7 = _index;

            // STAR 8
            int _start_i8 = _index;
            var _res8 = Enumerable.Empty<IToken>();
        label8:

            // CALLORVAR SPACING
            _CSharpSyntaxMatcher_Item _r9;

            _r9 = _MemoCall(_memo, "SPACING", _index, SPACING, null);

            if (_r9 != null) _index = _r9.NextIndex;

            // STAR 8
            var _r8 = _memo.Results.Pop();
            if (_r8 != null)
            {
                _res8 = _res8.Concat(_r8.Results);
                goto label8;
            }
            else
            {
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i8, _index, _memo.InputEnumerable, _res8.Where(_NON_NULL), true));
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label7; }

            // LITERAL '.'
            _ParseLiteralChar(_memo, ref _index, '.');

        label7: // AND
            var _r7_2 = _memo.Results.Pop();
            var _r7_1 = _memo.Results.Pop();

            if (_r7_1 != null && _r7_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i7, _index, _memo.InputEnumerable, _r7_1.Results.Concat(_r7_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i7;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label6; }

            // STAR 11
            int _start_i11 = _index;
            var _res11 = Enumerable.Empty<IToken>();
        label11:

            // CALLORVAR SPACING
            _CSharpSyntaxMatcher_Item _r12;

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
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i11, _index, _memo.InputEnumerable, _res11.Where(_NON_NULL), true));
            }

        label6: // AND
            var _r6_2 = _memo.Results.Pop();
            var _r6_1 = _memo.Results.Pop();

            if (_r6_1 != null && _r6_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i6, _index, _memo.InputEnumerable, _r6_1.Results.Concat(_r6_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i6;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label5; }

            // CALLORVAR CSharpIdentifier
            _CSharpSyntaxMatcher_Item _r13;

            _r13 = _MemoCall(_memo, "CSharpIdentifier", _index, CSharpIdentifier, null);

            if (_r13 != null) _index = _r13.NextIndex;

        label5: // AND
            var _r5_2 = _memo.Results.Pop();
            var _r5_1 = _memo.Results.Pop();

            if (_r5_1 != null && _r5_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i5, _index, _memo.InputEnumerable, _r5_1.Results.Concat(_r5_2.Results).Where(_NON_NULL), true) );
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
                _memo.Results.Push(new _CSharpSyntaxMatcher_Item(_start_i4, _index, _memo.InputEnumerable, _res4.Where(_NON_NULL), true));
            }

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // BIND chain
            chain = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _CSharpSyntaxMatcher_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new ContainerToken<string[]>(
            chain
                .Results
                .Cast<CSharpIdentifierToken>()
                .Select(container => container.Value)
                .ToArray()
        ); }, _r0), true) );
            }

        }


    } // class CSharpSyntaxMatcher

} // namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Grammar

