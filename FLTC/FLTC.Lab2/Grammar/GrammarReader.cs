using System;
using System.IO;
using System.Linq;

namespace FLTC.Lab2.Grammar
{
    internal class GrammarReader
    {
        public Grammar ReadGrammar(string file, Func<string, bool> isTokenOrLexicalRule)
        {
            // non-terminals - strings
            // terminals - strings
            // productions - numbered, lefthandside - sequence of strings, righthandside - sequence of strings
            // starting symbol
            Grammar grammar = new Grammar();

            string grammarSyntax = File.ReadAllText(file);

            var lines = grammarSyntax.Split(new [] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach(var line in lines)
            {
                var split = line.Split(new[] { "::=" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());

                var right = split.ElementAt(0);

                var rightNodes = right.Split("|", StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim());

                foreach(var node in rightNodes)
                {
                    if (node.Contains(Grammar.Node.StartNodePrefix))
                    {
                        grammar.AddStarting(node.Replace(Grammar.Node.StartNodePrefix, ""));
                    }
                    else if (isTokenOrLexicalRule(node))
                    {
                        grammar.AddTerminal(node);
                    }
                    else
                    {
                        grammar.AddNode(node);
                    }
                }

                var left = split.ElementAt(1);

                var leftNodes = left.Split("|", StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim());

                foreach (var node in leftNodes)
                {
                    if (node.Contains(Grammar.Node.StartNodePrefix))
                    {
                        grammar.AddStarting(node.Replace(Grammar.Node.StartNodePrefix, ""));
                    }
                    else if (isTokenOrLexicalRule(node) || node == Grammar.Node.EmptyString)
                    {
                        grammar.AddTerminal(node);
                    }
                    else
                    {
                        grammar.AddNode(node);
                    }
                }

                leftNodes = leftNodes.Select(n => n.Contains(Grammar.Node.StartNodePrefix) ? n.Replace(Grammar.Node.StartNodePrefix, "") : n);

                grammar.AddProduction(rightNodes.ToArray(), leftNodes.ToArray());
            }

            return grammar;
        }

        internal object ReadGrammar(string v)
        {
            throw new NotImplementedException();
        }
    }
}
