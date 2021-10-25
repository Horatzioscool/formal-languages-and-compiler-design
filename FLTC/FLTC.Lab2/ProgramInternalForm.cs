using System;
using System.Collections.Generic;
using System.Linq;
namespace FLTC.Lab2
{
    public interface IProgramInternalForm
    {
        void Add((string, string) pair);
    }

    internal class ProgramInternalForm : IProgramInternalForm
    {
        public readonly IList<(string, string)> Pairs = new List<(string, string)>();
        public void Add((string, string) pair)
        {
            Pairs.Add(pair);
        }
    }
}
