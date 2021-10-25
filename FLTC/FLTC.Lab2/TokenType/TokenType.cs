using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FLTC.Lab2
{
    public interface TokenType
    {
        bool Matches(string text);
    }

    public class ReservedTokenType : TokenType
    {
        private static IEnumerable<string> WordsFrom(string text)
        {
            return text.Replace("\\W", " ").Split(" ").Select(s => s.ToLowerInvariant());
        }

        public static readonly IEnumerable<string> ReservedWords = Enumerable.Empty<string>()
            .Concat(WordsFrom("thus is, known as:"))
            .Concat(WordsFrom("numeric text complex"))
            .Concat(WordsFrom("when, then: , when not:"))
            .Concat(WordsFrom("While, ;"))
            .Concat(WordsFrom("shall be"))
            .Concat(WordsFrom("added with, subtracted by, multiply with, divide by"))
            .Concat(WordsFrom("read, shall be written"))
            .Concat(WordsFrom("true, false"))
            .Concat(WordsFrom("equals, greater than, lesser than, different to"));

        public static readonly IEnumerable<char> Delimiters = new[]
        {
            '.',
            ':',
            ';',
            ','
        };

        public static readonly IEnumerable<char> Separators = new[]
        {
            ' ',
            '\t',
            '\n',
            '\r'
        };

        public bool Matches(string text)
        {
            var lowerText = text.ToLowerInvariant();
            return ReservedWords.Any(s => s == lowerText)
                || Delimiters.Any(o => new string(new [] { o }) == lowerText)
                || Separators.Any(s => new Regex($"{s}+").IsMatch(text));
        }
    }

    public class IdentifierTokenType : TokenType
    {
        private readonly Regex identifierRegex = new Regex("[a-zA-Z]\\w*");
        public bool Matches(string text)
        {
            return identifierRegex.IsMatch(text);
        }
    }

    public class ConstantTokenType : TokenType
    {
        public readonly Regex constantRegex = new Regex("((\\+|\\-)?[1-9][0-9]*|0)|(\"\\w*\")");
        public bool Matches(string text)
        {

            return constantRegex.IsMatch(text);
        }
    }
}
