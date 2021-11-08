using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FLTC.Lab2.FiniteAutomata
{
    public class FiniteAutomatonMenu
    {
        private FiniteAutomatonViewer Viewer { get; set; }
        private FiniteAutomaton Selected { get; set; }

        private string Menu => string.Join("\n", new [] {
                "1. Read automata",
                "2. Show states",
                "3. Show alphabet",
                "4. Show transitions",
                "5. Show final states",
                "6. Test automaton",
                "7. Check is deterministic",
        });
        public void Start()
        {
            var reader = new FiniteAutomatonReader();

            while (true)
            {
                Console.WriteLine(Menu);
                var input = Console.ReadLine();
                input = Regex.Replace(input, "[^0-9]+", "");

                switch (input)
                {
                    case "1":
                        {
                            var file = Console.ReadLine();
                            Selected = reader.ReadFromFile(file);
                            Viewer = new FiniteAutomatonViewer(Selected);
                            continue;
                        }
                    case "2":
                        {
                            Console.WriteLine(string.Join(", ", Viewer.States));
                            continue;
                        }
                    case "3":
                        {
                            Console.WriteLine(string.Join(", ", Viewer.Alphabet));
                            continue;
                        }
                    case "4":
                        {
                            Console.Write(string.Join("\n", Viewer.Transitions.Select(t => $"{t.Item1} -({t.Item3})-> {t.Item2}")));
                            continue;
                        }
                    case "5":
                        {
                            Console.WriteLine(string.Join(", ", Viewer.FinalStates));
                            continue;
                        }
                    case "6":
                        {
                            Console.WriteLine("Input string:");
                            var inputString = Console.ReadLine().Trim();
                            Console.WriteLine(Selected.Test(inputString));
                            continue;
                        }
                    case "7":
                        {
                            Console.WriteLine($"Automaton is deterministic: {Viewer.IsDeterministic}");
                            continue;
                        }
                    default:
                        {
                            Console.WriteLine("Please input a valid option");
                            continue;
                        }
                }
            }
        }

        private abstract class MenuItem
        {
            protected string Name { get; }
            protected string Description { get; }

            protected MenuItem(string name, string description)
            {
                Name = name;
                Description = description;
            }

            public abstract void Execute();
        }

        private class ReadAutomata : MenuItem
        {
            public ReadAutomata() : base("1", "Read the automata") {}

            public override void Execute()
            {
                throw new System.NotImplementedException();
            }
        }

        private class PrintStates : MenuItem
        {
            public PrintStates() : base("1", "Read the automata") { }

            public override void Execute()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
