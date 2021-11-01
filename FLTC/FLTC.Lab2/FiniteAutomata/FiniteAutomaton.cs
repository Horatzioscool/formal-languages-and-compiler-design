using System.Collections.Generic;

namespace FLTC.Lab2.FiniteAutomata
{
    public class FiniteAutomaton
    {
        internal ISet<State> States { get; set; } = new HashSet<State>();
        internal IList<Transition> Transitions { get; set; } = new List<Transition>();

        internal record State(string id, bool isInput = false, bool isFinal = false)
        {}

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
            States.Add(new State(id));
        }

        public void AddInputState(string id)
        {
            States.Add(new State(id, true));
        }

        public void AddFinalState(string id)
        {
            States.Add(new State(id, false, true));
            Transitions.Add(new Transition(id, Transition.FinalState, Transition.FinalTransitionLabel));
        }

        public void AddTransition(string source, string destination, string label)
        {
            Transitions.Add(new Transition(source, destination, label));
        }
    }
}
