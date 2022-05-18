using System;
using System.Collections;
using System.Linq;

namespace ActiveBC.PhraseRuleEngine.Lib.Common.Helpers
{
    public static class ErrorIndexHelper
    {
        public static void FillExceptionData(IDictionary data, string source, string? error = null, int? errorIndex = null)
        {
            data["source"] = source;
            data["error"] = error;

            if (errorIndex is not null)
            {
                data["error_index"] = errorIndex.Value;

                ErrorContext context = FindContext(source, errorIndex.Value);

                data["error_line_index"] = context.LineIndex;
                data["error_position_in_line"] = context.PositionInLine;

                if (context.Highlighter is not null)
                {
                    data["error_character"] = context.Highlighter.ErrorCharacter;
                    data["error_context"] = context.Highlighter.StringAroundError;
                }
            }
        }

        public static ErrorContext FindContext(string source, int errorIndex, int contextSize = 5)
        {
            const string separator = "\r\n";

            string[] lines = source.Split("\r\n");

            int accumulatedLength = 0;
            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                string line = lines[lineIndex];

                if (line.Length + separator.Length + accumulatedLength > errorIndex)
                {
                    int positionInLine = errorIndex - accumulatedLength;

                    ErrorHighlighter? highlighter = null;

                    if (positionInLine < line.Length)
                    {
                        highlighter = new ErrorHighlighter(line[positionInLine], CutLineAroundError(line, positionInLine, contextSize));
                    }

                    return new ErrorContext(lineIndex, positionInLine, highlighter);
                }

                accumulatedLength += line.Length + separator.Length;
            }

            throw new ArgumentOutOfRangeException(nameof(errorIndex), errorIndex, "Error index exceeds source length");
        }

        private static string CutLineAroundError(string line, int positionInLine, int contextSize)
        {
            return line
                .Skip(Math.Max(0, positionInLine - contextSize))
                .Take(Math.Min(positionInLine, contextSize) + 1 + contextSize)
                .JoinCharsToString();
        }

        public sealed class ErrorContext
        {
            public int LineIndex { get; }
            public int PositionInLine { get; }
            public ErrorHighlighter? Highlighter { get; }

            public ErrorContext(int lineIndex, int positionInLine, ErrorHighlighter? highlighter)
            {
                this.LineIndex = lineIndex;
                this.PositionInLine = positionInLine;
                this.Highlighter = highlighter;
            }
        }

        public sealed class ErrorHighlighter
        {
            public char ErrorCharacter { get; }
            public string StringAroundError { get; }

            public ErrorHighlighter(char errorCharacter, string stringAroundError)
            {
                this.ErrorCharacter = errorCharacter;
                this.StringAroundError = stringAroundError;
            }
        }
    }
}