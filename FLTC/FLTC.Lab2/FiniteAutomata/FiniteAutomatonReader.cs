using System.IO;
using System.Text.RegularExpressions;

namespace FLTC.Lab2.FiniteAutomata
{
    public class FiniteAutomatonReader
    {

        private Regex StatesRegex = new("(states:(\\r)?\\n(?'States'(\\w+(\\r)?\\n)+))");
        private Regex InitialStatesRegex = new("(initial:(\\r)?\\n(?'Initial'(\\w+(\\r)?\\n)+))");
        private Regex FinalStatesRegex = new("(final:(\\r)?\\n(?'Final'(\\w+(\\r)?\\n)+))");
        private Regex Transitions = new("(transitions:(\\r)?\\n(?'Transitions'((\\w+)-\\((\\w+)\\)->(\\w+)((\\r)?\\n)?)+))");

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
                automata.AddInputState(state);
            }

            var finalStates = match.Groups["Final"].Value.Split("\r\n", options: System.StringSplitOptions.RemoveEmptyEntries);

            foreach (var state in finalStates)
            {
                automata.AddFinalState(state);
            }

            var transitions = match.Groups["Transitions"].Value.Split("\r\n", options: System.StringSplitOptions.RemoveEmptyEntries);

            foreach (var transition in initialStates)
            {
                var transitionRegex = new Regex("(\\w+)-\\((\\w+)\\)->(\\w+)");
                var tmatch = transitionRegex.Match(transition);

                var source = tmatch.Groups[0].Value;
                var label = tmatch.Groups[1].Value;
                var dest = tmatch.Groups[2].Value;

                automata.AddTransition(source, dest, label);
            }

            return automata;
        }
    }
}
