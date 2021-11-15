using System.Collections.Generic;
using System.Linq;

namespace FLTC.Lab2.Grammar
{
    internal class Grammar
    {
        public class Node
        {
            public const string StartNodePrefix = "[START]";
            public const string EmptyString = "NIL";

            public string Name { get; set; }

            public bool isTerminal { get; set; }
            public bool isStarting { get; set; }
        }

        public class Production
        {
            public int ProductionID { get; set; }
            public string[] Left { get; set; }
            public string[] Right { get; set; }
        }

        public IList<Node> Nodes { get; set; } = new List<Node>();
        public IList<Production> Productions { get; set; } = new List<Production>();

        public void AddNode(string name)
        {
            if(!Nodes.Any(n => n.Name == name))
            {
                Nodes.Add(new Node
                {
                    Name = name,
                });
            }
        }

        public void AddStarting(string name)
        {
            var n = Nodes.FirstOrDefault(n => n.Name == name);
            if (n == null)
            {
                Nodes.Add(new Node
                {
                    Name = name,
                    isStarting = true
                });
            }
            else
            {
                n.isStarting = true;
            }
        }

        public void AddTerminal(string name)
        {
            var n = Nodes.FirstOrDefault(n => n.Name == name);
            if (n == null)
            {
                Nodes.Add(new Node
                {
                    Name = name,
                    isTerminal = true
                });
            }
            else
            {
                n.isTerminal = true;
            }
        }

        public void AddProduction(string[] right, string[] left)
        {
            if(!Productions.Any(p =>Enumerable.SequenceEqual(p.Right, right) && Enumerable.SequenceEqual(p.Left, left)))
            {
                Productions.Add(new Production
                {
                    ProductionID = Productions.Count(),
                    Left = left,
                    Right = right
                });
            }
        }

        public bool IsContextFree()
        {
            return !Productions.Any(p => p.Right.Count() > 1);
        }
    }
}
