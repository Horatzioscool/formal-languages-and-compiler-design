using System;
using System.Collections.Generic;
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
            var reservedWord = new ReservedTokenType();
            var identifier = new IdentifierTokenType();
            var constant = new ConstantTokenType();

            while (!input.EndOfStream)
            {
                var token = Detect(input);

                if (token.Is(reservedWord))
                {
                    // genPIF(token, 0)
                    pif.Add((token, 0));
                    continue;
                }

                if(token.Is(identifier) || token.Is(constant))
                {
                    var index = symbolTable.Insert(token);
                    // genPIF(token, index)
                    pif.Add((token, index.Item1));
                    continue;
                }

                throw new ApplicationException("Lexical error");
            }
        }

        private bool IsSeparator(char c)
        {
            return ReservedTokenType.Separators.Contains(c);
        }

        private bool IsOperator(char c)
        {
            return ReservedTokenType.Operators.Contains(c);
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
                else if (IsOperator(nextChar))
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
