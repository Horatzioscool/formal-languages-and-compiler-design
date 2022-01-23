using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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

                var left = split.ElementAt(0);

                var leftNodes = left.Split("|", StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim());

                foreach(var node in leftNodes)
                {
                    if (node.Contains(Grammar.Node.StartNodePrefix))
                    {
                        grammar.AddStarting(node.Replace(Grammar.Node.StartNodePrefix, ""));
                    }
                    else if (Regex.IsMatch(node, "\\\".+\\\"") && isTokenOrLexicalRule(Regex.Replace(node, "\"", "")))
                    {
                        grammar.AddTerminal(node);
                    }
                    else
                    {
                        grammar.AddNode(node);
                    }
                }

                leftNodes = leftNodes.Select(n => n.Contains(Grammar.Node.StartNodePrefix) ? n.Replace(Grammar.Node.StartNodePrefix, "") : n);

                var right = split.ElementAt(1);

                var rightNodes = right.Split("|", StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim());

                foreach (var node in rightNodes)
                {
                    var subNodes = node.Split("&", StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim()).ToArray();
                    foreach(var subNode in subNodes)
                    {
                        if ((Regex.IsMatch(subNode, "\\\".+\\\"") && isTokenOrLexicalRule(Regex.Replace(subNode, "\"", ""))) || subNode == Grammar.Node.EmptyString || subNode == "[SYM]" || subNode == "[CNST]")
                        {
                            grammar.AddTerminal(subNode);
                        }
                        else
                        {
                            grammar.AddNode(subNode);
                        }
                    }

                    grammar.AddProduction(subNodes, leftNodes.ToArray());
                }
            }

            return grammar;
        }
    }
}
