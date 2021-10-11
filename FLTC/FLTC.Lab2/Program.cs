using System;

namespace FLTC.Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var tokens = new[]
            {
                "thus",
                "ab",
                "is",
                "numeric",
                ".",
                "ab",
                "shall",
                "be",
                "read",
                ".",
                "thus",
                "ba",
                "is",
                "numeric",
                "."
            };

            var tableSize = 128;

            ISymbolTable table = new SymbolTable(tableSize);

            foreach(var token in tokens)
            {
                var pos = table.Insert(token);

                Console.WriteLine($"({pos.Item1}, {pos.Item2})");
            }
        }
    }
}
