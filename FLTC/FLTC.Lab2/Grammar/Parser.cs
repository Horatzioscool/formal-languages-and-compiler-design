using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using static FLTC.Lab2.Grammar.Grammar;

namespace FLTC.Lab2.Grammar
{
    // LR 0 parser
    public class Parser
    {
        private Grammar Grammar { get; set; }

        public Parser(Grammar grammar)
        {
            Grammar = grammar;
        }

        public (bool, IEnumerable<int>) Accept(IEnumerable<string> tokens)
        {
            var workStack = new Stack<(string, int)>();
            var inputStack = new Stack<string>(tokens.Reverse());
            var exitStack = new Stack<int>();

            var states = GetCanonicalStateCollection();
            var table = GetParsingTable(states);

            workStack.Push(("", 0));

            do
            {
                (var symbol, var state) = workStack.Peek();
                var canonicalState = states.First(s => s.Id == state);
                var action = table.Actions[state];

                switch (action)
                {
                    case ParsingTable.TableAction.Shift:
                        {
                            var token = inputStack.Pop();
                            var nextState = table.GoTos[(state, token)];
                            workStack.Push((token, nextState));
                            break;
                        }
                    case ParsingTable.TableAction.Reduce:
                        {
                            var reduction = table.Reductions[state];
                            var production = Grammar.GetProduction(reduction);
                            var left = production.Left.First();
                            foreach (var i in Enumerable.Range(0, production.Right.Length))
                            {
                                workStack.Pop();
                            }
                            exitStack.Push(production.ProductionID);
                            var top = workStack.Peek();
                            var nextState = table.GoTos[(top.Item2, left)];
                            workStack.Push((left, nextState));
                            break;
                        }
                    case ParsingTable.TableAction.Accept:
                        {
                            Console.WriteLine(string.Join(" ", exitStack));
                            return (true, exitStack);
                        }
                }
            } while (true);
        }

        private class ParserElementComparer : IEqualityComparer<ParserElement>
        {
            public bool Equals(ParserElement x, ParserElement y)
            {
                if (x is null && y is null)
                {
                    return true;
                }
                else if (x is null || y is null)
                {
                    return false;
                }

                return x != null && y != null && Enumerable.SequenceEqual(x.Production.Left, y.Production.Left) &&
                       Enumerable.SequenceEqual(x.Production.Right, y.Production.Right)
                       && x.DotPointer == y.DotPointer;
            }

            public int GetHashCode([DisallowNull] ParserElement obj)
            {
                var leftHashCode = obj.Production.Left.Sum(s => s.GetHashCode() % 100);
                var rightHashCode = obj.Production.Right.Sum(s => s.GetHashCode() % 100);

                return leftHashCode * 1000000 + rightHashCode;
            }
        }

        private static IEqualityComparer<ParserElement> Comparer = new ParserElementComparer();

        private ParsingTable GetParsingTable(IEnumerable<ParserState> canonicalStates)
        {
            ParsingTable table = new ParsingTable();

            foreach(var state in canonicalStates)
            {
                if (state.Elements.Any(e => e.HasDotAtTheEnd() && Grammar.IsStartProduction(e.Production)))
                {
                    table.Actions.Add(state.Id, ParsingTable.TableAction.Accept);
                }
                else if (!state.Elements.Any(e => e.HasDotAtTheEnd()))
                {
                    table.Actions.Add(state.Id, ParsingTable.TableAction.Shift);
                    foreach(var goTo in state.GoTos)
                    {
                        table.GoTos.Add((state.Id, goTo.Key), goTo.Value.Id);
                    }
                } 
                else
                {
                    var lastElement = state.Elements.First(e => e.HasDotAtTheEnd());
                    var production = lastElement.Production;
                    table.Actions.Add(state.Id, ParsingTable.TableAction.Reduce);
                    table.Reductions.Add(state.Id, production.ProductionID);
                }
            }

            return table;
        }

        private IEnumerable<ParserState> GetCanonicalStateCollection()
        {
            var states = new List<ParserState>();
            var nodes = Grammar.Nodes.Select(n => n.Name);

            var startProductions = Grammar.GetStartProductions();

            var startElements = startProductions.Select(p => new ParserElement
            {
                Production = p,
                DotPointer = 0
            });

            var startState = new ParserState
            {
                Elements = Closure(startElements)
            };

            states.Add(startState);

            var lastNewStates = new HashSet<ParserState>(states, StateComparer);
            var newStates = new HashSet<ParserState>(StateComparer);
            do
            {
                newStates = new HashSet<ParserState>(StateComparer);
                foreach (var state in lastNewStates)
                {
                    foreach(var node in nodes)
                    {
                        var result = GoTo(state, node);

                        if (result.Any())
                        {
                            var goToResultState = new ParserState()
                            {
                                Elements = result
                            };
                            state.GoTos[node] = goToResultState;
                            newStates.Add(goToResultState);
                        }
                    }
                }
                newStates = new HashSet<ParserState>(newStates.Where(ls => !states.Any(s => StateComparer.Equals(ls, s))), StateComparer);
                states.AddRange(newStates);
                lastNewStates = newStates;
            } while (newStates.Any());


            foreach(var state in states)
            {
                foreach(var goTo in state.GoTos)
                {
                    if (!states.Contains(goTo.Value))
                    {
                        state.GoTos.Remove(goTo);
                    }
                }
            }

            return states;
        }

