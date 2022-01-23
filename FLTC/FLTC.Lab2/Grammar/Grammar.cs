using System.Collections.Generic;
using System.Linq;

namespace FLTC.Lab2.Grammar
{
    public class Grammar
    {
        public class Node
        {
            public const string StartNodePrefix = "[START]";
            public const string EmptyString = "NIL";

            public string Name { get; set; }

            public bool IsTerminal { get; set; }
            public bool IsStarting { get; set; }
        }

        public class Production
        {
            public int ProductionID { get; set; }
            public string[] Left { get; set; }
            public string[] Right { get; set; }

            public override string ToString()
            {
                return $"{string.Join("|", Left)} -> {string.Join(",", Right)}";
            }
        }

        public IList<Node> Nodes { get; set; } = new List<Node>();
        public IList<Production> Productions { get; set; } = new List<Production>();

        public Node GetNode(string name)
        {
            return Nodes.FirstOrDefault(n => n.Name == name);
        }

        public Production GetProduction(int id)
        {
            return Productions.FirstOrDefault(p => p.ProductionID == id);
        }

        public IEnumerable<Production> GetStartProductions()
        {
            var startNodeNames = Nodes.Where(n => n.IsStarting).Select(n => n.Name);
            return Productions.Where(p => p.Left.Intersect(startNodeNames).Any());
        }

        public IEnumerable<Production> GetProductionsForLeft(string left)
        {
            return Productions.Where(p => p.Left.Contains(left));
        }
        
        public bool IsStartProduction(Production production)
        {
            var node = Nodes.FirstOrDefault(n => n.Name == production.Left.FirstOrDefault());

            return node.IsStarting;
        }

        public bool IsTerminalNode(string node)
        {
            return Nodes.FirstOrDefault(n => n.Name == node).IsTerminal;
        }

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
                    IsStarting = true
                });
            }
            else
            {
                n.IsStarting = true;
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
                    IsTerminal = true
                });
            }
            else
            {
                n.IsTerminal = true;
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
