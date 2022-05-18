﻿namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens
{
    public sealed class LiteralToken : ITerminalToken, ILiteralSetMemberToken
    {
        public string Literal { get; }

        public LiteralToken(string literal)
        {
            this.Literal = literal;
        }

        public override string ToString()
        {
            return $"{this.Literal}";
        }
    }
}