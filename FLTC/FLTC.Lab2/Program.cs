using FLTC.Lab2.Grammar;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FLTC.Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            //var menu = new FiniteAutomatonMenu();

            //menu.Start();

            //var testString = "-1";

            //Console.WriteLine(new ConstantTokenType().constantRegex.IsMatch(testString));

            //var reader = new GrammarReader();

            //var grammar = reader.ReadGrammar("./Syntax.in", (name) =>
            //{
            //    if(name == "[IDENT]")
            //    {
            //        return true;
            //    } else if(name == "[CONST]")
            //    {
            //        return true;
            //    } else if (new Regex("\".+\"").IsMatch(name))
            //    {
            //        return true;
            //    }
            //    return false;
            //});

            //Console.WriteLine($"Terminals: " +
            //    $"{string.Join(", ", grammar.Nodes.Where(n => n.IsTerminal).Select(n => n.Name))}");

            //Console.WriteLine($"Nonterminals: " +
            //    $"{string.Join(", ", grammar.Nodes.Where(n => !n.IsTerminal).Select(n => n.Name))}");

            //Console.WriteLine("Productions:\n" +
            //    string.Join("\n", 
            //        grammar.Productions.Select(p => 
            //            $"{string.Join("|", p.Right)}" +
            //            $" ::= " +
            //            $"{string.Join("|", p.Left)}"
            //            ))
            //        );
            //Console.WriteLine($"Is context free: {grammar.IsContextFree()}");

            //SimpleGrammarTest();
            FullGrammarTest();

            //var tableSize = 128;
           
            //var table = new SymbolTable(tableSize);
            //var pif = new ProgramInternalForm();

            //var scanner = new Scanner(table, pif);

            //using var program1 = new StreamReader("./program1.in");

            //scanner.Scan(program1);

            //File.WriteAllText("./symbol-table.log", table.ToString());
            //File.WriteAllText("./pif.log", "PIF: {\n" + string.Join(" ; ", pif.Pairs.Select(p => $"{p.Item2} = {p.Item1}")) + "}\n");
        }


        public static void SimpleGrammarTest()
        {
            var reader = new GrammarReader();

            var grammar = reader.ReadGrammar("./SimpleGrammar.in", (name) =>
            {
                if (name == "[SYM]")
                {
                    return true;
                }
                else if (name == "[CNST]")
                {
                    return true;
                }
                else if (new Regex("\".+\"").IsMatch(name))
                {
                    return true;
                }
                return false;
            });

            var parser = new Parser(grammar);

            var samplePif = new ProgramInternalForm();
            samplePif.Add(("ab", "SYM"));
            samplePif.Add(("=", "RES"));
            samplePif.Add(("12", "CNST"));
            samplePif.Add((".", "DEL"));

            var sampleTokens = new[] { "[SYM]", "\"=\"", "[CNST]", "\".\"" };
            //var sampleTokens = new[] { "NIL" };

            var result = parser.Accept(sampleTokens);
        }

        public static void FullGrammarTest()
        {
            var pif = new ProgramInternalForm();
            var symTable = new SymbolTable();
            var scanner = new Scanner(symTable, pif);
            var reader = new GrammarReader();
            var grammar = reader.ReadGrammar("./SimpleGrammar.in", (name) =>
            {
                return scanner.IsIdentifiableToken(name);
            });

            var parser = new Parser(grammar);

            using var program1 = new StreamReader("./program1.in");

            scanner.Scan(program1);

            var tokens = pif.Pairs
                .TakeWhile(p => p.Item1 != "STOP")
                    .Where(p => p.Item2 != "NAN")
                   .Select(p =>
                   {
                       if (p.Item2 == "SYM" || p.Item2 == "CNST")
                       {
                           return $"[{p.Item2}]";
                       }
                       else
                       {
                           return $"\"{p.Item1}\"".ToLower();
                       }
                   });

            var (isSuccess, productionList) = parser.Accept(tokens);

            var tree = ParseTree.FromProductions(productionList, grammar);

            foreach(var p in productionList)
            {
                var production = grammar.GetProduction(p);
                Console.WriteLine(production);
            }

            Console.WriteLine(tree);
        }
    }
}
