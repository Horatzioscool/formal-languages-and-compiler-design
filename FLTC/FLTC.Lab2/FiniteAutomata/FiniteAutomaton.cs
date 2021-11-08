using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FLTC.Lab2.FiniteAutomata
{
    public class FiniteAutomaton
    {
        internal IDictionary<string, State> States { get; set; } = new Dictionary<string, State>();
        internal IList<Transition> Transitions { get; set; } = new List<Transition>();

        internal record State(string id)
        {
            public bool isInput { get; set; } = false;
            public bool isFinal { get; set; } = false;
        }

        internal record Transition(string source, string destination, string label)
        {
            public const string StartState = "start";
            public const string FinalState = "final";
            public const string FinalTransitionLabel = "final";
            public bool IsInput => source == StartState;
            public bool IsFinal => destination == FinalState;
        }

        public void AddState(string id)
        {
            States.Add(id, new State(id));
        }

        public void AddInputState(string id)
        {
            if(States.TryGetValue(id, out var value)){
                value.isInput = true;
            }
            else
            {
                States.Add(id, new State(id)
                {
                    isInput = true
                });
            }
        }

        public void AddFinalState(string id)
        {
            if (States.TryGetValue(id, out var value))
            {
                value.isFinal = true;
            }
            else
            {
                States.Add(id, new State(id)
                {
                    isFinal = true
                });
            }
            Transitions.Add(new Transition(id, Transition.FinalState, Transition.FinalTransitionLabel));
        }

        public void AddTransition(string source, string destination, string label)
        {
            Transitions.Add(new Transition(source, destination, label));
        }

        private bool Test(string input, int charIndex, string currentState)
        {
            if(charIndex >= input.Length)
            {
                return States[currentState].isFinal;
            }

            var transitions = Transitions.Where(t => t.source == currentState && new Regex(t.label).IsMatch(input.Substring(charIndex, 1)));

            return States[currentState].isFinal && charIndex >= input.Length || transitions.Any(t => Test(input, charIndex + 1, t.destination));
        }

        public bool Test(string input)
        {
            return States.Values
                .Where(s => s.isInput)
                .Any(s => Test(input, 0, s.id));
        }
    }
}