        private IEnumerable<ParserElement> Closure(IEnumerable<ParserElement> elements)
        {
            var closureElements = new HashSet<ParserElement>(elements, Comparer);
            var previousElementsToExpand = new HashSet<ParserElement>(elements, Comparer);
            var elementsToExpand = new HashSet<ParserElement>(Comparer);

            do
            {
                elementsToExpand = new HashSet<ParserElement>(Comparer);
                foreach (var element in previousElementsToExpand)
                {
                    if (CanExpand(element))
                    {
                        var newElementsToExpand = Expand(element);
                        elementsToExpand.UnionWith(newElementsToExpand);
                    }
                }
                previousElementsToExpand = new HashSet<ParserElement>(elementsToExpand.Except(closureElements, Comparer), Comparer);
                closureElements.UnionWith(elementsToExpand);
            } while (elementsToExpand.Any());

            return closureElements;
        }

        private bool CanExpand(ParserElement element)
        {
            var current = element.GetCurrentNode();

            if(current == null)
                return false;

            if (Grammar.IsTerminalNode(current))
                return false;

            return true;
        }

        private IEnumerable<ParserElement> Expand(ParserElement element)
        {
            var current = element.GetCurrentNode();

            var productions = Grammar.GetProductionsForLeft(current);

            return productions.Select(p => new ParserElement
            {
                Production = p,
                DotPointer = 0
            });
        }

        private IEnumerable<ParserElement> GoTo(ParserState state, string node)
        {
            var resultingElements = state.Elements
                .Where(e => HasDotInFrontOf(e, node))
                .Select(e => e.GoTo());

            resultingElements = resultingElements.Where(e => e != null);

            if (!resultingElements.Any())
            {
                return Enumerable.Empty<ParserElement>();
            }

            return Closure(resultingElements);
        }

        private bool HasDotInFrontOf(ParserElement element, string node)
        {
            return element.GetCurrentNode() == node;
        }

        public class ParserElement
        {
            public Production Production { get; set; }
            public int DotPointer = 0;

            public ParserElement GoTo()
            {
                if (DotPointer >= Production.Right.Length)
                {
                    return null;
                }

                return new ParserElement
                {
                    Production = this.Production,
                    DotPointer = this.DotPointer + 1
                };
            }

            public bool HasDotAtTheEnd()
            {
                return DotPointer >= Production.Right.Length;
            }

            public string GetCurrentNode()
            {
                if (DotPointer >= Production.Right.Length)
                {
                    return null;
                }

                return Production.Right[DotPointer];
            }

            public override string ToString()
            {
                var rightProductions = new StringBuilder();

                for(int i = 0; i < Production.Right.Length; i++)
                {
                    if (i == DotPointer)
                    {
                        rightProductions.Append(".");
                    }
                    rightProductions.Append(Production.Right[i]);
                }

                if (Production.Right.Length == DotPointer)
                {
                    rightProductions.Append(".");
                }

                return $"{Production.Left.First()} -> {rightProductions}";
            }
        }

        public class ParserState 
        {
        
            private static int LastId = -1;

            public ParserState()
            {
                Id = LastId + 1;
                LastId++;
            }

            public int Id { get; set; }
            public IEnumerable<ParserElement> Elements;
            public IDictionary<string, ParserState> GoTos = new Dictionary<string, ParserState>();

            public override string ToString()
            {
                return string.Join(", ", Elements.Select(e => e.ToString()));
            }
        }

        private static IEqualityComparer<ParserState> StateComparer = new ParserStateComparer();

        private class ParserStateComparer : IEqualityComparer<ParserState>
        {
            public bool Equals(ParserState x, ParserState y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                return Enumerable.SequenceEqual(x.Elements, y.Elements, Comparer);
            }

            public int GetHashCode([DisallowNull] ParserState obj)
            {
                return obj.Elements.Select(e => e.GetHashCode()).Sum();
            }
        }
    }
}
