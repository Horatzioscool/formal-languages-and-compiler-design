using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FLTC.Lab2.FiniteAutomata
{
    public class FiniteAutomatonReader
    {

        private Regex StatesRegex = new("(states:(\\r)?\\n(?'States'(\\w+(\\r)?\\n)+))");
        private Regex InitialStatesRegex = new("(initial:(\\r)?\\n(?'Initial'(\\w+(\\r)?\\n)+))");
        private Regex FinalStatesRegex = new("(final:(\\r)?\\n(?'Final'(\\w+(\\r)?\\n)+))");
        private Regex Transitions = new("(transitions:(\\r)?\\n(?'Transitions'((\\w+)-\\((.+)\\)->(\\w+)((\\r)?\\n)?)+))");

        public FiniteAutomaton ReadFromFile(string path)
        {
            var reg = new Regex(
            StatesRegex.ToString() +
            InitialStatesRegex.ToString() +
            FinalStatesRegex.ToString() +
            Transitions.ToString()
            );

            var automata = new FiniteAutomaton();

            var text = File.ReadAllText(path);

            var match = reg.Match(text);

            if (!match.Success)
                throw new System.Exception("File did match expected");

            var states = match.Groups["States"].Value.Split("\r\n", options: System.StringSplitOptions.RemoveEmptyEntries);

            foreach(var state in states)
            {
                automata.AddState(state);
            }

            var initialStates = match.Groups["Initial"].Value.Split("\r\n", options: System.StringSplitOptions.RemoveEmptyEntries);

            foreach(var state in initialStates)
            {
                if (!states.Contains(state))
                    throw new ApplicationException("Initial state not in states list!");
                automata.AddInputState(state);
            }

            var finalStates = match.Groups["Final"].Value.Split("\r\n", options: System.StringSplitOptions.RemoveEmptyEntries);

            foreach (var state in finalStates)
            {
                automata.AddFinalState(state);
                if (!states.Contains(state))
                    throw new ApplicationException("Final state not in states list!");
            }

            var transitions = match.Groups["Transitions"].Value.Split("\r\n", options: System.StringSplitOptions.RemoveEmptyEntries);

            foreach (var transition in transitions)
            {
                var transitionRegex = new Regex("(\\w+)-\\((.+)\\)->(\\w+)");
                var tmatch = transitionRegex.Match(transition);

                var source = tmatch.Groups[1].Value;
                var label = tmatch.Groups[2].Value;
                var dest = tmatch.Groups[3].Value;

                automata.AddTransition(source, dest, label);
            }

            return automata;
        }
    }
}
