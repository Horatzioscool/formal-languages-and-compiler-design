using System;
using System.IO;
using System.Linq;

namespace FLTC.Lab2
{

    public interface IScanner
    {
        void Scan(StreamReader input);
    }

    public class Scanner : IScanner
    {
        private readonly ISymbolTable symbolTable;
        private readonly IProgramInternalForm pif;
        public Scanner(ISymbolTable symbolTable, IProgramInternalForm pif)
        {
            this.symbolTable = symbolTable;
            this.pif = pif;
        }

        public void Scan(StreamReader input)
        {
            var reservedWord = new ReservedWordTokenType();
            var delimiter = new DelimiterTokenType();
            var separator = new SeparatorTokenType();
            var identifier = new IdentifierTokenType();
            var constant = new ConstantTokenType();

            while (!input.EndOfStream)
            {
                var token = Detect(input);

                if (token.Is(separator))
                {
                    pif.Add((token, "NAN"));
                    continue;
                }

                if (token.Is(delimiter))
                {
                    pif.Add((token, "DEL"));
                    continue;
                }

                if (token.Is(reservedWord))
                {
                    pif.Add((token, "RES"));
                    continue;
                }

                if (token.Is(identifier))
                {
                    var index = symbolTable.Insert(token);
                    pif.Add((token, "SYM"));
                    continue;
                }

                if (token.Is(constant))
                {
                    var index = symbolTable.Insert(token);
                    pif.Add((token, "SYM"));
                    continue;
                }

                throw new ApplicationException("Lexical error");
            }
        }

        private bool IsSeparator(char c)
        {
            return SeparatorTokenType.Separators.Contains(c);
        }

        private bool IsDelimiter(char c)
        {
            return DelimiterTokenType.Delimiters.Contains(c);
        }

        public string Detect(StreamReader input)
        {
            var buffer = "";
            var shouldContinue = true;
            var isSeparatorBuffer = false;

            while (shouldContinue && !input.EndOfStream)
            {
                char nextChar = (char) input.Peek();

                if (IsSeparator(nextChar))
                {
                    if(buffer.Length == 0)
                    {
                        isSeparatorBuffer = true;
                        buffer += nextChar;
                        input.Read();
                    }
                    else if(isSeparatorBuffer)
                    {
                        buffer += nextChar;
                        input.Read();
                    }
                    else
                    {
                        return buffer;
                    }
                }
                else if (IsDelimiter(nextChar))
                {
                    if(buffer.Length == 0)
                    {
                        buffer += nextChar;
                        input.Read();
                    }
                    return buffer;
                }
                else
                {
                    if (buffer.Length == 0)
                    {
                        isSeparatorBuffer = false;
                        buffer += nextChar;
                        input.Read();
                    }
                    else if (isSeparatorBuffer)
                    {
                        return buffer;
                    }
                    else
                    {
                        buffer += nextChar;
                        input.Read();
                    }
                }
            }
            return buffer;
        }
    }
}
