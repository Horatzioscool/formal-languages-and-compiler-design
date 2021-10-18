using System;
using System.Collections.Generic;
using System.Linq;
namespace FLTC.Lab2
{
    public interface IProgramInternalForm
    {
        void Add((string, int) pair);
    }

    internal class ProgramInternalForm : IProgramInternalForm
    {
        public readonly IList<(string, int)> Pairs = new List<(string, int)>();
        public void Add((string, int) pair)
        {
            Pairs.Add(pair);
        }
    }
}
