using System.Collections.Generic;
using System.Linq;
using static FLTC.Lab2.FiniteAutomata.FiniteAutomaton;

namespace FLTC.Lab2.FiniteAutomata
{
    public class FiniteAutomatonViewer
    {
        private readonly FiniteAutomaton automaton;
        public FiniteAutomatonViewer(FiniteAutomaton automata)
        {
            this.automaton = automata;
        }

        public IEnumerable<string> States => automaton.States
            .Values
            .Select(s => s.id);
        public IEnumerable<string> Alphabet => automaton.Transitions
            .Where(t => t.label != Transition.FinalTransitionLabel)
            .Select(t => t.label)
            .ToHashSet();

        public IEnumerable<(string, string, string)> Transitions => automaton.Transitions
            .Select(t => (t.source, t.destination, t.label));

        public IEnumerable<string> FinalStates => automaton.States
            .Values
            .Where(s => s.isFinal)
            .Select(s => s.id);

        public bool IsDeterministic => automaton.States.Values.Any(s =>
                automaton.Transitions.Where(t => t.source == s.id)
                .GroupBy(t => t.label)
                .Any(g => g.Count() > 1)
                );

        public bool IsDFA => automaton.States.Values.Any(s =>
            automaton.Transitions.Where(t => t.source == s.id).Count() > 1
        );
    }
}
