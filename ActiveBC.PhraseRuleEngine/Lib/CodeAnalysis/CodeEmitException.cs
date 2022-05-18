using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Helper;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using Microsoft.CodeAnalysis;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis
{
    public sealed class CodeEmitException : Exception
    {
        public CodeEmitException(string message, string code, IReadOnlyCollection<Diagnostic> diagnostics) : base(message)
        {
            this.Data["code"] = code;
            this.Data["code_sample"] = diagnostics
                .Select(diagnostic => CodeHighlightHelper.DiagnosticToString(diagnostic, code))
                .JoinToString("\r\n");
        }
    }
}