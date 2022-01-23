using System.Collections.Generic;
using System.Linq;
using System.Text;
using static FLTC.Lab2.Grammar.Grammar;

namespace FLTC.Lab2.Grammar
{
    public class ParseTree
    {
        private Node Root { get; set; }

        private class Node
        {
            private static int LastId = 0;
            public Node()
            {
                Id = LastId;
                LastId++;
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public Node Parent { get; set; }
            public Node RightSibling { get; set; }
            public Node LeftChild { get; set; }
        }

        private class ParseTreeBuilder
        {
            private readonly IEnumerable<int> parserProductions;
            private readonly Grammar grammar;
            public ParseTreeBuilder(IEnumerable<int> parserProductions, Grammar grammar)
            {
                this.parserProductions = parserProductions;
                this.grammar = grammar;
            }

            private int currentProductionIndex;
            public ParseTree Build()
            {
                var firstProductionId = parserProductions.First();
                var firstProduction = grammar.GetProduction(firstProductionId);

                var root = new Node
                {
                    Name = firstProduction.Left.First()
                };

                currentProductionIndex = 0;
                root.LeftChild = Expand(root, firstProduction);

                return new ParseTree
                {
                    Root = root
                };
            }

            private Node Expand(Node parent, Production production)
            {
                if (production == null || currentProductionIndex >= parserProductions.Count())
                    return null;

                var nodes = production.Right.Select(l =>
                {
                    var grammarNode = grammar.GetNode(l);

                    if (grammarNode.IsTerminal)
                    {
                        return new Node
                        {
                            Name = l,
                            Parent = parent
                        };
                    }

                    var node = new Node
                    {
                        Name = l,
                        Parent = parent
                    };

                    currentProductionIndex += 1;

                    if(currentProductionIndex < parserProductions.Count())
                    {
                        var nextProduction = grammar.GetProduction(parserProductions.ElementAt(currentProductionIndex));
                        node.LeftChild = Expand(node, nextProduction);
                    }

                    return node;

                }).ToList();

                for(int i = 0; i < nodes.Count() - 1; i++)
                {
                    nodes[i].RightSibling = nodes[i + 1];
                }

                return nodes.First();
            }

        }

        public static ParseTree FromProductions(IEnumerable<int> parserProductions, Grammar grammar)
        {
            return new ParseTreeBuilder(parserProductions, grammar).Build();
        }

        public override string ToString()
        {
            var queue = new Queue<Node>();
            queue.Enqueue(Root);

            var sb = new StringBuilder();

            do
            {
                var current = queue.Dequeue();
                if (current != null)
                {
                    if (current.Parent != null)
                    {
                        sb.Append($"{current.Id} {current.Name}\n\tP:{current.Parent.Id} {current.Parent.Name}\n\tLC:{current.LeftChild?.Id} {current.LeftChild?.Name ?? "X"}\n\tRS:{current.RightSibling?.Id} {current.RightSibling?.Name ?? "X"}\n");
                    } else
                    {
                        sb.Append($"{current.Id} {current.Name}\n\tLC:{current.LeftChild?.Id} {current.LeftChild?.Name ?? "X"}\n\tRS:{current.RightSibling?.Id} {current.RightSibling?.Name ?? "X"}\n");
                    }
                }
                if (current.LeftChild != null)
                {
                    queue.Enqueue(current.LeftChild);
                }
                if (current.RightSibling != null)
                {
                    queue.Enqueue(current.RightSibling);
                }
            } while (queue.Count() > 0);

            return sb.ToString();
        }
    }
}

