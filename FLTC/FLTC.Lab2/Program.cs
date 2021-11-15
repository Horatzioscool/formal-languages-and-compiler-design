using FLTC.Lab2.FiniteAutomata;
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

            var reader = new GrammarReader();

            var grammar = reader.ReadGrammar("./SimpleSyntax.in", (name) =>
            {
                if(name == "[IDENT]")
                {
                    return true;
                } else if(name == "[CONST]")
                {
                    return true;
                } else if (new Regex("\".+\"").IsMatch(name))
                {
                    return true;
                }
                return false;
            });

            Console.WriteLine($"Terminals: " +
                $"{string.Join(", ", grammar.Nodes.Where(n => n.isTerminal).Select(n => n.Name))}");

            Console.WriteLine($"Nonterminals: " +
                $"{string.Join(", ", grammar.Nodes.Where(n => !n.isTerminal).Select(n => n.Name))}");

            Console.WriteLine("Productions:\n" +
                string.Join("\n", 
                    grammar.Productions.Select(p => 
                        $"{string.Join("|", p.Right)}" +
                        $" ::= " +
                        $"{string.Join("|", p.Left)}"
                        ))
                    );
            Console.WriteLine($"Is context free: {grammar.IsContextFree()}");

            //var tableSize = 128;
           
            //var table = new SymbolTable(tableSize);
            //var pif = new ProgramInternalForm();

            //var scanner = new Scanner(table, pif);

            //using var program1 = new StreamReader("./program1.in");

            //scanner.Scan(program1);

            //File.WriteAllText("./symbol-table.log", table.ToString());
            //File.WriteAllText("./pif.log", "PIF: {\n" + string.Join(" ; ", pif.Pairs.Select(p => $"{p.Item2} = {p.Item1}")) + "}\n");
        }
    }
}
