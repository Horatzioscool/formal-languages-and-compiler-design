using System;
using System.IO;
using System.Linq;

namespace FLTC.Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var testString = "-1";

            Console.WriteLine(new ConstantTokenType().constantRegex.IsMatch(testString));

            var tableSize = 128;
           
            var table = new SymbolTable(tableSize);
            var pif = new ProgramInternalForm();

            var scanner = new Scanner(table, pif);

            using var program1 = new StreamReader("./program1.in");

            scanner.Scan(program1);

            File.WriteAllText("./symbol-table.log", table.ToString());
            File.WriteAllText("./pif.log", "PIF: {\n" + string.Join(" ; ", pif.Pairs.Select(p => $"{p.Item2} = {p.Item1}")) + "}\n");
        }
    }
}
