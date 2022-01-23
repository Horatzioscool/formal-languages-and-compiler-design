using System.Collections.Generic;

namespace FLTC.Lab2.Grammar
{
    public class ParsingTable
    {
        public IDictionary<int, TableAction> Actions = new Dictionary<int, TableAction>();
        public IDictionary<(int, string), int> GoTos = new Dictionary<(int, string), int>();
        public IDictionary<int, int> Reductions = new Dictionary<int, int>();

        public enum TableAction
        {
            Shift,
            Accept,
            Reduce
        }
    }
}
