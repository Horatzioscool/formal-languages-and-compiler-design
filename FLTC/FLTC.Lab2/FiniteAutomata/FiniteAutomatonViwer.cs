using System.Collections.Generic;
using System.Linq;
using static FLTC.Lab2.FiniteAutomata.FiniteAutomaton;

namespace FLTC.Lab2.FiniteAutomata
{
    public class FiniteAutomatonViewer
    {
        private readonly FiniteAutomaton automata;
        public FiniteAutomatonViewer(FiniteAutomaton automata)
        {
            this.automata = automata;
        }

        public IEnumerable<string> States => automata.States
            .Select(s => s.id);
        public IEnumerable<string> Alphabet => automata.Transitions
            .Where(t => t.label != Transition.FinalTransitionLabel)
            .Select(t => t.label)
            .ToHashSet();

        public IEnumerable<(string, string, string)> Transitions => automata.Transitions
            .Select(t => (t.source, t.destination, t.label));

        public IEnumerable<string> FinalStates => automata.States
            .Where(s => s.isFinal)
            .Select(s => s.id);
    }
}
